using System.IO;
using System.Linq;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;

namespace EngineCell.Application.Services.DataServices
{
    public sealed class ExportService : BaseDataService
    {
        public string WriteCsvExport(int cellTestId)
        {
            var fileName = @"D:\Test.txt";
            var captureTimes = CellPointRepository.GetCaptureTimesForTest(cellTestId);

            if (captureTimes.IsNullOrEmpty())
                return null;

            var points = CellPointRepository.GetCellPointInTest(cellTestId);

            if (points.IsNullOrEmpty())
                return null;

            var rowData = new string[points.Count];

            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine($"CaptureTime,{string.Join(",", points.OrderBy(p => p.Id).Select(p => p.Name))}");
                foreach (var row in captureTimes.OrderBy(t => t).Select(captureTime => CellPointRepository.GetDataFromCellTest(cellTestId, captureTime)).SelectMany(data => data))
                {
                    for (var i = 0; i < points.Count; i++)
                    {
                        rowData[i] = "0.0";
                    }

                    rowData[points.IndexOf(new IdNamePair(row.CellPointId, row.CellPointName))] = row.Data.ToString("0.0##");
                    file.WriteLine($"{row.CaptureTime.ToString("hh:mm:ss.fff")},{string.Join(",", rowData.ToList())}");
                }
            }

            return fileName;
        }
    }
}
