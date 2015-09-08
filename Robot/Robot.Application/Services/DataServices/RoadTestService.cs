using Robot.Models.Models;
using System.Collections.Generic;

namespace Robot.Application.Services.DataServices
{
    public interface IRoadTestService
    {
        IList<RoadTestChartModel> GetAllRoadTests();
    }

    public sealed class RoadTestService : BaseDataService, IRoadTestService
    {
        public IList<RoadTestChartModel> GetAllRoadTests()
        {
            return RoadTestRepository.GetAllActiveRoadTests();
        }
    }
}
