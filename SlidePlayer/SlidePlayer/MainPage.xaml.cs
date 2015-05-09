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

namespace SlidePlayer
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
            Library.Speed = (int)Speed.Value;
            Library.Playing += (Windows.UI.Xaml.Media.Imaging.BitmapImage image, int index) =>
            {
                Display.Source = image;
                Position.Value = index;
            };
            Library.Stopped += () =>
            {
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
                Display.Source = null;
                Position.Value = 0;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = Library.Add(Value.Text);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = Library.Remove((int)Position.Value);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Library.IsPlaying)
            {
                Library.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                Library.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Library.Stop();
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Library.Go(ref Display, Value.Text, e);
        }

        private void Position_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Library.Position = (int)Position.Value;
        }

        private void Speed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Speed != null)
            {
                Library.Speed = (int)Speed.Value;
            }
        }
    }
}
