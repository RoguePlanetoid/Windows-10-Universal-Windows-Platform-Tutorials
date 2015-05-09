using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

public class Library
{
    private const int length = 800;
    private const int speed = 600;
    private const int light = 300;

    private int turn = 0;
    private int count = 0;
    private bool play = false;
    private DispatcherTimer timer = new DispatcherTimer();
    private List<int> items = new List<int>();
    private Random random = new Random((int)DateTime.Now.Ticks);

    public Brush Background { get; set; }

    public void Show(string content, string title)
    {
        try
        {
            IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
        }
        catch
        {

        }
    }

    private void highlight(Grid grid, int index)
    {
        Canvas canvas = (Canvas)grid.FindName("Pad" + index.ToString());
        Brush brush = canvas.Background; // Original Background
        canvas.Background = Background; // New Background
        DispatcherTimer lightup = new DispatcherTimer();
        lightup.Interval = TimeSpan.FromMilliseconds(light);
        lightup.Tick += (object sender, object e) =>
        {
            canvas.Background = brush; // Original Background
            lightup.Stop();
        };
        lightup.Start();
    }

    private List<int> select(int start, int finish, int total)
    {
        int number;
        List<int> numbers = new List<int>();
        while ((numbers.Count < total)) // Select Numbers
        {
            // Random non-unique Number between Start and Finish
            number = random.Next(start, finish + 1);
            numbers.Add(number); // Add Number
        }
        return numbers;
    }

    public void New(Grid grid)
    {
        items = select(0, 3, length);
        play = false;
        turn = 0;
        count = 0;
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(speed);
        timer.Tick += (object sender, object e) =>
        {
            if (count <= turn)
            {
                highlight(grid, items[count]);
                count++;
            }
            if (count > turn)
            {
                timer.Stop();
                play = true;
                count = 0;
            }
        };
        timer.Start();
    }

    public void Tapped(Grid grid, Canvas canvas)
    {
        if (play == true)
        {
            int value = int.Parse(canvas.Name.Replace("Pad", string.Empty));
            highlight(grid, value);
            if (value == items[count])
            {
                if (count < turn)
                {
                    count++;
                }
                else
                {
                    play = false;
                    turn++;
                    count = 0;
                    timer.Start();
                }
            }
            else
            {
                timer.Stop();
                Show("Game Over! You scored " + turn + "!", "Touch Game");
                play = false;
                turn = 0;
                count = 0;
            }
        }
    }
}
