using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Models;
using Robot.Models.Helpers;

namespace EngineCell.Models.Repositories
{
    public interface ICellPointRepository
    {
        IList<CellPointModel> GetCellPointsByCellId(int cellId);
        void UpdateCellPoint(CellPointModel point);
        void UpdateCellPointAlarm(CellPointAlarmModel alarm);
        void DeleteCellPointAlarm(int cellPointAlarmId);
    }

    public class CellPointRepository : ICellPointRepository
    {
        public IList<CellPointModel> GetCellPointsByCellId(int cellId)
        {
            Cell cell = null;
            var result = NHibernateHelper.CurrentSession.QueryOver<CellPointExtended>()
                .JoinAlias(cp => cp.Cell, () => cell)
                .Where(() => cell.CellId == cellId)
                .List<CellPointExtended>();

            return result != null && result.Any()
                ? result.Select(r => new CellPointModel(r)).ToList()
                : new List<CellPointModel>();
        }

        public void UpdateCellPoint(CellPointModel point)
        {
            var cellPoint = NHibernateHelper.CurrentSession.QueryOver<CellPoint>()
                .Where(cp => cp.CellPointId == point.CellPointId)
                .SingleOrDefault<CellPoint>();

            if (cellPoint == null)
                return;

            cellPoint.CustomName = point.CustomName;
            cellPoint.IncludeInStripChart = point.IncludeInStripChart;
            cellPoint.StripChartScale = point.StripChartScale;

            NHibernateHelper.CurrentSession.Update(cellPoint);
            NHibernateHelper.CurrentSession.Flush();
        }

        public void UpdateCellPointAlarm(CellPointAlarmModel alarm)
        {
            var cellPointAlarm = NHibernateHelper.CurrentSession.QueryOver<CellPointAlarm>()
                .Where(cpa => cpa.CellPointAlarmId == alarm.CellPointAlarmId)
                .SingleOrDefault<CellPointAlarm>() ?? new CellPointAlarm();

            cellPointAlarm.LowShutdownWarning = alarm.LowShutdownWarning;
            cellPointAlarm.LowShutdownExecute = alarm.LowShutdownExecute;
            cellPointAlarm.HighShutdownWarning = alarm.HighShutdownWarning;
            cellPointAlarm.HighShutdownExecute = alarm.HighShutdownExecute;
            cellPointAlarm.UpdateDateTime = DateTime.Now;

            NHibernateHelper.CurrentSession.SaveOrUpdate(cellPointAlarm);
            NHibernateHelper.CurrentSession.Flush();
        }

        public void DeleteCellPointAlarm(int cellPointAlarmId)
        {
            NHibernateHelper.CurrentSession.CreateSQLQuery("DELETE FROM CellPointAlarm WHERE CellPointAlarmId = :id")
                .SetParameter("id", cellPointAlarmId)
                .ExecuteUpdate();
        }
    }
}
