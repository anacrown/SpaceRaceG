using System;

namespace SpaceRaceG.DataProvider
{
    public struct DataFrame
    {
        public DateTime Time { get; }

        public string Board { get; }

        public int FrameNumber { get; }

        public DataFrame(DateTime time, string board, int frameNumber)
        {
            Time = time;
            Board = board;
            FrameNumber = frameNumber;
        }

    }
}