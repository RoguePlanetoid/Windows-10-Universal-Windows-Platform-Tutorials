using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace DialControl
{
    public sealed class Dial : Control
    {
        public Dial()
        {
            this.DefaultStyleKey = typeof(Dial);
        }

        Grid knob;
        RotateTransform value;
        bool hasCapture = false;

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(double),
        typeof(Dial), null);

        public static readonly DependencyProperty KnobProperty =
        DependencyProperty.Register("Knob", typeof(UIElement),
        typeof(Dial), null);

        public static readonly DependencyProperty FaceProperty =
        DependencyProperty.Register("Face", typeof(UIElement),
        typeof(Dial), null);

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public UIElement Knob
        {
            get { return (UIElement)GetValue(KnobProperty); }
            set { SetValue(KnobProperty, value); }
        }

        public UIElement Face
        {
            get { return (UIElement)GetValue(FaceProperty); }
            set { SetValue(FaceProperty, value); }
        }

        private double angleQuadrant(double width, double height, Windows.Foundation.Point point)
        {
            double radius = width / 2;
            Windows.Foundation.Point centre = new Windows.Foundation.Point(radius, height / 2);
            Windows.Foundation.Point start = new Windows.Foundation.Point(0, height / 2);
            double triangleTop = Math.Sqrt(Math.Pow((point.X - centre.X), 2)
              + Math.Pow((centre.Y - point.Y), 2));
            double triangleHeight = (point.Y > centre.Y) ?
              point.Y - centre.Y : centre.Y - point.Y;
            return ((triangleHeight * Math.Sin(90)) / triangleTop) * 100;
        }

        private double getAngle(Windows.Foundation.Point point)
        {
            double diameter = knob.ActualWidth;
            double height = knob.ActualHeight;
            double radius = diameter / 2;
            double rotation = angleQuadrant(diameter, height, point);
            if ((point.X > radius) && (point.Y <= radius))
            {
                rotation = 90.0 + (90.0 - rotation);
            }
            else if ((point.X > radius) && (point.Y > radius))
            {
                rotation = 180.0 + rotation;
            }
            else if ((point.X < radius) && (point.Y > radius))
            {
                rotation = 270.0 + (90.0 - rotation);
            }
            return rotation;
        }

        private void setPosition(double rotation)
        {
            if (Minimum > 0 && Maximum > 0 && Minimum < 360 && Maximum <= 360)
            {
                if (rotation < Minimum) { rotation = Minimum; }
                if (rotation > Maximum) { rotation = Maximum; }
            }
            value.Angle = rotation;
            Value = rotation;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            knob = ((Grid)GetTemplateChild("Knob"));
            value = ((RotateTransform)GetTemplateChild("DialValue"));
            if (Minimum > 0 && Minimum < 360) { setPosition(Minimum); }
            knob.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
            {
                hasCapture = false;
            };
            knob.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
            {
                hasCapture = true;
                setPosition(getAngle(e.GetCurrentPoint(knob).Position));
            };
            knob.PointerMoved += (object sender, PointerRoutedEventArgs e) =>
            {
                if (hasCapture)
                {
                    setPosition(getAngle(e.GetCurrentPoint(knob).Position));
                }
            };
            knob.PointerExited += (object sender, PointerRoutedEventArgs e) =>
            {
                hasCapture = false;
            };
        }
    }
}
