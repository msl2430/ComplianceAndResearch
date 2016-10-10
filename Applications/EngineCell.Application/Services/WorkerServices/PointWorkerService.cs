using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IPointWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public class PointWorkerService : BaseWorkerThreadService, IPointWorkerService
    {
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }
        private TestDisplayViewModel TestDisplayViewModel { get; set; }
        private IWidgetRepository WidgetRepository { get; set; }

        private ScratchPadConstants.FloatIndexes TempPoint { get; set; }
        private DateTime CaptureTime { get; set; }

        public PointWorkerService(TestDisplayViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            TestDisplayViewModel = viewModel;
            ApplicationSessionFactory = viewModel.ApplicationSessionFactory;
            WidgetRepository = new WidgetRepository();            
        }

        public override void DoWork()
        {

            var appSession = ApplicationSessionFactory;
            CancellationToken = new CancellationTokenSource();
            ApplicationSessionFactory.LogEvent("Point data collection started.", true);
            ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 1);
            while (!CancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (appSession.OptoMmpFactory != null && appSession.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            ConfigurePacWidgets();
                            CaptureTime = DateTime.Now;
                            foreach (var cellPoint in appSession.CellPoints.Where(cp => !cp.IsCustomValue))
                            {
                                if (cellPoint.IsAnalog)
                                {
                                    TempPoint = (ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                                    Dispatcher.Invoke(() =>
                                    {
                                        cellPoint.Data = Math.Truncate(appSession.ScratchPadFactory.GetScratchPadFloat(TempPoint.ToInt()).Value*10000m)/10000m;
                                    });
                                    if (!cellPoint.IsAverage || cellPoint.AverageSeconds == null || cellPoint.AverageSeconds.Value <= 0)
                                        continue;
                                    Dispatcher.Invoke(() =>
                                    {
                                        cellPoint.MostRecentData.Add(cellPoint.Data);
                                        if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds*2) //2 because we're capturing 2 data points per second
                                            cellPoint.MostRecentData.RemoveAt(0);
                                        cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m;
                                    });                                    
                                }
                                else
                                {
                                    TempPoint = (ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                                    Dispatcher.Invoke(() =>
                                    {
                                        cellPoint.Data = appSession.ScratchPadFactory.GetScratchPadFloat(TempPoint.ToInt()).Value > 0 ? 1m : 0m;
                                    });
                                }

                                if (cellPoint.IsInput) continue;

                                TempPoint = (ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true);
                                appSession.ScratchPadFactory.SetScratchPadValue(TempPoint.ToInt(), ScratchPadConstants.DefaultNullValue);
                            }
                            foreach (var cellPoint in appSession.CellPoints.Where(cp => cp.IsCustomValue))
                            {
                                TempPoint = (ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true);
                                appSession.ScratchPadFactory.SetScratchPadValue(TempPoint.ToInt(), cellPoint.CustomValue.Value);
                                Dispatcher.Invoke(() =>
                                {
                                    cellPoint.Data = cellPoint.CustomValue.Value;
                                    cellPoint.MostRecentData.Add(cellPoint.Data);
                                    if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds * 2) //2 because we're capturing 2 data points per second
                                        cellPoint.MostRecentData.RemoveAt(0);
                                    cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m;
                                });                                
                            }

                            if (appSession.CurrentCellTest != null &&
                                TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(
                                    ScratchPadConstants.IntegerIndexes.TestRunning.ToInt()).Value == 1)
                            {
                                ExportService.WriteDataToFile(appSession.CurrentCellTest.CellTestId, CaptureTime, appSession.CellPoints.Where(cp => cp.IsRecord).ToList());
                                CellPointRepository.CreateCellPointData(
                                    appSession.CellPoints.Where(cp => cp.IsRecord)
                                        .Select(cp => cp.ToCellTestPointDataModel(appSession.CurrentCellTest.CellTestId, DateTime.Now))
                                        .ToList());
                            }

                            TestDisplayViewModel.UpdateVisibleCellPoints();

                            TestDisplayViewModel.VentControl1Display.Inside = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Inside.ToInt()).Value;
                            TestDisplayViewModel.VentControl1Display.Outside = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Outside.ToInt()).Value;
                            TestDisplayViewModel.VentControl1Display.Output = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Output.ToInt()).Value;
                        }
                    }
                    Thread.Sleep(500);
                }
                catch (TaskCanceledException ex)
                {
                    //do nothing
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => ApplicationSessionFactory.LogEvent("Error in point data collection: " + ex.Message));
                }
            }
            WorkCompleted();
        }

        private VentilationControlViewModel Settings { get; set; }
        private IList<WidgetSettingValueModel> VentCtrl1WidgetSettings { get; set; }

        private void ConfigurePacWidgets()
        {
            //TODO: All widget settings
            VentCtrl1WidgetSettings = WidgetRepository.GetWidgetSettingByWidgetCell(ApplicationSessionFactory.CurrentCellId, WidgetConstants.Widget.VentilationControl1);

            if (!VentCtrl1WidgetSettings.IsNotNullOrEmpty()) return;
            if(Settings == null)
                Settings = new VentilationControlViewModel();
            Settings.SetValues(VentCtrl1WidgetSettings);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1InsideType.ToInt(), Settings.Inside.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutsideType.ToInt(), Settings.Outside.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutputType.ToInt(), Settings.Output.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Setpoint.ToInt(), Settings.SetPoint);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Gain.ToInt(), Settings.Gain);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1Status.ToInt(), Settings.IsActive ? 1 : 0);
        }

        protected override void WorkCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 0);
                ApplicationSessionFactory.LogEvent("Point data collection stopped.", true);
            });
        }
    }
}
