namespace Opto22.Core.Models
{
    public class ScratchPadModel<T>
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public T Value { get; set; }

        public ScratchPadModel() { } 
        public ScratchPadModel(int index, string name, T value)
        {
            Index = index;
            Name = name;
            Value = value;
        }
    }
}
