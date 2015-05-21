using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Background
{
    public sealed class Trigger : Windows.ApplicationModel.Background.IBackgroundTask
    {
        public void Run(Windows.ApplicationModel.Background.IBackgroundTaskInstance taskInstance)
        {
            try
            {
                string value = Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("value") ?
                    (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["value"] :
                    string.Empty;
                StringBuilder template = new StringBuilder();
                template.Append("<toast><visual version='2'><binding template='ToastText02'>");
                template.Append("<text id='2'>TimeZoneChanged</text>");
                template.AppendFormat("<text id='1'>{0}</text>", value);
                template.Append("</binding></visual></toast>");
                Windows.Data.Xml.Dom.XmlDocument xml = new Windows.Data.Xml.Dom.XmlDocument();
                xml.LoadXml(template.ToString());
                Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(
                    new Windows.UI.Notifications.ToastNotification(xml));
            }
            catch
            {

            }
        }
    }
}
