using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

public class Library
{
    private bool rotating = false;
    private Storyboard rotation = new Storyboard();

    public void Rotate(string axis, ref Image target)
    {
        if (rotating)
        {
            rotation.Stop();
            rotating = false;
        }
        else
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0.0;
            animation.To = 360.0;
            animation.BeginTime = TimeSpan.FromSeconds(1);
            animation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, "(UIElement.Projection).(PlaneProjection.Rotation" + axis + ")");
            rotation.Children.Clear();
            rotation.Children.Add(animation);
            rotation.Begin();
            rotating = true;
        }
    }
}
