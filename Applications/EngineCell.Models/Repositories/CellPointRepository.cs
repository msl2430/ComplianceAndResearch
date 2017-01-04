using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Helpers;
using EngineCell.Models.Models;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace EngineCell.Models.Repositories
{
    public interface ICellPointRepository
    {
        IList<CellPointModel> GetCellPointsByCellId(int cellId);
        CellTestModel GetCellTestById(int cellTestId);

        void UpdateCellPoint(CellPointModel point);
        void UpdateCellPointAlarm(CellPointAlarmModel alarm);
        void DeleteCellPointAlarm(int cellPointAlarmId);

        int CreateCellTest(int cellId, ControlConstants.CellTestType type);
        void UpdateCellTestEndTime(CellTestModel cellTest);
        void CreateCellPointData(IList<CellTestPointDataModel> points);

        IList<DateTime> GetCaptureTimesForTest(int cellTestId);
        IList<IdNamePair> GetCellPointInTest(int cellTestId);
        IList<CellTestPointDataModel> GetDataFromCellTest(int cellTestId, DateTime captureTime);
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
            var test = NHibernateHelper.CurrentSession.QueryOver<CellTestExtended>()
                .Where(ct => ct.CellTestId == cellTestId)
                .SingleOrDefault<CellTestExtended>();

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
            cellPoint.IsRecord = point.IsRecord;
            cellPoint.IsAverage = point.IsAverage;
            cellPoint.AverageSeconds = point.AverageSeconds;
            cellPoint.IncludeInStripChart = point.IncludeInStripChart;
            cellPoint.StripChartScale = point.StripChartScale;
            //cellPoint.IsCustomValue = point.IsCustomValue;
            //cellPoint.CustomValue = point.CustomValue;

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

        public int CreateCellTest(int cellId, ControlConstants.CellTestType type)
        {
            var newTest = new CellTest() {CellId = cellId, StartTime = DateTime.Now, CellTestTypeId = type};
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
            var str = new StringBuilder();

            foreach (var point in points)
            {
                str.AppendFormat("({0}, {1}, {2}, '{3}'),", point.CellTestId, point.CellPointId, point.Data, point.CaptureTime);
            }

            var sqlString = str.ToString().Substring(0, str.Length - 1);
            NHibernateHelper.CurrentSession.CreateSQLQuery("INSERT INTO CellTestPointData (CellTestId, CellPointId, Data, CaptureTime) VALUES " + sqlString).ExecuteUpdate();
        }

        public IList<DateTime> GetCaptureTimesForTest(int cellTestId)
        {
            return NHibernateHelper.CurrentSession.QueryOver<CellTestPointData>()
                .Where(t => t.CellTestId == cellTestId)
                .SelectList(list => list.SelectGroup(t => t.CaptureTime))
                .OrderBy(t => t.CaptureTime).Asc
                .List<DateTime>();
        }

        public IList<IdNamePair> GetCellPointInTest(int cellTestId)
        {
            IdNamePair idNamePair = null;
            CellPoint point = null;
            CellTest test = null;
            return NHibernateHelper.CurrentSession.QueryOver<CellTestPointDataExtended>()
                .JoinAlias(t => t.CellPoint, () => point)
                .JoinAlias(t => t.CellTest, () => test)
                .Where(() => test.CellTestId == cellTestId)
                .SelectList(list => list.SelectGroup(() => point.CellPointId).WithAlias(() => idNamePair.Id)
                .SelectGroup(()=> point.CustomName).WithAlias(() => idNamePair.Name))
                .OrderBy(() => point.CellPointId).Asc
                .TransformUsing(Transformers.AliasToBean<IdNamePair>())
                .List<IdNamePair>();
        } 

        public IList<CellTestPointDataModel> GetDataFromCellTest(int cellTestId, DateTime captureTime)
        {
            CellTest test = null;
            var result = NHibernateHelper.CurrentSession.QueryOver<CellTestPointDataExtended>()
                .JoinAlias(t => t.CellTest, () => test)
                .Where(t => test.CellTestId == cellTestId && t.CaptureTime == captureTime)
                .List<CellTestPointDataExtended>();

            return result.IsNotNullOrEmpty() ? result.Select(r => new CellTestPointDataModel(r)).ToList() : new List<CellTestPointDataModel>();
        }
    }
}
