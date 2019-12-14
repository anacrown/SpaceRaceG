using System.Runtime.CompilerServices;

namespace SpaceRaceG.AI
{
    public class Cell
    {
        public Board Board { get; set; }
        public Point P { get; private set; }
        public Element Element { get; private set; }

        public Cell() { }
        public Cell(Board board, Element element, Point p) : this() => Reset(element, p);

        public void Reset(Element element, Point p = null)
        {
            Element = element;
            if (p != null) P = p;
        }

        public Cell this[Direction direction] => Board[this.P[direction]];
    }
}