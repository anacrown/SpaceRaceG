using System.Windows.Input;

namespace SpaceRaceG.AI
{
    public class Size
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public Size(int size) : this(size, size) { }
        
        public int Width { get; }
        public int Height { get; }
    }
}