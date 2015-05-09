using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private Random random = new Random();

    private void add(Grid grid, int row, int column)
    {
        Ellipse dot = new Ellipse();
        dot.Width = 20;
        dot.Height = 20;
        dot.Fill = new SolidColorBrush(Colors.White);
        dot.SetValue(Grid.ColumnProperty, column);
        dot.SetValue(Grid.RowProperty, row);
        grid.Children.Add(dot);
    }

    public Grid Dice(int Value)
    {
        Grid grid = new Grid();
        grid.Height = 100;
        grid.Width = 100;
        for (int index = 0; (index <= 2); index++) // 3 by 3 Grid 
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        switch (Value)
        {
            case 0:
                // No Dots
                break;
            case 1:
                add(grid, 1, 1); // Middle
                break;
            case 2:
                add(grid, 0, 2); // Top Right
                add(grid, 2, 0); // Bottom Left
                break;
            case 3:
                add(grid, 0, 2); // Top Right
                add(grid, 1, 1); // Middle
                add(grid, 2, 0); // Bottom Left
                break;
            case 4:
                add(grid, 0, 0); // Top Left
                add(grid, 0, 2); // Top Right
                add(grid, 2, 0); // Bottom Left
                add(grid, 2, 2); // Bottom Right
                break;
            case 5:
                add(grid, 0, 0); // Top Left
                add(grid, 0, 2); // Top Right
                add(grid, 1, 1); // Middle
                add(grid, 2, 0); // Bottom Left
                add(grid, 2, 2); // Bottom Right
                break;
            case 6:
                add(grid, 0, 0); // Top Left
                add(grid, 0, 2); // Top Right
                add(grid, 1, 0); // Middle Left
                add(grid, 1, 2); // Middle Right
                add(grid, 2, 0); // Bottom Left
                add(grid, 2, 2); // Bottom Right
                break;
        }
        return grid;
    }

    public int Roll()
    {
        return random.Next(1, 7);
    }
}
