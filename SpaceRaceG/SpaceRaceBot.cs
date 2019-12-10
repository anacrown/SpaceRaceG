using System;
using SpaceRaceG.DataProvider;

namespace SpaceRaceG
{
    public class SpaceRaceBot
    {
        public SpaceRaceBotSettings Settings { get; }
        public IDataProvider DataProvider { get; }
        public IDataLogger DataLogger { get; }

        public SpaceRaceBot(SpaceRaceBotSettings settings)
        {
            Settings = settings;

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
            DataLogger = new FileSystemDataLogger(new FileSystemDataLoggerSettings());

            DataProvider.Start();
        }

        private void DataProviderOnDataReceived(object sender, DataFrame frame)
        {
            DataLogger.Log(DataProvider.Name, DataProvider.StartTime, frame);

            DataProvider.SendResponse("");
        }
    }
}