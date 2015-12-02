using System.Collections.Generic;
using System.Linq;
using Robot.Models.Models;

namespace Robot.Application.ViewModels
{
    public sealed class NewManufacturerFlyoutViewModel : BaseViewModel
    {
        public IList<CountryModel> Countries { get; set; }

        public int DefaultCountry { get { return Countries.Any() ? Countries.IndexOf(Countries.FirstOrDefault(c => c.CountryId == 226)) : 0; } }

        private bool _isUniqueName { get; set; }
        public bool IsUniqueName
        {
            get { return _isUniqueName; }
            set
            {
                _isUniqueName = value;
                OnPropertyChanged("IsUniqueName");
            }
        }
    }
}
