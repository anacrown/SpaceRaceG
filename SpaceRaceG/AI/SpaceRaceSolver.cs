using System;

//-file "C:\Users\misho\source\repos\SpaceRaceG\SpaceRaceG\App_Data\Logs\WEB [nais@mail.ru]"\
//-uri "http://localhost:8080/another-context/board/player/nais@mail.ru?code=13476795611535248716"

namespace SpaceRaceG.AI
{
    public class SpaceRaceSolver
    {
        public static Element GetElement(char c)
        {
            switch (c)
            {
                case ' ': return Element.NONE;
                case 'x': return Element.EXPLOSION;
                case '☼': return Element.WALL;
                case '☺': return Element.HERO;
                case '☻': return Element.OTHER_HERO;
                case '+': return Element.DEAD_HERO;
                case '$': return Element.GOLD;
                case '♣': return Element.BOMB;
                case '0': return Element.STONE;
                case '7': return Element.BULLET_PACK;
                case '*': return Element.BULLET;
                default: throw new Exception("Invalid Symbol in board");
            }
        }

        public bool Answer(out string response)
        {
            response = "UP";

            return true;
        }
    }
}
