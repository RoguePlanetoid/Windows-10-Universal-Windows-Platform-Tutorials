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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CarouselControl
{
    public sealed partial class Carousel : UserControl
    {
        public Carousel()
        {
            this.InitializeComponent();
            init();
        }

        private Windows.UI.Xaml.Media.Animation.Storyboard animation =
            new Windows.UI.Xaml.Media.Animation.Storyboard();
        private List<Windows.UI.Xaml.Media.Imaging.BitmapImage> list =
            new List<Windows.UI.Xaml.Media.Imaging.BitmapImage>();

        private Point point;
        private Point radius = new Point { X = -20, Y = 200 };
        private double speed = 0.0125;
        private double perspective = 55;
        private double distance;

        private void layout(ref Canvas display)
        {
            display.Children.Clear();
            for (int index = 0; index < list.Count(); index++)
            {
                Image item = new Image();
                item.Width = 150;
                item.Source = list[index];
                item.Tag = index * ((Math.PI * 2) / list.Count);
                point.X = Math.Cos((double)item.Tag) * radius.X;
                point.Y = Math.Sin((double)item.Tag) * radius.Y;
                Canvas.SetLeft(item, point.X - (item.Width - perspective));
                Canvas.SetTop(item, point.Y);
                distance = 1 / (1 - (point.X / perspective));
                item.RenderTransform = new ScaleTransform();
                item.Opacity = ((ScaleTransform)item.RenderTransform).ScaleX =
                    ((ScaleTransform)item.RenderTransform).ScaleY = distance;
                display.Children.Add(item);
            }
        }

        private void rotate()
        {
            foreach (Image item in Display.Children)
            {
                double angle = (double)item.Tag;
                angle -= speed;
                item.Tag = angle;
                point.X = Math.Cos(angle) * radius.X;
                point.Y = Math.Sin(angle) * radius.Y;
                Canvas.SetLeft(item, point.X - (item.Width - perspective));
                Canvas.SetTop(item, point.Y);
                if (radius.X >= 0)
                {
                    distance = 1 * (1 - (point.X / perspective));
                    Canvas.SetZIndex(item, -(int)(point.X));
                }
                else
                {
                    distance = 1 / (1 - (point.X / perspective));
                    Canvas.SetZIndex(item, (int)(point.X));
                }
                item.Opacity = ((ScaleTransform)item.RenderTransform).ScaleX =
                    ((ScaleTransform)item.RenderTransform).ScaleY = distance;
            }
            animation.Begin();
        }

        private void init()
        {
            animation.Completed += (object sender, object e) =>
            {
                rotate();
            };
            animation.Begin();
        }

        public void Add(Windows.UI.Xaml.Media.Imaging.BitmapImage image)
        {
            list.Add(image);
            layout(ref Display);
        }

        public void RemoveLast()
        {
            if (list.Any())
            {
                list.Remove(list.Last());
                layout(ref Display);
            }
        }

        public void New()
        {
            list.Clear();
            layout(ref Display);
        }
    }
}
