using System.Collections.Generic;
using Robot.Models.Models;

namespace Robot.Application.Services.DataServices
{
    public interface ICountryService
    {
        IList<CountryModel> GetCountries();
    }

    public sealed class CountryService : BaseDataService, ICountryService
    {
        public IList<CountryModel> GetCountries()
        {
            return CountryRepository.GetCountries();
        }
    }
}
