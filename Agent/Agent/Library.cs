using System;
using Windows.ApplicationModel.Background;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

public class Library
{
    private IBackgroundTaskRegistration registration;

    private bool started
    {
        get
        {
            return BackgroundTaskRegistration.AllTasks.Count > 0;
        }
    }

    public bool Init()
    {
        if (started)
        {
            registration = BackgroundTaskRegistration.AllTasks.Values.First();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Save(string value)
    {
        ApplicationData.Current.LocalSettings.Values["value"] = value;
    }

    public async Task<bool> Toggle()
    {
        if (started)
        {
            registration.Unregister(true);
            registration = null;
            return false;
        }
        else
        {
            try
            {
                await BackgroundExecutionManager.RequestAccessAsync();
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = typeof(Agent.Background.Trigger).FullName;
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                builder.TaskEntryPoint = builder.Name;
                builder.Register();
                registration = BackgroundTaskRegistration.AllTasks.Values.First();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
