using System;
using System.Linq;
using SpaceRaceG.AI;

//-file "C:\Users\misho\source\repos\SpaceRaceG\SpaceRaceG\App_Data\Logs\WEB [nais@mail.ru]"\
//-uri "http://92.124.142.118:8080/another-context/board/player/nais@mail.ru?code=13476795611535248716"

namespace SpaceRaceG
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

        public bool Answer(Board board, out string response)
        {
            response = string.Empty;

            var pack = board.SingleOrDefault(c => c.Element == Element.BULLET_PACK);
            var player = board.SingleOrDefault(c => c.Element == Element.HERO);

            if (player != null && pack != null)
            {
                var bulletPackMap = new Map2(board).Check(player.P);
                var path = bulletPackMap.Tracert(pack.P);

                if (path.Length > 1)
                {
                    var next = path.Skip(1).First();
                    var direction = player.P.GetDirectionTo(next);

                    var command = direction.GetCommand();

                    response = command.ToString();
                    return true;
                }
            }

            return false;
        }
    }
}
