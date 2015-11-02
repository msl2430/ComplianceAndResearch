using Robot.Models.Models;
using System.Collections.Generic;

namespace Robot.Application.Services.DataServices
{
    public interface ISetPointService
    {
        ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId);
        ThrottleSetPointChartModel GetSetPointChartById(int chartId);
    }

    public class SetPointService : BaseDataService, ISetPointService
    {
        public ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId)
        {
            return SetPointRepository.GetSetPointChartByModelId(modelId);
        }

        public ThrottleSetPointChartModel GetSetPointChartById(int chartId)
        {
            return SetPointRepository.GetSetPointChartById(chartId);
        }
    }
}
