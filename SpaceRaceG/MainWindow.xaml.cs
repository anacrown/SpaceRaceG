using System;
using System.Windows;
using SpaceRaceG.AI;
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
            bot.DataProviderTimeChanged += (sender, i) => Slider.Value = i;
            bot.DataProviderStarted += (sender, provider) => Slider.Maximum = provider.FrameCount - 1;
        }
        
        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var frameNumber = (int)e.NewValue;

            if (_bot.FrameNumber != frameNumber)
                _bot.MoveToFrame(frameNumber);
        }
    }
}
