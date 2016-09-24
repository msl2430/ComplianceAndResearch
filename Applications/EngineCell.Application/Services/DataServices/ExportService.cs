using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.DataServices
{
    public sealed class ExportService : BaseDataService
    {
        private string CurrentFilename { get; set; }

        public string WriteCsvExport(int cellTestId)
        {
            var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Tests\CellTest_" + cellTestId + "_" + DateTime.Now.ToString("MM_dd_yyyy");
            for (var i = 1; i < 99; i++)
            {
                if (File.Exists(fileName))
                    fileName += "_" + i;
                else
                    break;
            }
            fileName += ".csv";
            var captureTimes = CellPointRepository.GetCaptureTimesForTest(cellTestId);
            
            if (captureTimes.IsNullOrEmpty())
                return null;

            var points = CellPointRepository.GetCellPointInTest(cellTestId);

            if (points.IsNullOrEmpty())
                return null;

            var initialCaptureTime = captureTimes.OrderBy(c => c).First();

            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"CaptureTime,CaptureTestTime,{string.Join(",", points.OrderBy(p => p.Id).Select(p => p.Name))}");
                foreach (var captureTime in captureTimes.OrderBy(t => t))
                {
                    var captureTimeData = CellPointRepository.GetDataFromCellTest(cellTestId, captureTime);
                    var rowData = new string[points.Count];
                    for (var i = 0; i < points.Count; i++)
                    {
                        var data = captureTimeData.FirstOrDefault(t => t.CellPointName == points[i].Name);
                        rowData[i] = data?.Data.ToString("0.0##") ?? "0.0";
                    }
                    var captureRunTime = (captureTime - initialCaptureTime).TotalSeconds > 0 ? (captureTime - initialCaptureTime).ToString(@"hh\:mm\:ss\.fff") : "00:00:00";
                    file.WriteLine($"{captureTime.ToString("hh:mm:ss.fff")},{captureRunTime},{string.Join(",", rowData.ToList())}");
                }
            }

            return fileName;
        }

        public string GetFilename()
        {
            return CurrentFilename;
        }

        public void CreateDataFile(int cellTestId, IList<PointDataModel> dataPoints)
        {
            var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Tests\CellTest_" + cellTestId + "_" + DateTime.Now.ToString("MM_dd_yyyy") + ".csv";
            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"CaptureTime,CaptureTestTime,{string.Join(",", dataPoints.OrderBy(p => p.PointId).Select(p => p.CustomName))}");
            }
            CurrentFilename = fileName;
        }

        public static void WriteDataToFile(int cellTestId, DateTime captureTime, IList<PointDataModel> dataPoints)
        {
            var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Tests\CellTest_" + cellTestId + "_" + DateTime.Now.ToString("MM_dd_yyyy") + ".csv";
            using (var file = new StreamWriter(fileName, true))
            {
                var captureRunTime = (captureTime - captureTime).TotalSeconds > 0 ? (captureTime - captureTime).ToString(@"hh\:mm\:ss\.fff") : "00:00:00";
                file.WriteLine($"{captureTime.ToString("hh:mm:ss.fff")},{captureRunTime},{string.Join(",", dataPoints.OrderBy(p => p.PointId).Select(p => p.Data).ToList())}");
            }
        }
    }
}
