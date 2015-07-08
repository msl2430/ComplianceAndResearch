using System.Collections.Generic;
using System.Linq;
using Admissions.Models.Helpers;
using NHibernate.Criterion;
using Robot.Models.DataObjects;
using Robot.Models.Models;

namespace Robot.Models.Repositories
{
    public interface IModelRepostiory
    {
        IList<CarModel> GetModelByManufacturerId(int manufacturerId);
        bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId);
        int AddModel(string name, int manufacturerId, int year);
    }

    public sealed class ModelRepostiory : IModelRepostiory
    {
        public IList<CarModel> GetModelByManufacturerId(int manufacturerId)
        {
            var models = NHibernateHelper.CurrentSession.QueryOver<Model>()
                .Where(m => m.ManufacturerId == manufacturerId)
                .List();

            return models != null && models.Any() ? models.Select(m => new CarModel(m)).ToList() : new List<CarModel>();
        }

        public bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId)
        {
            return NHibernateHelper.CurrentSession.QueryOver<Model>()
                .Where(m => m.ManufacturerId == manufacturerId)
                .WhereRestrictionOn(m => m.Name).IsInsensitiveLike(name, MatchMode.Anywhere)
                .Select(Projections.Constant(1))
                .Take(1)
                .SingleOrDefault<int>() != 1;
        }

        public int AddModel(string name, int manufacturerId, int year)
        {
            var model = new Model
            {
                Name = name,
                ManufacturerId = manufacturerId,
                Year = year
            };

            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();
            
            NHibernateHelper.ClearQueryCache();

            return model.ModelId;
        }
    }
}
