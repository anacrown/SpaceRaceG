using System.Windows;

namespace SpaceRaceG
{
    public partial class MainWindow : Window
    {
        public MainWindow(SpaceRaceBot bot)
        {
            InitializeComponent();

            GameControl.Content = new SpaceRaceGame(bot);
            
            Slider.Visibility = bot.Settings.DataSource == DataSource.FILE ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
