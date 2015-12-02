using System.ComponentModel;
using Newtonsoft.Json;

namespace EngineCell.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static void ApplyDefaultValues(this object self)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(self))
            {
                var defaultAttribute = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

                if (defaultAttribute != null)
                {
                    prop.SetValue(self, defaultAttribute.Value);
                }
            }
        }
    }
}
