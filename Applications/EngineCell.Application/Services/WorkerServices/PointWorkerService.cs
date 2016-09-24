using System;
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

        public PointWorkerService(TestDisplayViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            TestDisplayViewModel = viewModel;
            ApplicationSessionFactory = viewModel.ApplicationSessionFactory;
            WidgetRepository = new WidgetRepository();            
        }

        public override void DoWork()
        {
            try
            {
                var appSession = ApplicationSessionFactory;
                CancellationToken = new CancellationTokenSource();
                ApplicationSessionFactory.LogEvent("Point data collection started.", true);
                ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 1);
                while (!CancellationToken.IsCancellationRequested)
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
                            var captureTime = DateTime.Now;
                            foreach (var cellPoint in appSession.CellPoints.Where(cp => !cp.IsCustomValue))
                            {
                                if (cellPoint.IsAnalog)
                                {
                                    var point = (ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                                    cellPoint.Data = Math.Truncate(appSession.ScratchPadFactory.GetScratchPadFloat(point.ToInt()).Value*10000m)/10000m;
                                    if (!cellPoint.IsAverage || cellPoint.AverageSeconds == null || cellPoint.AverageSeconds.Value <= 0)
                                        continue;

                                    cellPoint.MostRecentData.Add(cellPoint.Data);
                                    if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds*2) //2 because we're capturing 2 data points per second
                                        cellPoint.MostRecentData.RemoveAt(0);

                                    cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average()*10000m)/10000m;
                                }
                                else
                                {
                                    var point = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                                    cellPoint.Data = appSession.ScratchPadFactory.GetScratchPadFloat(point.ToInt()).Value > 0 ? 1m : 0m;
                                }
                            }
                            foreach (var cellPoint in appSession.CellPoints.Where(cp => cp.IsCustomValue))
                            {
                                var point = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true);
                                appSession.ScratchPadFactory.SetScratchPadValue(point.ToInt(), cellPoint.CustomValue);
                            }

                            if (appSession.CurrentCellTest != null &&
                                TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(
                                    ScratchPadConstants.IntegerIndexes.TestRunning.ToInt()).Value == 1)
                            {
                                ExportService.WriteDataToFile(appSession.CurrentCellTest.CellTestId, captureTime, appSession.CellPoints.Where(cp => cp.IsRecord).ToList());
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
                WorkCompleted();
            }
            catch (TaskCanceledException ex)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => ApplicationSessionFactory.LogEvent("Error in point data collection: " + ex.Message, true));
            }
        }

        private void ConfigurePacWidgets()
        {
            //TODO: All widget settings
            var ventCtrl1Settings = WidgetRepository.GetWidgetSettingByWidgetCell(ApplicationSessionFactory.CurrentCellId, WidgetConstants.Widget.VentilationControl1);

            if (!ventCtrl1Settings.IsNotNullOrEmpty()) return;
            var settings = new VentilationControlViewModel();
            settings.SetValues(ventCtrl1Settings);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1InsideType.ToInt(), settings.Inside.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutsideType.ToInt(), settings.Outside.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutputType.ToInt(), settings.Output.ToInt());
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Setpoint.ToInt(), settings.SetPoint);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Gain.ToInt(), settings.Gain);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1Status.ToInt(), settings.IsActive ? 1 : 0);
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
