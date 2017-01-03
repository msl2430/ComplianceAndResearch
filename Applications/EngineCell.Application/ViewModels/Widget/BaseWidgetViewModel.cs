using System.Collections.Generic;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public abstract class BaseWidgetViewModel : BaseViewModel
    {
        private bool _isActive { get; set; }
        public bool IsActive { get { return _isActive; } set { _isActive = value; OnPropertyChanged("IsActive"); } }

        public abstract void SetValues(IList<WidgetSettingValueModel> settings);

        public abstract IList<WidgetSettingValueModel> GetValues(int cellId);
    }
}
