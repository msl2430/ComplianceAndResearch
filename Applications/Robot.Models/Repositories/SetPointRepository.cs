using Robot.Core.Constants;
using Robot.Models.DataObjects;
using Robot.Models.Helpers;
using Robot.Models.Models;

namespace Robot.Models.Repositories
{
    public interface ISetPointRepository
    {
        ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId);
        ThrottleSetPointChartModel GetSetPointChartById(int setPointChartId);
    }

    public sealed class SetPointRepository : ISetPointRepository
    {
        public ThrottleSetPointChartModel GetSetPointChartByModelId(int modelId)
        {
            var result = NHibernateHelper.CurrentSession.QueryOver<ThrottleSetPointChartExtended>()
                .Where(tsp => tsp.ModelId == modelId && tsp.Status == StatusConstants.SqlStatus.Active)
                .OrderBy(tsp => tsp.CreationDateTime).Desc
                .Take(1)
                .SingleOrDefault<ThrottleSetPointChartExtended>();

            return new ThrottleSetPointChartModel(result);
        }

        public ThrottleSetPointChartModel GetSetPointChartById(int setPointChartId)
        {
            var result = NHibernateHelper.CurrentSession.QueryOver<ThrottleSetPointChartExtended>()
                .Where(tsp => tsp.ThrottleSetPointChartId == setPointChartId && tsp.Status == StatusConstants.SqlStatus.Active)
                .OrderBy(tsp => tsp.CreationDateTime).Desc
                .Take(1)
                .SingleOrDefault<ThrottleSetPointChartExtended>();

            return new ThrottleSetPointChartModel(result);
        }
    }
}
