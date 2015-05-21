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

namespace Location
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
        Library Library = new Library();

        private async void Location_Click(object sender, RoutedEventArgs e)
        {
            Windows.Devices.Geolocation.Geopoint position = await Library.Position();
            DependencyObject marker = Library.Marker();
            Display.Children.Add(marker);
            Windows.UI.Xaml.Controls.Maps.MapControl.SetLocation(marker, position);
            Windows.UI.Xaml.Controls.Maps.MapControl.SetNormalizedAnchorPoint(marker, new Point(0.5, 0.5));
            Display.ZoomLevel = 12;
            Display.Center = position;
        }
    }
}
