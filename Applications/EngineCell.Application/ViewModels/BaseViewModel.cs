using System.ComponentModel;
using EngineCell.Application.Factories;
using Opto22.Core.Models;

namespace EngineCell.Application.ViewModels
{
    public abstract class BaseViewModel : BaseModel, INotifyPropertyChanged
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
