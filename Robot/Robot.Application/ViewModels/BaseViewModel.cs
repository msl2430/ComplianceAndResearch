using System.ComponentModel;
using Robot.Application.Factories;

namespace Robot.Application.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
