namespace EngineCell.Application.ViewModels.Widget
{
    public class StarterDisplayViewModel: BaseViewModel
    {
        private long _crankTime { get; set; }
        public long CrankTime
        {
            get { return _crankTime; }
            set { _crankTime = value; OnPropertyChanged("CrankTime"); }
        }

        private decimal _startParameter { get; set; }
        public decimal StartParameter
        {
            get { return _startParameter; }
            set { _startParameter = value; OnPropertyChanged("StartParameter"); }
        }

        private long _addedCrankTime { get; set; }
        public long AddedCrankTime
        {
            get { return _addedCrankTime; }
            set { _addedCrankTime = value; OnPropertyChanged("AddedCrankTime"); }
        }

        public string Name { get; set; }

        public StarterDisplayViewModel(string name, bool isActive)
        {
            Name = name;
            CrankTime = 10;
            StartParameter = 1;
            AddedCrankTime = 10;
        }
    }
}
