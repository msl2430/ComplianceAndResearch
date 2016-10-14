using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;
using Opto22.Core.Models;

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
                    while (!CancellationToken.IsCancellationRequested)
                    {
                        if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                        if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();

                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (appSession.OptoMmpFactory != null && appSession.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            ConfigurePacWidgets();
                            CaptureTime = DateTime.Now;
                            var taskList = new List<Task>();
                            var scratchPadValues = appSession.ScratchPadFactory.GetScratchPadFloatRange(1000, 1073);
                            foreach (var cellPoint in appSession.CellPoints)
                            {
                                var task = new Task(() =>
                                {
                                    CaptureCellPoint(cellPoint, scratchPadValues.FirstOrDefault(sc => sc.Index == ((ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName, true)).ToInt()), appSession);
                                });
                                taskList.Add(task);
                                task.Start();
                            }

                            Task.WaitAll(taskList.ToArray());

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

                            Dispatcher.Invoke(() => {
                                TestDisplayViewModel.UpdateVisibleCellPoints();
                            });

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

        private static void CaptureCellPoint(PointDataModel cellPoint, IScratchPadModel<decimal> scratchPadValue, IApplicationSessionFactory appSession)
        {
            if (scratchPadValue == null)
                return;
            if (!cellPoint.IsCustomValue)
            {
                if (cellPoint.IsAnalog)
                {
                    cellPoint.Data = Math.Truncate(scratchPadValue.Value * 10000m) / 10000m;
                    if (!cellPoint.IsAverage || cellPoint.AverageSeconds == null || cellPoint.AverageSeconds.Value <= 0)
                        return;
                    cellPoint.MostRecentData.Add(cellPoint.Data);
                    if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds * 2) //2 because we're capturing 2 data points per second
                        cellPoint.MostRecentData.RemoveAt(0);
                    cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m;
                }
                else
                {
                    cellPoint.Data = scratchPadValue.Value > 0 ? 1m : 0m;
                }

                if (cellPoint.IsInput) return;

                var customTempPoint = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true);
                appSession.ScratchPadFactory.SetScratchPadValue(customTempPoint.ToInt(), ScratchPadConstants.DefaultNullValue);
            }
            else
            {
                var tempPoint = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true);
                appSession.ScratchPadFactory.SetScratchPadValue(tempPoint.ToInt(), cellPoint.CustomValue.Value);
                cellPoint.Data = cellPoint.CustomValue.Value;
                cellPoint.MostRecentData.Add(cellPoint.Data);
                if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds * 2) //2 because we're capturing 2 data points per second
                    cellPoint.MostRecentData.RemoveAt(0);
                cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m;
            }
        }

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
