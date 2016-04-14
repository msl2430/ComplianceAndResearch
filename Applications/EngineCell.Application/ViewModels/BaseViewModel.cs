using EngineCell.Application.Factories;
using EngineCell.Core.Models;

namespace EngineCell.Application.ViewModels
{
    public abstract class BaseViewModel : BaseModel
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        private int _zIndex { get; set; }
        public int ZIndex
        {
            get { return _zIndex;}
            set { _zIndex = value; OnPropertyChanged("ZIndex"); OnPropertyChanged("IsEnabled"); }
        }

        public bool IsEnabled => ZIndex == 1;

    }
}
