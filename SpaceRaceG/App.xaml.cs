using System;
using System.Linq;
using System.Windows;

namespace SpaceRaceG
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppData.Set();

            base.OnStartup(e);

            var botSettings = new SpaceRaceBotSettings();
            switch (e.Args.FirstOrDefault())
            {
                case "-uri":
                    botSettings.Uri = e.Args[1];
                    botSettings.DataSource = DataSource.WEB;
                    break;
                case "-file":
                    botSettings.Path = e.Args[1];
                    botSettings.DataSource = DataSource.FILE;
                    break;
                default: throw new Exception("Args not supported");
            }

            var bot = new SpaceRaceBot(botSettings);

            MainWindow = new MainWindow(bot);
            MainWindow.Show();

            bot.Start();
        }
    }
}

//-file "C:\Users\misho\source\repos\SpaceRaceG\SpaceRaceG\App_Data\Logs\WEB [nais@mail.ru]"\
//-uri "http://localhost:8080/another-context/board/player/nais@mail.ru?code=13476795611535248716"
