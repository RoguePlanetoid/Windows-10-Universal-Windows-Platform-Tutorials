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

namespace DataInput
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

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Email.Text = string.Empty;
            Website.Text = string.Empty;
            Telephone.Text = string.Empty;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Email.Text = Library.LoadSetting("Email");
            Website.Text = Library.LoadSetting("Website");
            Telephone.Text = Library.LoadSetting("Telephone");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Library.SaveSetting("Email", Email.Text);
            Library.SaveSetting("Website", Website.Text);
            Library.SaveSetting("Telephone", Telephone.Text);
        }
    }
}
