using System;

namespace SpaceRaceG.AI
{
    public static class DirectionExtension
    {
        public static SolverCommand GetCommand(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return SolverCommand.Up;
                case Direction.UpRight:
                    return SolverCommand.UpRight;
                case Direction.UpLeft:
                    return SolverCommand.UpLeft;

                case Direction.Right:
                    return SolverCommand.Right;
                case Direction.Left:
                    return SolverCommand.Left;

                case Direction.Down:
                    return SolverCommand.Down;
                case Direction.DownRight:
                    return SolverCommand.DownRight;
                case Direction.DownLeft:
                    return SolverCommand.DownLeft;

                case Direction.Unknown:
                    return SolverCommand.Empty;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}