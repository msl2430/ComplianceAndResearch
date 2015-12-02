using System.ComponentModel;
using Robot.Application.Factories;
using Robot.Core.Models;

namespace Robot.Application.ViewModels
{
    public class BaseViewModel : BaseModel, INotifyPropertyChanged
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
