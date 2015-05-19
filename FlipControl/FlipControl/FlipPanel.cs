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

namespace FlipControl
{
    public sealed class FlipPanel : Control
    {
        public FlipPanel()
        {
            this.DefaultStyleKey = typeof(FlipPanel);
        }

        public static readonly DependencyProperty FrontContentProperty =
        DependencyProperty.Register("FrontContent", typeof(object),
        typeof(FlipPanel), null);

        public static readonly DependencyProperty BackContentProperty =
        DependencyProperty.Register("BackContent", typeof(object),
        typeof(FlipPanel), null);

        public static readonly DependencyProperty IsFlippedProperty =
        DependencyProperty.Register("IsFlipped", typeof(bool),
        typeof(FlipPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(FlipPanel), null);

        public object FrontContent
        {
            get { return GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        public object BackContent
        {
            get { return GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void changeVisualState(bool useTransitions)
        {
            if (IsFlipped)
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Windows.UI.Xaml.Controls.Primitives.ToggleButton flipButton = 
                (Windows.UI.Xaml.Controls.Primitives.ToggleButton)GetTemplateChild("FlipButton");
            if (flipButton != null)
            {
                flipButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    changeVisualState(true);
                };
            }
            Windows.UI.Xaml.Controls.Primitives.ToggleButton flipButtonAlt = 
                (Windows.UI.Xaml.Controls.Primitives.ToggleButton)GetTemplateChild("FlipButtonAlternative");
            if (flipButtonAlt != null)
            {
                flipButtonAlt.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    changeVisualState(true);
                };
            }
            changeVisualState(false);
        }
    }
}
