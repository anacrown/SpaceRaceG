namespace SpaceRaceG.AI
{
    public class Cell
    {
        public Point P { get; private set; }
        public Element Element { get; private set; }

        public Cell() { }
        public Cell(Element element, Point p) => Reset(element, p);
        public void Reset(Element element, Point p = null)
        {
            Element = element;
            if (p != null) P = p;
        }
    }
}