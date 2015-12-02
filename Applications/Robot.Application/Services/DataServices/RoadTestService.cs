using Robot.Models.Models;
using System.Collections.Generic;

namespace Robot.Application.Services.DataServices
{
    public interface IRoadTestService
    {
        IList<RoadTestChartModel> GetAllRoadTests();
        IList<RoadTestChartModel> GetLearningRoadTests();
    }

    public sealed class RoadTestService : BaseDataService, IRoadTestService
    {
        public IList<RoadTestChartModel> GetAllRoadTests()
        {
            return RoadTestRepository.GetAllActiveRoadTests();
        }

        public IList<RoadTestChartModel> GetLearningRoadTests()
        {
            return RoadTestRepository.GetLearningRoadTests();
        }
    }
}
