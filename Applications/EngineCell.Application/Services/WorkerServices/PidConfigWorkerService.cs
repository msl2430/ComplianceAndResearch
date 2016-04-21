﻿using System;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IPidConfigWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public class PidConfigWorkerService : BaseWorkerThreadService, IPidConfigWorkerService
    {
        private PidConfigViewModel ViewModel { get; set; }

        public PidConfigWorkerService(PidConfigViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if(!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();

                        ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigType.ToInt(), ViewModel.PidType.ToInt());
                        ViewModel.Input = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.InputValue.ToInt()).Value;
                        ViewModel.SetPoint = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.SetPointValue.ToInt()).Value;
                        ViewModel.Output = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputValue.ToInt()).Value;
                    }
                    Thread.Sleep(250);
                }
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void WorkCompleted()
        {
        }
    }
}
