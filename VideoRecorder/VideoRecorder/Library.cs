using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

public class Library
{
    private const string videoFilename = "video.mp4";
    private string filename;

    private MediaCapture capture;
    private InMemoryRandomAccessStream buffer;

    public static bool Recording;

    private async Task<bool> init()
    {
        if (buffer != null)
        {
            buffer.Dispose();
        }
        buffer = new InMemoryRandomAccessStream();
        if (capture != null)
        {
            capture.Dispose();
        }
        try
        {
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings 
            { 
                StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo 
            };
            capture = new MediaCapture();
            await capture.InitializeAsync();
            capture.RecordLimitationExceeded += (MediaCapture sender) =>
            {
                Stop();
                throw new Exception("Exceeded Record Limitation");
            };
            capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
            {
                Recording = false;
                throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
            };
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
            {
                throw ex.InnerException;
            }
            throw;
        }
        return true;
    }

    public async void Record(CaptureElement preview)
    {
        await init();
        preview.Source = capture;
        await capture.StartPreviewAsync();
        await capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), buffer);
        if (Recording) throw new InvalidOperationException("cannot excute two records at the same time");
        Recording = true;
    }

    public async void Stop()
    {
        await capture.StopRecordAsync();
        Recording = false;
    }

    public async Task Play(CoreDispatcher dispatcher, MediaElement playback)
    {
        IRandomAccessStream video = buffer.CloneStream();
        if (video == null) throw new ArgumentNullException("buffer");
        StorageFolder storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        if (!string.IsNullOrEmpty(filename))
        {
            StorageFile original = await storageFolder.GetFileAsync(filename);
            await original.DeleteAsync();
        }
        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(videoFilename, CreationCollisionOption.GenerateUniqueName);
            filename = storageFile.Name;
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RandomAccessStream.CopyAndCloseAsync(video.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await video.FlushAsync();
                video.Dispose();
            }
            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
            playback.SetSource(stream, storageFile.FileType);
            playback.Play();
        });
    }
}
