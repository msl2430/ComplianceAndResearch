using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Helpers;
using EngineCell.Models.Models;

namespace EngineCell.Models.Repositories
{
    public interface ICellPointRepository
    {
        IList<CellPointModel> GetCellPointsByCellId(int cellId);
        CellTestModel GetCellTestById(int cellTestId);

        void UpdateCellPoint(CellPointModel point);
        void UpdateCellPointAlarm(CellPointAlarmModel alarm);
        void DeleteCellPointAlarm(int cellPointAlarmId);

        int CreateCellTest(int cellId);
        void UpdateCellTestEndTime(CellTestModel cellTest);
        void CreateCellPointData(IList<CellTestPointDataModel> points);
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

        public CellTestModel GetCellTestById(int cellTestId)
        {
            var test = NHibernateHelper.CurrentSession.QueryOver<CellTest>()
                .Where(ct => ct.CellTestId == cellTestId)
                .SingleOrDefault<CellTest>();

            return test != null ? new CellTestModel(test) : null;
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

        public int CreateCellTest(int cellId)
        {
            var newTest = new CellTest() {CellId = cellId, StartTime = DateTime.Now};
            NHibernateHelper.CurrentSession.Save(newTest);
            NHibernateHelper.CurrentSession.Flush();
            return newTest.CellTestId;
        }

        public void UpdateCellTestEndTime(CellTestModel cellTest)
        {
            var existing = NHibernateHelper.CurrentSession.QueryOver<CellTest>()
                .Where(ct => ct.CellTestId == cellTest.CellTestId)
                .SingleOrDefault<CellTest>();

            if (existing == null)
                return;

            existing.EndTime = DateTime.Now;

            NHibernateHelper.CurrentSession.Update(existing);
            NHibernateHelper.CurrentSession.Flush();
        }

        public void CreateCellPointData(IList<CellTestPointDataModel> points)
        {
            //using (var session = NHibernateHelper.CurrentSession)
            using (var transaction = NHibernateHelper.CurrentSession.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                foreach (var point in points)
                    NHibernateHelper.CurrentSession.Save(new CellTestPointData
                    {
                        CellTestId = point.CellTestId,
                        CellPointId = point.CellPointId,
                        Data = point.Data,
                        CaptureTime = point.CaptureTime
                    });
                transaction.Commit();
            }
        }
    }
}
