using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceRaceG.AI
{
    public class Map2 : Matrix<MapEntry>
    {
        public Board Board { get; }

        public Map2(Board board, params Point[] checkedPoints) : base(board.Size)
        {
            Board = board;

            foreach (var checkedPoint in checkedPoints)
            {
                this[checkedPoint].Weight = -1;
                this[checkedPoint].BChecked = true;
            }

            for (var i = 0; i < Size.Width; i++)
            for (var j = 0; j < Size.Height; j++)
                this[i, j].Position = new Point(i, j);
        }

        public Map2 Check(params Point[] startPoints)
        {
            foreach (var startPoint in startPoints)
            {
                this[startPoint].BChecked = true;
                this[startPoint].BWatched = true;
                this[startPoint].Weight = 0;
            }

            var pointList = startPoints.ToList();

            do
            {
                var array = pointList.ToArray();
                pointList.Clear();

                foreach (var p in array)
                    pointList.AddRange(check(p));
            }
            while (pointList.Count > 0);

            return this;
        }

        private IEnumerable<Point> check(Point point)
        {
            this[point].BChecked = true;
            var array = point.GetCrossVicinity(Size).Where(n => !this[n].BChecked && !this[n].BWatched).ToArray();

            foreach (var index in array)
            {
                this[index].BWatched = true;
                this[index].Weight += this[point].Weight + 1;
            }

            return array;
        }

        public Point[] Tracert(Point point) => tracert_forward(point).Reverse().ToArray();

        private IEnumerable<Point> tracert_forward(Point point)
        {
            var entry = this[point];

            var iAntiCycle = Math.Max(Board.Size.Width, Board.Size.Height);
            while (entry != null && entry.Weight > 0)
            {
                iAntiCycle--;
                if (iAntiCycle < 0) yield break;

                yield return entry.Position;
                var array = entry.Position.GetCrossVicinity(Size).Select(p => this[p]).Where(e => e.Weight != -1 && e.Weight < entry.Weight).ToArray();
                if (!array.Any()) yield break;

                entry = array.MinSingle(e => e.Weight);
            }
        }
    }
}