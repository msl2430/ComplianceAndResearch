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
        IList<WidgetSettingValueModel> GetWidgetSettingByWidgetCell(int cellId, WidgetConstants.Widget widgetId);
        void SaveWidgetSettings(IList<WidgetSettingValueModel> settings);
    }

    public class WidgetRepository : IWidgetRepository
    {
        public IList<WidgetSettingValueModel> GetWidgetSettingByWidgetCell(int cellId, WidgetConstants.Widget widgetId)
        {
            var result = NHibernateHelper.CurrentSession.QueryOver<WidgetSettingValue>()
                .Where(w => w.WidgetId == widgetId && w.CellId == cellId)
                .List<WidgetSettingValue>();

            return result.IsNotNullOrEmpty() ? result.Select(r => new WidgetSettingValueModel(r)).ToList() : new List<WidgetSettingValueModel>();
        }

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
