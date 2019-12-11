using System;
using System.Windows;
using SpaceRaceG.DataProvider;

namespace SpaceRaceG
{
    public partial class MainWindow : Window
    {
        private readonly SpaceRaceBot _bot;

        public MainWindow(SpaceRaceBot bot)
        {
            _bot = bot;
            InitializeComponent();

            GameControl.Content = new SpaceRaceGame(bot);

            if (bot.Settings.DataSource != DataSource.FILE) return;

            Slider.Visibility = Visibility.Visible;
            bot.DataProvider.Started += DataProviderOnStarted;
            bot.DataProvider.TimeChanged += DataProviderOnTimeChanged;
        }

        private void DataProviderOnStarted(object sender, EventArgs e)
        {
            Slider.Maximum = _bot.DataProvider.FrameCount - 1;
        }

        private void DataProviderOnTimeChanged(object sender, int e)
        {
            Slider.Value = e;
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var frameNumber = (int)e.NewValue;

            if (_bot.DataProvider.FrameNumber != frameNumber)
                _bot.DataProvider.MoveToFrame(frameNumber);
        }
    }
}
