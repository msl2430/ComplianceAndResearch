using System.Collections.Generic;
using System.Linq;
using Admissions.Models.Helpers;
using Robot.Models.DataObjects;
using Robot.Models.Models;

namespace Robot.Models.Repositories
{
    public interface ICountryRepository
    {
        IList<CountryModel> GetCountries();
    }

    public sealed class CountryRepository : ICountryRepository
    {
        public IList<CountryModel> GetCountries()
        {
            var countries = NHibernateHelper.CurrentSession.QueryOver<Country>().List<Country>();

            return countries != null && countries.Any() ? countries.Select(c => new CountryModel(c)).ToList() : new List<CountryModel>();
        }
    }
}
