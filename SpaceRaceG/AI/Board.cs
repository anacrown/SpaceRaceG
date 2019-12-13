using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRaceG.DataProvider;
using SpaceRaceG.Properties;

namespace SpaceRaceG.AI
{
    public class Board : IEnumerable<Cell>
    {
        private static int _current;
        private static readonly int PoolSize;
        private static readonly Board[] Boards;

        private Cell[] _cells;

        public int Size { get; private set; }
        public DataFrame Frame { get; private set; }
        
        static Board()
        {
            _current = 0;
            PoolSize = Settings.Default.BoardPoolSize;

            Boards = new Board[PoolSize];
            for (var i = 0; i < PoolSize; i++) Boards[i] = new Board();
        }
        private Board() { }
        public static Board Current => Boards[_current];
        public static Board Next(DataFrame frame) => Boards[_current = (_current + 1) % PoolSize].Reset(frame);

        private Board Reset(DataFrame frame)
        {
            Frame = frame;

            if (_cells == null)
            {
                _cells = new Cell[frame.Board.Length];
                Size = (int)Math.Sqrt(frame.Board.Length);

                for (var i = 0; i < _cells.Length; i++)
                    _cells[i] = new Cell(new Point(i % Size, i / Size), SpaceRaceSolver.GetElement(frame.Board[i]));
            }
            else
            {
                for (var i = 0; i < _cells.Length; i++)
                    _cells[i].Reset(SpaceRaceSolver.GetElement(frame.Board[i]));
            }

            return this;
        }
        public Cell this[int i, int j] => _cells[i + j * Size];
        public Cell this[Point p] => this[p.X, p.Y];

        public IEnumerator<Cell> GetEnumerator() => new BoardEnumerator<Cell>(_cells);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class BoardEnumerator<TU> : IEnumerator<TU>
        {
            private readonly IEnumerator _enumerator;
            public BoardEnumerator(IEnumerable<TU> cells) => _enumerator = cells.GetEnumerator();
            public void Dispose() { }
            public bool MoveNext() => _enumerator.MoveNext();
            public void Reset() => _enumerator.Reset();
            TU IEnumerator<TU>.Current => (TU)_enumerator.Current;
            object IEnumerator.Current => Current;
        }
    }
}