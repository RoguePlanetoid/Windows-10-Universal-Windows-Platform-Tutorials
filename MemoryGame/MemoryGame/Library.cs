using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private int moves = 0;
    private int firstId = 0;
    private int secondId = 0;
    private Canvas first;
    private Canvas second;
    private int[,] board = new int[4, 4];
    private List<int> matches = new List<int>();
    private Random random = new Random((int)DateTime.Now.Ticks);

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private UIElement shape(ref PointCollection points, Color fill)
    {
        Polygon polygon = new Polygon();
        polygon.Stretch = Stretch.Uniform;
        polygon.StrokeLineJoin = PenLineJoin.Round;
        polygon.Points = points;
        polygon.Height = 40;
        polygon.Width = polygon.Height;
        polygon.Fill = new SolidColorBrush(fill);
        polygon.Stroke = new SolidColorBrush(fill);
        polygon.StrokeThickness = 5;
        polygon.Margin = new Thickness(10);
        return polygon;
    }

    private UIElement card(int type)
    {
        PointCollection points = new PointCollection();
        switch (type)
        {
            case 1: // Circle
                EllipseGeometry ellipse = new EllipseGeometry();
                Path circle = new Path();
                ellipse.Center = new Point(20, 20);
                ellipse.RadiusX = 20;
                ellipse.RadiusY = 20;
                circle.Data = ellipse;
                circle.Stroke = new SolidColorBrush(Colors.DarkRed);
                circle.Fill = new SolidColorBrush(Colors.DarkRed);
                circle.StrokeThickness = 5;
                circle.Margin = new Thickness(10);
                return circle;
            case 2: // Cross
                Path lines = new Path();
                LineGeometry line1 = new LineGeometry();
                LineGeometry line2 = new LineGeometry();
                GeometryGroup linegroup = new GeometryGroup();
                line1.StartPoint = new Point(0, 0);
                line1.EndPoint = new Point(40, 40);
                line2.StartPoint = new Point(40, 0);
                line2.EndPoint = new Point(0, 40);
                linegroup.Children.Add(line1);
                linegroup.Children.Add(line2);
                lines.Data = linegroup;
                lines.Stroke = new SolidColorBrush(Colors.DarkBlue);
                lines.StrokeThickness = 5;
                lines.Margin = new Thickness(10);
                return lines;
            case 3: // Triangle
                points.Add(new Point(150, 0));
                points.Add(new Point(0, 250));
                points.Add(new Point(300, 250));
                return shape(ref points, Colors.Green);
            case 4: // Square
                points.Add(new Point(0, 0));
                points.Add(new Point(0, 100));
                points.Add(new Point(100, 100));
                points.Add(new Point(100, 0));
                return shape(ref points, Colors.DarkMagenta);
            case 5: // Pentagon
                points.Add(new Point(0, 125));
                points.Add(new Point(150, 0));
                points.Add(new Point(300, 125));
                points.Add(new Point(250, 300));
                points.Add(new Point(50, 300));
                return shape(ref points, Colors.Crimson);
            case 6: // Hexagon
                points.Add(new Point(75, 0));
                points.Add(new Point(225, 0));
                points.Add(new Point(300, 150));
                points.Add(new Point(225, 300));
                points.Add(new Point(75, 300));
                points.Add(new Point(0, 150));
                return shape(ref points, Colors.DarkCyan);
            case 7: // Star
                points.Add(new Point(9, 2));
                points.Add(new Point(11, 7));
                points.Add(new Point(17, 7));
                points.Add(new Point(12, 10));
                points.Add(new Point(14, 15));
                points.Add(new Point(9, 12));
                points.Add(new Point(4, 15));
                points.Add(new Point(6, 10));
                points.Add(new Point(1, 7));
                points.Add(new Point(7, 7));
                return shape(ref points, Colors.Gold);
            case 8: // Rhombus
                points.Add(new Point(50, 0));
                points.Add(new Point(100, 50));
                points.Add(new Point(50, 100));
                points.Add(new Point(0, 50));
                return shape(ref points, Colors.OrangeRed);
            default:
                return null;
        }
    }

    private void add(ref Grid grid, int row, int column)
    {
        Canvas canvas = new Canvas();
        canvas.Height = 60;
        canvas.Width = 60;
        canvas.Background = Background;
        canvas.Tapped += async (object sender, TappedRoutedEventArgs e) =>
        {
            int selected;
            canvas = ((Canvas)(sender));
            row = (int)canvas.GetValue(Grid.RowProperty);
            column = (int)canvas.GetValue(Grid.ColumnProperty);
            selected = board[row, column];
            if ((matches.IndexOf(selected) < 0))
            {
                if ((firstId == 0)) // No Match
                {
                    first = canvas;
                    firstId = selected;
                    first.Children.Clear();
                    first.Children.Add(card(selected));
                }
                else if ((secondId == 0))
                {
                    second = canvas;
                    if (!first.Equals(second)) // Different
                    {
                        secondId = selected;
                        second.Children.Clear();
                        second.Children.Add(card(selected));
                        if ((firstId == secondId)) // Is Match
                        {
                            matches.Add(firstId);
                            matches.Add(secondId);
                            if (!(first == null))
                            {
                                first.Background = null;
                                first = null;
                            }
                            if (!(second == null))
                            {
                                second.Background = null;
                                second = null;
                            }
                            if ((matches.Count == 16))
                            {
                                Show("Well Done! You matched them all in " + moves + " moves!", "Memory Game");
                            }
                        }
                        else // No Match
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1.5));
                            if (!(first == null))
                            {
                                first.Children.Clear();
                                first = null;
                            }
                            if (!(second == null))
                            {
                                second.Children.Clear();
                                second = null;
                            }
                        }
                        moves++;
                        firstId = 0;
                        secondId = 0;
                    }
                }
            }
        };
        canvas.SetValue(Grid.ColumnProperty, column);
        canvas.SetValue(Grid.RowProperty, row);
        grid.Children.Add(canvas);
    }

    private void layout(ref Grid Grid)
    {
        moves = 0;
        matches.Clear();
        Grid.Children.Clear();
        Grid.ColumnDefinitions.Clear();
        Grid.RowDefinitions.Clear();
        // Setup 4x4 Grid
        for (int Index = 0; (Index <= 3); Index++)
        {
            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        add(ref Grid, 0, 0);
        add(ref Grid, 0, 1);
        add(ref Grid, 0, 2);
        add(ref Grid, 0, 3);
        add(ref Grid, 1, 0);
        add(ref Grid, 1, 1);
        add(ref Grid, 1, 2);
        add(ref Grid, 1, 3);
        add(ref Grid, 2, 0);
        add(ref Grid, 2, 1);
        add(ref Grid, 2, 2);
        add(ref Grid, 2, 3);
        add(ref Grid, 3, 0);
        add(ref Grid, 3, 1);
        add(ref Grid, 3, 2);
        add(ref Grid, 3, 3);
    }

    private List<int> select(int start, int finish, int total)
    {
        int number;
        List<int> numbers = new List<int>();
        while ((numbers.Count < total)) // Select Numbers
        {
            // Random Number between Start and Finish
            number = random.Next(start, finish + 1);
            if ((!numbers.Contains(number)) || (numbers.Count < 1))
            {
                numbers.Add(number); // Add if number Chosen or None
            }
        }
        return numbers;
    }

    public void New(Grid grid)
    {
        layout(ref grid);
        List<int> values = new List<int>();
        List<int> indices = new List<int>();
        int counter = 0;
        while (values.Count < 17)
        {
            List<int> numbers = select(1, 8, 8); // Random 1 - 8
            for (int number = 0; (number <= 7); number++)
            {
                values.Add(numbers[number]); // Add to Cards
            }
        }
        indices = select(1, 16, 16); // Random 1 - 16
        for (int Column = 0; (Column <= 3); Column++) // Board Columns
        {
            for (int Row = 0; (Row <= 3); Row++) // Board Rows
            {
                board[Column, Row] = values[indices[counter] - 1];
                counter++;
            }
        }
    }
}
