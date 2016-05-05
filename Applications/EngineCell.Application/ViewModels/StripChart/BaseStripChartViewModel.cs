using EngineCell.Application.Factories;
using OxyPlot;

namespace EngineCell.Application.ViewModels.StripChart
{
    public abstract class BaseStripChartViewModel : BaseViewModel
    {
        public PlotModel PlotModel { get; set; }

        protected bool _isPlay { get; set; }
        public bool IsPlay {
            get { return _isPlay; }
            set { _isPlay = value; OnPropertyChanged("IsPlay"); }
        }

        protected BaseStripChartViewModel()
        {
            PlotModel = new PlotModel();
            IsPlay = false;
        }        

        protected abstract void InitializePlotModel();

        public abstract void CreateSeries();

        public abstract void UpdateSeries();
    }
}
