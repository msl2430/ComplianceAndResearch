namespace EngineCell.Core.Models {
    public class Pair<T, Y> {

        public Pair() {
        }

        public Pair(T first, Y second) {
            First = first;
            Second = second;
        }

        public T First { get; set; }
        public Y Second { get; set; }
    }
}
