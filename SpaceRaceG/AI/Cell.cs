namespace SpaceRaceG.AI
{
    public class Cell
    {
        public Point P { get; private set; }
        public Element Element { get; private set; }

        public Cell(Point p, Element element)
        {
            P = p;
            Element = element;
        }
        public void Reset(Element element)
        {
            Element = element;
        }
    }
}