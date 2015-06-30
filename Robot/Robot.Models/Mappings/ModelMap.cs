using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class ModelMap :ClassMap<Model>
    {
        public ModelMap()
        {
            Table("Model");
            Cache.NonStrictReadWrite();

            Id(m => m.ModelId);

            Map(m => m.ManufacturerId);
            Map(m => m.Name);
            Map(m => m.Year);
        }
    }
}
