using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

public class Item
{
    public string Id { get; set; }
    public string Content { get; set; }
    public Brush Colour { get; set; }
}

public class Library
{
    private Random random = new Random((int)DateTime.Now.Ticks);

    private Color stringToColour(string value)
    {
        return Color.FromArgb(
        Byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
    }

    public static Rect getElementRect(FrameworkElement element)
    {
        GeneralTransform buttonTransform = element.TransformToVisual(null);
        Point point = buttonTransform.TransformPoint(new Point());
        return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
    }

    public async void Add(ListBox display, string value, ComboBox colour, object selection)
    {
        string id = random.Next(1, 100000000).ToString();
        SecondaryTile tile = new SecondaryTile(id, value, id, new Uri("ms-appx:///"), TileSize.Default);
        Color background = stringToColour(((ComboBoxItem)colour.SelectedItem).Tag.ToString());
        tile.VisualElements.BackgroundColor = background;
        tile.VisualElements.ForegroundText = ForegroundText.Light;
        tile.VisualElements.ShowNameOnSquare150x150Logo = true;
        tile.VisualElements.ShowNameOnSquare310x310Logo = true;
        tile.VisualElements.ShowNameOnWide310x150Logo = true;
        await tile.RequestCreateForSelectionAsync(getElementRect((FrameworkElement)selection));
        display.Items.Add(new Item { Id = tile.TileId, Content = value, Colour = new SolidColorBrush(background) });
    }

    public async void Remove(ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            string id = ((Item)display.SelectedItem).Id;

            if (SecondaryTile.Exists(id))
            {
                SecondaryTile tile = new SecondaryTile(id);
                await tile.RequestDeleteAsync();
            }
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }

    public async void List(ListBox display)
    {
        display.Items.Clear();
        IReadOnlyList<SecondaryTile> list = await SecondaryTile.FindAllAsync();
        foreach (SecondaryTile item in list)
        {
            display.Items.Add(new Item
            {
                Id = item.TileId,
                Content = item.DisplayName,
                Colour = new SolidColorBrush(item.VisualElements.BackgroundColor)
            });
        }
    }
}
