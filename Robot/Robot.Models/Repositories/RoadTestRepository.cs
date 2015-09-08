using Robot.Core.Constants;
using Robot.Models.DataObjects;
using Robot.Models.Helpers;
using Robot.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Models.Repositories
{
    public interface IRoadTestRepository
    {
        IList<RoadTestChartModel> GetAllActiveRoadTests();
    }

    public sealed class RoadTestRepository : IRoadTestRepository
    {
        public IList<RoadTestChartModel> GetAllActiveRoadTests()
        {
            var results = NHibernateHelper.CurrentSession.QueryOver<RoadTestChartExtended>()
                .Where(rtc => rtc.Status == StatusConstants.SqlStatus.Active)
                .List<RoadTestChartExtended>();

            return results != null && results.Any() ? results.Select(r => new RoadTestChartModel(r)).ToList() : new List<RoadTestChartModel>();
        }
    }
}
