using Robot.Models.Models;
using System.Collections.Generic;

namespace Robot.Application.Services.DataServices
{
    public interface ISetPointService
    {
        ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId);
    }

    public class SetPointService : BaseDataService, ISetPointService
    {
        public ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId)
        {
            return SetPointRepository.GetSetPointChartByModelId(modelId);
        }
    }
}
