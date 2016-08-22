using System;
using System.IO;
using System.Linq;
using System.Reflection;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.DataServices
{
    public sealed class ExportService : BaseDataService
    {
        public string WriteCsvExport(int cellTestId)
        {
            var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Tests\CellTest_" + cellTestId + "_" + DateTime.Now.ToString("MM_dd_yyyy") + ".csv";
            var captureTimes = CellPointRepository.GetCaptureTimesForTest(cellTestId);

            if (captureTimes.IsNullOrEmpty())
                return null;

            var points = CellPointRepository.GetCellPointInTest(cellTestId);

            if (points.IsNullOrEmpty())
                return null;

            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"CaptureTime,{string.Join(",", points.OrderBy(p => p.Id).Select(p => p.Name))}");
                foreach (var captureTime in captureTimes.OrderBy(t => t))
                {
                    var captureTimeData = CellPointRepository.GetDataFromCellTest(cellTestId, captureTime);
                    var rowData = new string[points.Count];
                    for (var i = 0; i < points.Count; i++)
                    {
                        var data = captureTimeData.FirstOrDefault(t => t.CellPointName == points[i].Name);
                        rowData[i] = data?.Data.ToString("0.0##") ?? "0.0";
                    }

                    file.WriteLine($"{captureTime.ToString("hh:mm:ss.fff")},{string.Join(",", rowData.ToList())}");
                }
            }

            return fileName;
        }
    }
}
