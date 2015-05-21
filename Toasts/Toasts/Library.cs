using System;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Linq;
using Windows.UI.Xaml.Controls;

public class Item
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string Time { get; set; }
}

public class Library
{
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Add(ref ListBox display, string value, TimeSpan occurs)
    {
        DateTime when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            occurs.Hours, occurs.Minutes, occurs.Seconds);
        if (when > DateTime.Now)
        {
            StringBuilder template = new StringBuilder();
            template.Append("<toast><visual version='2'><binding template='ToastText02'>");
            template.AppendFormat("<text id='2'>{0}</text>", value);
            template.AppendFormat("<text id='1'>{0}</text>", when.ToLocalTime());
            template.Append("</binding></visual></toast>");
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(template.ToString());
            ScheduledToastNotification toast = new ScheduledToastNotification(xml, when);
            toast.Id = random.Next(1, 100000000).ToString();
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            display.Items.Add(new Item { Id = toast.Id, Content = value, Time = when.ToString() });
        }
    }

    public void Remove(ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.RemoveFromSchedule(notifier.GetScheduledToastNotifications().Where(
                p => p.Id.Equals(((Item)display.SelectedItem).Id)).SingleOrDefault());
            display.Items.RemoveAt(display.SelectedIndex);
        }
    }
}
