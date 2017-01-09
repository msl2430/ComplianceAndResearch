﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.Factories;

namespace EngineCell.Application.Services.WorkerServices
{
    public sealed class PhaseWorkerService : BaseWorkerThreadService
    {
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        private int phaseIndex = 0;

        public PhaseWorkerService(IApplicationSessionFactory appSession)
        {
            ApplicationSessionFactory = appSession;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                SetNextPhase();
                ApplicationSessionFactory.CurrentCellTest.IsRunning = true;                
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10 || !ApplicationSessionFactory.CurrentCellTest.IsRunning) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    if (ApplicationSessionFactory.CurrentPhaseRunning.Widgets.All(w => w.IsComplete))
                    {
                        ApplicationSessionFactory.LogEvent("Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]] complete.", true);
                        if (ApplicationSessionFactory.CurrentCellTest.Phases.Count >= phaseIndex + 1)
                        {
                            SetNextPhase();
                        }
                        else
                        {
                            //test complete
                            ApplicationSessionFactory.LogEvent("Test Complete.", true);
                            ApplicationSessionFactory.CurrentCellTest.IsRunning = false;
                            CancellationToken.Cancel();
                            continue;
                        }
                    }

                    if (!ApplicationSessionFactory.CurrentPhaseRunning.Widgets.Any(w => w.IsError)) continue;
                    
                    ApplicationSessionFactory.LogEvent("Errors in Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]]:", true);
                    foreach (var widget in ApplicationSessionFactory.CurrentPhaseRunning.Widgets.Where(w => w.IsError))
                    {
                        ApplicationSessionFactory.LogEvent(" - " + widget.ErrorReason, true);
                    }
                    ApplicationSessionFactory.CurrentCellTest.IsRunning = false;
                    CancellationToken.Cancel();
                    
                }
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ApplicationSessionFactory.LogEvent("Erorr in Phase worker: " + ex.Message, true);
            }
        }

        private void SetNextPhase()
        {
            var tempPhase = ApplicationSessionFactory.CurrentCellTest.Phases[phaseIndex];
            tempPhase.IsRunning = true;
            tempPhase.StartDateTime = new DateTime();
            ApplicationSessionFactory.CurrentPhaseRunning = tempPhase;
            phaseIndex++;
            tempPhase = null;
            ApplicationSessionFactory.LogEvent("Starting Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]].", true);
        }

        protected override void WorkCompleted()
        {
            
        }
    }
}
