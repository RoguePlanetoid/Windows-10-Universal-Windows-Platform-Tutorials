using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

public class Library
{
    public void Back(ref WebView web)
    {
        if (web.CanGoBack)
        {
            web.GoBack();
        }
    }

    public void Forward(ref WebView web)
    {
        if (web.CanGoForward)
        {
            web.GoForward();
        }
    }

    public void Go(ref WebView web, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            try
            {
                web.Navigate(new Uri(value));
            }
            catch
            {

            }
            web.Focus(FocusState.Keyboard);
        }
    }
}
