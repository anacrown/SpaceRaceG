using System;
using System.Threading;
using SpaceRaceG.AI;
using SpaceRaceG.DataProvider;

namespace SpaceRaceG
{
    public class SpaceRaceBot
    {
        public SpaceRaceBotSettings Settings { get; }
        private IDataProvider DataProvider { get; }
        private IDataLogger DataLogger { get; }
        private SpaceRaceSolver Solver { get; }

        public int FrameNumber => DataProvider.FrameNumber;

        public event EventHandler<Board> BoardLoaded;
        public event EventHandler<IDataProvider> DataProviderStarted;
        public event EventHandler<IDataProvider> DataProviderStopped;
        public event EventHandler<int> DataProviderTimeChanged;

        public SpaceRaceBot(SpaceRaceBotSettings settings)
        {
            Settings = settings;

            Solver = new SpaceRaceSolver();

            switch (Settings.DataSource)
            {
                case DataSource.WEB:
                    DataProvider = new WebSocketDataProvider(new WebSocketDataProviderSettings(new IdentityUser(Settings.Uri)));
                    break;
                case DataSource.FILE:
                    DataProvider = new FileSystemDataProvider(new FileSystemDataProviderSettings(Settings.Path));
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            DataProvider.DataReceived += DataProviderOnDataReceived;
            DataProvider.Started += (sender, args) => DataProviderStarted?.Invoke(this, (IDataProvider)sender);
            DataProvider.Stopped += (sender, args) => DataProviderStopped?.Invoke(this, (IDataProvider)sender);
            DataProvider.TimeChanged += (sender, i) => DataProviderTimeChanged?.Invoke(this, i);

            DataLogger = new FileSystemDataLogger(new FileSystemDataLoggerSettings());
        }

        private void DataProviderOnDataReceived(object sender, DataFrame frame)
        {
            DataLogger.Log(DataProvider.Name, DataProvider.StartTime, frame);

            var board = Board.Next(frame);
            BoardLoaded?.BeginInvoke(this, board, ar => {}, null);

            if (Solver.Answer(board, out var response))
            {
                DataLogger.Log(DataProvider.Name, DataProvider.StartTime, DateTime.Now, frame.FrameNumber, response);

                Thread.Sleep(100);
                DataProvider.SendResponse(response);

                return;
            }

            DataProvider.SendResponse("");
        }

        public void Start() => DataProvider?.Start();
        public void MoveToFrame(int frameNumber) => DataProvider.MoveToFrame(frameNumber);
    }
}