using System;

namespace Robot.Core.Models
{
    [Serializable]
    public class IdNamePair 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IdNamePair()
        {
        }

        public IdNamePair(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var right = obj as IdNamePair;

            return right != null
                && right.Id == Id
                && right.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Id, Name);
        }
    }
}
