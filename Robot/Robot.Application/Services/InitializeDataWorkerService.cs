using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Opto22.Core.Models;
using Opto22.Core.Service;
using Robot.Application.Session;

namespace Robot.Application.Services
{
    public sealed class InitializeDataWorkerService : BaseWorkerThreadService
    {
        private IPointService PointService { get; set; }
        private ListBox PointList { get; set; }

        public InitializeDataWorkerService(IPointService pointService, Dispatcher currentDispatcher, ListBox pointList)
        {
            PointService = pointService;
            Dispatcher = currentDispatcher;
            PointList = pointList;
        }

        public async override void DoWork()
        {
            try
            {
                IsRunning = true;
                if (!ApplicationSession.WaitForConnection())
                    return;
                var points = await PointService.GetAllPointsAsync();
                ApplicationSession.AnalogPoints = points;
                Dispatcher.Invoke(() => FillListBox(points.ToList()), DispatcherPriority.Normal);
            }
            catch (TaskCanceledException ex)
            {
                IsRunning = false;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show("Shits fucked up \n" + ex.Message), DispatcherPriority.Normal);
            }
        }

        protected override void WorkCompleted()
        {
            IsRunning = false;
        }

        private void FillListBox(IList<AnalogPointModel> points)
        {
            foreach (var item in points.Select(point => new ListBoxItem
            {
                Name = string.Concat("Point", point.PointNumber),
                Content = string.IsNullOrEmpty(point.PointName) ? string.Concat("Point", point.PointNumber) : point.PointName,
            }).OrderBy(p => p.Name))
            {
                PointList.Items.Add(item);
            }
        }
    }
}
