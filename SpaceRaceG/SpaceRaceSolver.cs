using System;
using System.Collections.Generic;
using System.Linq;
using SpaceRaceG.AI;

//-file "C:\Users\misho\source\repos\SpaceRaceG\SpaceRaceG\App_Data\Logs\WEB [nais@mail.ru]"\
//-uri "http://92.124.142.118:8080/another-context/board/player/nais@mail.ru?code=13476795611535248716"
//-uri "http://192.168.1.150:8080/codenjoy-contest/board/player/demo6@codenjoy.com?code=7881037345545140492&gameName=codingbattle2019"

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
                case '*': return Element.BULLET;
                default: throw new Exception("Invalid Symbol in board");
            }
        }

        public bool Answer(Board board, out string response)
        {
            response = String.Empty;
            var player = board.SingleOrDefault(c => IsHero(c.Element));

            if (player != null)
            {
                var packs = board.Where(c => c.Element == Element.BULLET_PACK).ToArray();
                var paths_to_pack = packs.Select(c =>
                {
                    //TODO: CheckedPoints - опасные клетки
                    var map = new Map2(board, getAttentionCells(board).Select(c2 => c2.P[Direction.Down]).Where(p => p.OnBoard(board.Size)).ToArray());
                    map.Check(c.P);
                    return (path: map.Tracert(player.P), target: c);
                }).ToArray();

                var path_short = paths_to_pack.MinSingle(p => p.path.Length);

                var next = (path_short.path.Length >= 2) ? path_short.path.Skip(1).First() : path_short.target.P;
                var direction = player.P.GetDirectionTo(next);
                var command = direction.GetCommand();

                response = command.ToString();
                return true;
            }

            return false;
        }

        private bool IsHero(Element element)
        {
            return element == Element.HERO ||
                   element == Element.HERO_up ||
                   element == Element.HERO_down ||
                   element == Element.HERO_down_left ||
                   element == Element.HERO_down_right ||
                   element == Element.HERO_right ||
                   element == Element.HERO_left;
        }

        private IEnumerable<Cell> getAttentionCells(Board board)
        {
            foreach (var cell in board)
            {
                if (cell.Element == Element.BULLET)
                    yield return cell[Direction.Up];

                if (cell.Element == Element.STONE)
                    yield return cell;

                if (cell.Element == Element.BOMB)
                {
                    yield return cell;
                    foreach (var neighbor in cell.P.GetNeighbors(board.Size).Select(p => board[p]))
                        yield return neighbor;
                }
            }

            yield break;
        }
    }
}
