using System;
using System.Collections.Generic;
using System.Linq;
using SpaceRaceG.AI;

//-uri ye"http://192.168.1.150:8080/codenjoy-contest/board/plar/demo6@codenjoy.com?code=7881037345545140492&gameName=codingbattle2019"
//-file "D:\HACKATHON\SpaceRaceG\SpaceRaceG\App_Data\Logs\WEB [demo6@codenjoy.com]"

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

                case 'V': return Element.HERO_down;
                case '↙': return Element.HERO_down_left;
                case '➘': return Element.HERO_down_right;
                case '<': return Element.HERO_left;
                case '>': return Element.HERO_right;
                case 'A': return Element.HERO_up;
                case '↖': return Element.HERO_up_left;
                case '➚': return Element.HERO_up_right;

                case '☻': return Element.OTHER_HERO;
                case '+': return Element.DEAD_HERO;
                case '$': return Element.GOLD;
                case '♣': return Element.BOMB;
                case '0': return Element.STONE;
                case '7': return Element.BULLET_PACK;

                case 'n':
                case '*': return Element.BULLET;
                default: throw new Exception("Invalid Symbol in board");
            }
        }

        public bool Answer(Board board, out string response)
        {
            response = String.Empty;
            var player = board.SingleOrDefault(c => IsHero(c.Element));
            board.AttentionPoints = getAttentionPoints(board).ToArray();

            if (player != null)
            {
                // var packs = board.Where(c => c.Element == Element.BULLET_PACK).ToArray();
                // var paths_to_pack = packs.Select(c =>
                // {
                //     //TODO: CheckedPoints - опасные клетки
                //     var map = new Map2(board, board.AttentionPoints);
                //     map.Check(player.P);
                //     return (path: map.Tracert(c.P), target: c);
                // }).ToArray();

                var pack = board.FirstOrDefault(c => c.Element == Element.BULLET_PACK);
                if (pack != null)
                {

                    var map = new Map2(board, board.AttentionPoints);
                    map.Check(player.P);
                    var path_short = (path: map.Tracert(pack.P), target: pack);

                    board.Map = map;

                    board.PathPoints = path_short.path;

                    var next = (path_short.path.Length >= 1) ? path_short.path.First() : path_short.target.P;
                    var direction = player.P.GetDirectionTo(next);
                    var command = direction.GetCommand();

                    board.Responce = response = command.ToString();
                    return true;
                }
            }

            return false;
        }

        private bool IsHero(Element element)
        {
            return element == Element.HERO || 
                   element == Element.HERO_down || 
                   element == Element.HERO_down_left ||
                   element == Element.HERO_down_right || 
                   element == Element.HERO_left || 
                   element == Element.HERO_right || 
                   element == Element.HERO_up ||
                   element == Element.HERO_up_left || 
                   element == Element.HERO_up_right;
        }

        private IEnumerable<Point> _getAttentionPoints(Board board)
        {
            foreach (var cell in board)
            {
                if (cell.Element == Element.BULLET)
                    yield return cell.P[Direction.Up];

                if (cell.Element == Element.STONE)
                    yield return cell.P[Direction.Down];

                if (cell.Element == Element.BOMB)
                {
                    yield return cell.P;
                    foreach (var neighbor in cell.P.GetNeighbors(board.Size))
                        yield return neighbor[Direction.Down];
                }

                if (cell.Element == Element.WALL)
                    yield return cell.P;

                if (cell.Element == Element.OTHER_HERO)
                    yield return cell.P;
            }

            yield break;
        }

        private IEnumerable<Point> getAttentionPoints(Board board) =>
            _getAttentionPoints(board).Where(p => p.OnBoard(board.Size));
    }
}
