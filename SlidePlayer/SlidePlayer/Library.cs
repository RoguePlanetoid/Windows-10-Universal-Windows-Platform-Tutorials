using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

public class Library
{
    public delegate void PlayingEvent(BitmapImage image, int index);
    public event PlayingEvent Playing;
    public delegate void StoppedEvent();
    public event StoppedEvent Stopped;

    private List<BitmapImage> items = new List<BitmapImage>();
    private int index = 0;
    private bool paused = false;

    public bool IsPlaying { get; set; }
    public int Speed { get; set; }
    public int Position { get; set; }

    public void Go(ref Image display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                display.Source = new BitmapImage(new Uri(value));
            }
            catch
            {

            }
        }
    }

    public double Add(string value)
    {
        items.Add(new BitmapImage(new Uri(value)));
        return items.Count - 1;
    }

    public double Remove(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
        return items.Count - 1;
    }

    public async void Play()
    {
        if (items.Any() && (!paused || !IsPlaying))
        {
            IsPlaying = true;
            paused = false;
            while (IsPlaying)
            {
                if (items.Count > 0)
                {
                    if (index < items.Count)
                    {

                        Playing(items[index], index);
                        index += 1;
                    }
                    else
                    {
                        this.Stop();
                    }
                }
                await Task.Delay(Speed);
            }
        }
    }

    public void Pause()
    {
        if (items.Any() && IsPlaying)
        {
            paused = true;
            IsPlaying = false;
        }
    }

    public void Stop()
    {
        if (items.Any())
        {
            index = 0;
            paused = false;
            IsPlaying = false;
            Stopped();
        }
    }
}
