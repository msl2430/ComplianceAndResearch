﻿using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Robot.Models.DataObjects;
using Robot.Models.Helpers;
using Robot.Models.Models;

namespace Robot.Models.Repositories
{
    public interface IModelRepository
    {
        IList<CarModel> GetModelByManufacturerId(int manufacturerId);
        bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId);
        int AddModel(string name, int manufacturerId, int year, decimal pulseMultiplier, decimal pulsePerRev);
    }

    public sealed class ModelRepository : IModelRepository
    {
        public IList<CarModel> GetModelByManufacturerId(int manufacturerId)
        {
            Manufacturer manufacturer = null;
            var models = NHibernateHelper.CurrentSession.QueryOver<ModelExtended>()
                .JoinAlias(m => m.Manufacturer, () => manufacturer)
                .Where(() => manufacturer.ManufacturerId == manufacturerId)
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

        public int AddModel(string name, int manufacturerId, int year, decimal pulseMultiplier, decimal pulsePerRev)
        {
            var model = new Model
            {
                Name = name,
                ManufacturerId = manufacturerId,
                Year = year,
                PulseMultiplier = pulseMultiplier,
                PulsePerRev = pulsePerRev
            };

            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();
            
            NHibernateHelper.ClearQueryCache();

            return model.ModelId;
        }
    }
}
