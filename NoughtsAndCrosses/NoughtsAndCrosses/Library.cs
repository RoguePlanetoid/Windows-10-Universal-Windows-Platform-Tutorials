using System;
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
    private const string nought = "O";
    private const string cross = "X";

    private bool won = false;
    private string piece = "";
    private string[,] board = new string[3, 3];

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    private bool winner()
    {
        return
        (board[0, 0] == piece && board[0, 1] == piece && board[0, 2] == piece) ||
        (board[1, 0] == piece && board[1, 1] == piece && board[1, 2] == piece) ||
        (board[2, 0] == piece && board[2, 1] == piece && board[2, 2] == piece) ||
        (board[0, 0] == piece && board[1, 0] == piece && board[2, 0] == piece) ||
        (board[0, 1] == piece && board[1, 1] == piece && board[2, 1] == piece) ||
        (board[0, 2] == piece && board[1, 2] == piece && board[2, 2] == piece) ||
        (board[0, 0] == piece && board[1, 1] == piece && board[2, 2] == piece) ||
        (board[0, 2] == piece && board[1, 1] == piece && board[2, 0] == piece);
    }

    private bool drawn()
    {
        return
        board[0, 0] != string.Empty && board[0, 1] != string.Empty && board[0, 2] != string.Empty &&
        board[1, 0] != string.Empty && board[1, 1] != string.Empty && board[1, 2] != string.Empty &&
        board[2, 0] != string.Empty && board[2, 1] != string.Empty && board[2, 2] != string.Empty;
    }

    private Path getPiece()
    {
        if ((piece == cross)) // Draw X
        {
            Path lines = new Path();
            LineGeometry line1 = new LineGeometry();
            LineGeometry line2 = new LineGeometry();
            GeometryGroup linegroup = new GeometryGroup();
            line1.StartPoint = new Point(0, 0);
            line1.EndPoint = new Point(60, 60);
            line2.StartPoint = new Point(60, 0);
            line2.EndPoint = new Point(0, 60);
            linegroup.Children.Add(line1);
            linegroup.Children.Add(line2);
            lines.Data = linegroup;
            lines.Stroke = new SolidColorBrush(Colors.Red);
            lines.StrokeThickness = 5;
            lines.Margin = new Thickness(10);
            return lines;
        }
        else // Draw O
        {
            EllipseGeometry ellipse = new EllipseGeometry();
            Path circle = new Path();
            ellipse.Center = new Point(30, 30);
            ellipse.RadiusX = 30;
            ellipse.RadiusY = 30;
            circle.Data = ellipse;
            circle.Stroke = new SolidColorBrush(Colors.Blue);
            circle.StrokeThickness = 5;
            circle.Margin = new Thickness(10);
            return circle;
        }
    }

    private void add(ref Grid grid, int row, int column)
    {
        Canvas canvas = new Canvas();
        canvas.Height = 80;
        canvas.Width = 80;
        canvas.Background = Background;
        canvas.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (!won)
            {
                canvas = ((Canvas)(sender));
                if ((canvas.Children.Count < 1))
                {
                    canvas.Children.Add(getPiece());
                    board[(int)canvas.GetValue(Grid.RowProperty),
                    (int)canvas.GetValue(Grid.ColumnProperty)] = piece;
                }
                if (winner())
                {
                    won = true;
                    Show((piece + " wins!"), "Noughts and Crosses");
                }
                else if (drawn())
                {
                    Show("Draw!", "Noughts and Crosses");
                }
                else
                {
                    piece = ((piece == cross) ? nought : cross); // Swap Players
                }
            }
            else
            {
                Show("Game Over!", "Noughts and Crosses");
            }
        };
        canvas.SetValue(Grid.ColumnProperty, column);
        canvas.SetValue(Grid.RowProperty, row);
        grid.Children.Add(canvas);
    }

    private void layout(ref Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        // Setup 3x3 Grid
        for (int Index = 0; (Index <= 2); Index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        add(ref grid, 0, 0); // Top Left
        add(ref grid, 0, 1); // Top Middle
        add(ref grid, 0, 2); // Top Right
        add(ref grid, 1, 0); // Middle Left
        add(ref grid, 1, 1); // Centre
        add(ref grid, 1, 2); // Middle Right
        add(ref grid, 2, 0); // Bottom Left
        add(ref grid, 2, 1); // Bottom Middle
        add(ref grid, 2, 2); // Bottom Right
    }

    public async void New(Grid grid)
    {
        layout(ref grid);
        board[0, 0] = "";
        board[0, 1] = "";
        board[0, 2] = "";
        board[1, 0] = "";
        board[1, 1] = "";
        board[1, 2] = "";
        board[2, 0] = "";
        board[2, 1] = "";
        board[2, 2] = "";
        won = false;
        if (await Confirm("Who goes First?", "Noughts and Crosses", nought, cross))
        {
            piece = nought;
        }
        else
        {
            piece = cross;
        }
    }
}
