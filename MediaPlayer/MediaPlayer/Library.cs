using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

public class Library
{
    public delegate void PlayingEvent();
    public event PlayingEvent Playing;

    private DispatcherTimer timer = new DispatcherTimer();

    public void Init()
    {
        timer.Tick += (object sender, object e) =>
        {
            if (Playing != null) Playing();
        };
    }

    public void Timer(bool enabled)
    {
        if (enabled) timer.Start(); else timer.Stop();
    }

    public void Go(ref MediaElement display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                display.Source = new Uri(value, UriKind.Absolute);
                display.Play();
            }
            catch
            {

            }
        }
    }
}