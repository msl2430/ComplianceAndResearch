﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EngineCell.Core.Models
{
    [Serializable]
    public class BaseModel : ICloneable, INotifyPropertyChanged
    {
        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var prop in GetType().GetProperties())
            {
                if (prop.Name.ToLower().Contains("password"))
                    str.AppendFormat("{0}: {1}", prop.Name, "".PadLeft(prop.GetValue(this, null).ToString().Length, '*'));
                else
                    str.AppendFormat("{0}: {1} ", prop.Name, prop.GetValue(this, null));
            }
            return str.ToString();
        }

        protected void InstantiateFromDataObject(object dataObject)
        {
            foreach (var propInfo in GetType().GetProperties())
            {
                var setProp = dataObject.GetType().GetProperty(propInfo.Name);
                if (setProp != null && setProp.PropertyType == propInfo.PropertyType)
                    propInfo.SetValue(this, setProp.GetValue(dataObject));
            }
        }

        public T Clone<T>()
        {
            return (T)Clone();
        }

        public object Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
