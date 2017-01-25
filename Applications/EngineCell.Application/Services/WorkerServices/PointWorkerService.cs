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

        private IList<IScratchPadModel<decimal>> ScratchPadValues { get; set; }
        private IList<Task> TaskList { get; set; }

        public PointWorkerService(TestDisplayViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            TestDisplayViewModel = viewModel;
            ApplicationSessionFactory = viewModel.ApplicationSessionFactory;
            WidgetRepository = new WidgetRepository();            
        }

        public override void DoWork()
        {

            CancellationToken = new CancellationTokenSource();
            ApplicationSessionFactory.LogEvent("Point data collection started.", true);            
            TaskList = new List<Task>();
            
            try
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();                    

                    //Check if we're connected to and collecting data from Opto before proceeding
                    if (ApplicationSessionFactory.OptoMmpFactory != null && ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                    {
                        ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 1);
                        CaptureTime = DateTime.Now;

                        ScratchPadValues = ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatRange(1000, 1073);
                        TaskList.Clear();
                        foreach (var cellPoint in ApplicationSessionFactory.CellPoints.Where(p => p.IsActive || p.HasPhaseTrigger))
                        {
                            CaptureCellPoint(cellPoint, ScratchPadValues.FirstOrDefault(sc => sc.Index == ((ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName, true)).ToInt()), ApplicationSessionFactory);
                        }

                        Task.WaitAll(TaskList.ToArray());

                        for(var i = 0; i < TaskList.Count; i++)
                        {
                            TaskList[i] = null;
                        }

                        if (ApplicationSessionFactory.CurrentCellTest != null 
                            && ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt()) == 1
                            && ApplicationSessionFactory.CellPoints.Any(cp => cp.IsRecord && cp.IsActive))
                        {
                            ExportService.WriteDataToFile(ApplicationSessionFactory.CurrentCellTest.CellTestId, CaptureTime, ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord && cp.IsActive).ToList());
                            CellPointRepository.CreateCellPointData(
                                ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord && cp.IsActive)
                                    .Select(cp => cp.ToCellTestPointDataModel(ApplicationSessionFactory.CurrentCellTest.CellTestId, DateTime.Now))
                                    .ToList());
                        }

                        Dispatcher.Invoke(() => {
                            TestDisplayViewModel.UpdateVisibleCellPoints();
                        });
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
                try
                {
                    Dispatcher.Invoke(() => ApplicationSessionFactory.LogEvent("Error in point data collection: " + ex.Message));
                }
                catch (Exception)
                {
                }
            }
            
            WorkCompleted();
        }

        private static void CaptureCellPoint(PointDataModel cellPoint, IScratchPadModel<decimal> scratchPadValue, IApplicationSessionFactory appSession)
        {
            if (scratchPadValue == null || !cellPoint.IsActive)
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

                if (cellPoint.IsInput || !cellPoint.IsActive) return;

                appSession.ScratchPadFactory.SetScratchPadValue(((ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true)).ToInt(), ScratchPadConstants.DefaultNullValue);
            }
            else
            {
                Task.Run(() => appSession.ScratchPadFactory.SetScratchPadValue(((ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName + "Value", true)).ToInt(), cellPoint.CustomValue.Value));
                cellPoint.Data = cellPoint.CustomValue.Value;
                cellPoint.MostRecentData.Add(cellPoint.Data);
                if (cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds * 2) //2 because we're capturing 2 data points per second
                    cellPoint.MostRecentData.RemoveAt(0);
                cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m;
            }
        }        

        protected override void WorkCompleted()
        {
            ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 0);
            ApplicationSessionFactory.LogEvent("Point data collection stopped.", true);
        }
    }
}
