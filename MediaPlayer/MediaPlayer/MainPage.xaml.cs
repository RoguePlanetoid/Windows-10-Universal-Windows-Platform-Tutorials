using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediaPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public Library Library = new Library();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Library.Init();
            Library.Playing += () =>
            {
                Position.Value = (int)Display.Position.TotalMilliseconds;
            };
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Display.CurrentState == MediaElementState.Playing)
            {
                Display.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                Display.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Display.Stop();
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Library.Go(ref Display, Value.Text, e);
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Volume != null)
            {
                Display.Volume = (double)Volume.Value;
            }
        }

        private void Display_MediaOpened(object sender, RoutedEventArgs e)
        {
            Position.Maximum = (int)Display.NaturalDuration.TimeSpan.TotalMilliseconds;
            Display.Play();
            Play.Icon = new SymbolIcon(Symbol.Pause);
            Play.Label = "Pause";
        }

        private void Display_MediaEnded(object sender, RoutedEventArgs e)
        {
            Play.Icon = new SymbolIcon(Symbol.Play);
            Play.Label = "Play";
            Display.Stop();
            Position.Value = 0;
        }

        private void Display_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Library.Timer(Display.CurrentState == MediaElementState.Playing);
        }
    }
}
