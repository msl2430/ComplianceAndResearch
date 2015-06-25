using System.Collections.Generic;
using System.Linq;
using Admissions.Models.Helpers;
using NHibernate;
using NHibernate.Criterion;
using Robot.Models.DataObjects;
using Robot.Models.Models;

namespace Robot.Models.Repositories
{
    public interface IManufacturerRepository
    {
        IList<ManufacturerModel> GetManufacturerList();
        bool CheckUniqueManufacturerName(string name);
        void AddManufacturer(string name, int countryId, string modelName);
    }

    public sealed class ManufacturerRepository : IManufacturerRepository
    {
        public IList<ManufacturerModel> GetManufacturerList()
        {
            var manufacturer = NHibernateHelper.CurrentSession.QueryOver<ManufacturerExtended>()
                .Cacheable().CacheMode(CacheMode.Normal)
                .List<ManufacturerExtended>();

            return manufacturer != null && manufacturer.Any() ? manufacturer.Select(m => new ManufacturerModel(m)).ToList() : null;
        }

        public bool CheckUniqueManufacturerName(string name)
        {
            return NHibernateHelper.CurrentSession.QueryOver<Manufacturer>()
                .WhereRestrictionOn(m => m.Name).IsInsensitiveLike(name, MatchMode.Exact)
                .Select(Projections.Constant(1))
                .Take(1)
                .SingleOrDefault<int>() != 1;
        }

        public void AddManufacturer(string name, int countryId, string modelName)
        {
            var manufacturer = new Manufacturer
            {
                Name = name,
                CountryId = countryId
            };

            NHibernateHelper.CurrentSession.Save(manufacturer);
            NHibernateHelper.CurrentSession.Flush();

            if (!string.IsNullOrEmpty(modelName))
            {
                var model = new Model
                {
                    Name = modelName,
                    ManufacturerId = manufacturer.ManufacturerId
                };

                NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();
            }

            NHibernateHelper.ClearQueryCache();
        }
    }
}
