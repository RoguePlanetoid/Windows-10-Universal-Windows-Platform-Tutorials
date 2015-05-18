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

namespace ClockControl
{
    public sealed partial class Clock : UserControl
    {
        public Clock()
        {
            this.InitializeComponent();
            init();
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private Canvas markers = new Canvas();
        private Canvas face = new Canvas();
        private Windows.UI.Xaml.Shapes.Rectangle secondsHand;
        private Windows.UI.Xaml.Shapes.Rectangle minutesHand;
        private Windows.UI.Xaml.Shapes.Rectangle hoursHand;

        private Brush rimForeground = new SolidColorBrush(Windows.UI.Colors.White);
        private Brush rimBackground = new SolidColorBrush(Windows.UI.Colors.Black);

        private int secondsWidth = 1;
        private int secondsHeight;
        private int minutesWidth = 5;
        private int minutesHeight;
        private int hoursWidth = 8;
        private int hoursHeight;
        private double diameter;
        private bool isrealtime = true;
        private bool showSeconds = true;
        private bool showMinutes = true;
        private bool showHours = true;

        public bool IsRealTime { get { return isrealtime; } set { isrealtime = value; } }
        public bool ShowSeconds { get { return showSeconds; } set { showSeconds = value; } }
        public bool ShowMinutes { get { return showMinutes; } set { showMinutes = value; } }
        public bool ShowHours { get { return showHours; } set { showHours = value; } }
        public DateTime Time { get; set; }

        private Windows.UI.Xaml.Shapes.Rectangle hand(double width, double height, double radiusX, double radiusY, double thickness)
        {
            Windows.UI.Xaml.Shapes.Rectangle hand = new Windows.UI.Xaml.Shapes.Rectangle();
            hand.Width = width;
            hand.Height = height;
            hand.Fill = Foreground;
            hand.StrokeThickness = thickness;
            hand.RadiusX = radiusX;
            hand.RadiusY = radiusY;
            return hand;
        }

        private void removeHand(ref Windows.UI.Xaml.Shapes.Rectangle hand)
        {
            if (hand != null && face.Children.Contains(hand))
            {
                face.Children.Remove(hand);
            }
        }

        private void addHand(ref Windows.UI.Xaml.Shapes.Rectangle hand)
        {
            if (!face.Children.Contains(hand))
            {
                face.Children.Add(hand);
            }
        }

        private TransformGroup transformGroup(double Angle, double X, double Y)
        {
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform firstTranslate = new TranslateTransform();
            firstTranslate.X = X;
            firstTranslate.Y = Y;
            transformGroup.Children.Add(firstTranslate);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = Angle;
            transformGroup.Children.Add(rotateTransform);
            TranslateTransform secondTranslate = new TranslateTransform();
            secondTranslate.X = diameter / 2;
            secondTranslate.Y = diameter / 2;
            transformGroup.Children.Add(secondTranslate);
            return transformGroup;
        }

        private void secondHand(int seconds)
        {
            removeHand(ref secondsHand);
            if (ShowSeconds)
            {
                secondsHand = hand(secondsWidth, secondsHeight, 0, 0, 0);
                secondsHand.RenderTransform = transformGroup(seconds * 6,
                -secondsWidth / 2, -secondsHeight + 4.25);
                addHand(ref secondsHand);
            }
        }

        private void minuteHand(int minutes, int seconds)
        {
            removeHand(ref minutesHand);
            if (ShowMinutes)
            {
                minutesHand = hand(minutesWidth, minutesHeight, 2, 2, 0.6);
                minutesHand.RenderTransform = transformGroup(6 * minutes + seconds / 10,
                -minutesWidth / 2, -minutesHeight + 4.25);
                addHand(ref minutesHand);
            }
        }

        private void hourHand(int hours, int minutes, int seconds)
        {
            removeHand(ref hoursHand);
            if (ShowHours)
            {
                hoursHand = hand(hoursWidth, hoursHeight, 3, 3, 0.6);
                hoursHand.RenderTransform = transformGroup(30 * hours + minutes / 2 + seconds / 120,
                -hoursWidth / 2, -hoursHeight + 4.25);
                addHand(ref hoursHand);
            }
        }

        private void Layout(ref Canvas canvas)
        {
            Windows.UI.Xaml.Shapes.Ellipse rim = new Windows.UI.Xaml.Shapes.Ellipse();
            canvas.Children.Clear();
            diameter = canvas.Width;
            double inner = diameter - 15;
            rim.Height = diameter;
            rim.Width = diameter;
            rim.Stroke = RimBackground;
            rim.StrokeThickness = 20;
            canvas.Children.Add(rim);
            markers.Children.Clear();
            markers.Width = inner;
            markers.Height = inner;
            for (int i = 0; i < 60; i++)
            {
                Windows.UI.Xaml.Shapes.Rectangle marker = new Windows.UI.Xaml.Shapes.Rectangle();
                marker.Fill = RimForeground;
                if ((i % 5) == 0)
                {
                    marker.Width = 3;
                    marker.Height = 8;
                    marker.RenderTransform = transformGroup(i * 6, -(marker.Width / 2),
                    -(marker.Height * 2 + 4.5 - rim.StrokeThickness / 2 - inner / 2 - 6));
                }
                else
                {
                    marker.Width = 1;
                    marker.Height = 4;
                    marker.RenderTransform = transformGroup(i * 6, -(marker.Width / 2),
                    -(marker.Height * 2 + 12.75 - rim.StrokeThickness / 2 - inner / 2 - 8));
                }
                markers.Children.Add(marker);
            }
            canvas.Children.Add(markers);
            face.Width = diameter;
            face.Height = diameter;
            canvas.Children.Add(face);
            secondsHeight = (int)diameter / 2 - 20;
            minutesHeight = (int)diameter / 2 - 40;
            hoursHeight = (int)diameter / 2 - 60;
        }

        private void init()
        {
            Layout(ref Display);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (object sender, object e) =>
            {
                if (IsRealTime) Time = DateTime.Now;
                secondHand(Time.Second);
                minuteHand(Time.Minute, Time.Second);
                hourHand(Time.Hour, Time.Minute, Time.Second);
            };
            timer.Start();
        }

        public Brush RimForeground
        {
            get { return rimForeground; }
            set
            {
                rimForeground = value == null ? new SolidColorBrush(Windows.UI.Colors.Black) : value;
                Layout(ref Display);
            }
        }

        public Brush RimBackground
        {
            get { return rimBackground; }
            set
            {
                rimBackground = value == null ? new SolidColorBrush(Windows.UI.Colors.White) : value;
                Layout(ref Display);
            }
        }

        public bool Enabled
        {
            get { return timer.IsEnabled; }
            set
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
            }
        }
    }
}
