using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Helpers;
using EngineCell.Models.Models;

namespace EngineCell.Models.Repositories
{
    public interface IWidgetRepository
    {
        CellTestPhaseModel AddPhaseToTest(int cellTestId, int phaseIndex, string phaseName);
        void UpdatePhase(int cellTestPhaseId, string name);
        void DeletePhaseFromTest(int cellTestId, int cellTestPhaseId);
        CellTestPhaseWidgetModel AddWidgetToPhase(int cellTestPhaseId, WidgetConstants.Widget widget);
        void DeleteWidgetFromPhase(int cellTestPhaseWidgetId);

        IList<WidgetSettingValueModel> GetWidgetSettingByWidgetCell(int cellId, WidgetConstants.Widget widgetId);
        void SaveWidgetSettings(IList<WidgetSettingValueModel> settings);
    }

    public class WidgetRepository : IWidgetRepository
    {
        public CellTestPhaseModel AddPhaseToTest(int cellTestId, int phaseIndex, string phaseName)
        {
            var newPhase = new CellTestPhase
            {
                CellTestId = cellTestId,
                PhaseOrder = phaseIndex,
                Name = phaseName
            };

            NHibernateHelper.CurrentSession.Save(newPhase);
            NHibernateHelper.CurrentSession.Flush();

            return new CellTestPhaseModel(newPhase);
        }

        public void UpdatePhase(int cellTestPhaseId, string name)
        {
            var phase = NHibernateHelper.CurrentSession.QueryOver<CellTestPhase>()
                .Where(t => t.CellTestPhaseId == cellTestPhaseId)
                .SingleOrDefault<CellTestPhase>();

            if (phase == null)
                return;

            phase.Name = name;

            NHibernateHelper.CurrentSession.Update(phase);
            NHibernateHelper.CurrentSession.Flush();
        }

        public void DeletePhaseFromTest(int cellTestId, int cellTestPhaseId)
        {
            var phase = NHibernateHelper.CurrentSession.QueryOver<CellTestPhase>()
                .Where(t => t.CellTestPhaseId == cellTestPhaseId)
                .SingleOrDefault<CellTestPhase>();

            if (phase == null)
                return;

            var index = phase.PhaseOrder;

            NHibernateHelper.CurrentSession.CreateSQLQuery("UPDATE CellTestPhase SET PhaseOrder = PhaseOrder - 1 WHERE CellTestId = :cellTestId AND PhaseOrder > :phaseOrder")
                .SetParameter("cellTestId", cellTestId)
                .SetParameter("phaseOrder", phase.PhaseOrder)
                .ExecuteUpdate();

            NHibernateHelper.CurrentSession.Delete(phase);
            NHibernateHelper.CurrentSession.Flush();
        }

        public CellTestPhaseWidgetModel AddWidgetToPhase(int cellTestPhaseId, WidgetConstants.Widget widget)
        {
            var newWidget = new CellTestPhaseWidget
            {
                CellTestPhaseId = cellTestPhaseId,
                WidgetId = widget
            };

            NHibernateHelper.CurrentSession.Save(newWidget);
            NHibernateHelper.CurrentSession.Flush();

            return new CellTestPhaseWidgetModel(newWidget);
        }

        public void DeleteWidgetFromPhase(int cellTestPhaseWidgetId)
        {
            NHibernateHelper.CurrentSession.CreateSQLQuery("DELETE FROM CellTestPhaseWidget_Setting WHERE CellTestPhaseWidgetId = :cellTestPhaseWidgetId")
                .SetParameter("cellTestPhaseWidgetId", cellTestPhaseWidgetId)
                .ExecuteUpdate();

            NHibernateHelper.CurrentSession.CreateSQLQuery("DELETE FROM CellTestPhaseWidget WHERE CellTestPhaseWidgetId = :cellTestPhaseWidgetId")
                .SetParameter("cellTestPhaseWidgetId", cellTestPhaseWidgetId)
                .ExecuteUpdate();
        }

        [Obsolete("Remove", false)]
        public IList<WidgetSettingValueModel> GetWidgetSettingByWidgetCell(int cellId, WidgetConstants.Widget widgetId)
        {
            var result = NHibernateHelper.CurrentSession.QueryOver<WidgetSettingValue>()
                .Where(w => w.WidgetId == widgetId && w.CellId == cellId)
                .List<WidgetSettingValue>();

            return result.IsNotNullOrEmpty() ? result.Select(r => new WidgetSettingValueModel(r)).ToList() : new List<WidgetSettingValueModel>();
        }

        [Obsolete("Remove", false)]
        public void SaveWidgetSettings(IList<WidgetSettingValueModel> settings)
        {
            NHibernateHelper.CurrentSession.CreateSQLQuery("DELETE FROM WidgetSettingValue WHERE CellId = :cellId AND WidgetId = :widgetId")
                .SetParameter("cellId", settings.First().CellId)
                .SetParameter("widgetId", settings.First().WidgetId.ToInt())
                .ExecuteUpdate();

            foreach (var setting in settings)
            {
                var newSetting = new WidgetSettingValue
                {
                    WidgetId = setting.WidgetId,
                    CellId = setting.CellId,
                    WidgetSettingId = setting.WidgetSettingId,
                    Value = setting.Value
                };

                NHibernateHelper.CurrentSession.Save(newSetting);
            }
        }
    }
}
