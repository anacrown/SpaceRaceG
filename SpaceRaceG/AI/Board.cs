using System;
using System.Linq;
using SpaceRaceG.DataProvider;
using SpaceRaceG.Properties;

namespace SpaceRaceG.AI
{
    public class Board : Matrix<Cell>
    {
        private static int _current;
        private static readonly int PoolSize;
        private static readonly Board[] Boards;

        public DataFrame Frame { get; private set; }

        static Board()
        {
            _current = 0;
            PoolSize = Settings.Default.BoardPoolSize;

            Boards = new Board[PoolSize];
        }
        public static Board Next(DataFrame frame) => (Boards[_current = (_current + 1) % PoolSize] ?? new Board(frame)).Reset(frame);

        protected Board(DataFrame frame) : base(new Size((int)Math.Sqrt(frame.Board.Length)))
        {
            for (var i = 0; i < Cells.Length; i++)
                Cells[i].Reset(SpaceRaceSolver.GetElement(frame.Board[i]), new Point(i % Size.Width, i / Size.Width));
        }
        private Board Reset(DataFrame frame)
        {
            Frame = frame;

            for (var i = 0; i < Cells.Length; i++)
                Cells[i].Reset(SpaceRaceSolver.GetElement(frame.Board[i]));

            return this;
        }
    }
}