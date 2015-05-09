using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Library
{
    private string colourToString(Color value)
    {
        return String.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
            value.A, value.R, value.G, value.B);
    }

    private Color stringToColour(string value)
    {
        return Color.FromArgb(
        Byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
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

    public void Init(ref InkCanvas display, ref ComboBox size, ref ComboBox colour)
    {
        string selectedSize = ((ComboBoxItem)size.SelectedItem).Tag.ToString();
        string selectedColour = ((ComboBoxItem)colour.SelectedItem).Tag.ToString();
        InkDrawingAttributes attributes = new InkDrawingAttributes();
        attributes.Color = stringToColour(selectedColour);
        attributes.Size = new Size(int.Parse(selectedSize), int.Parse(selectedSize));
        attributes.IgnorePressure = false;
        attributes.FitToCurve = true;
        display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
        display.InkPresenter.InputDeviceTypes = 
           CoreInputDeviceTypes.Mouse | 
           CoreInputDeviceTypes.Pen | 
           CoreInputDeviceTypes.Touch;
    }

    public void Colour(ref InkCanvas display, ref ComboBox colour)
    {
        if (display != null)
        {
            string selectedColour = ((ComboBoxItem)colour.SelectedItem).Tag.ToString();
            InkDrawingAttributes attributes = display.InkPresenter.CopyDefaultDrawingAttributes();
            attributes.Color = stringToColour(selectedColour);
            display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
            
        }
    }

    public void Size(ref InkCanvas display, ref ComboBox size)
    {
        if (display != null)
        {
            string selectedSize = ((ComboBoxItem)size.SelectedItem).Tag.ToString();
            InkDrawingAttributes attributes = display.InkPresenter.CopyDefaultDrawingAttributes();
            attributes.Size = new Size(int.Parse(selectedSize), int.Parse(selectedSize));
            display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
        }
    }

    public async void New(InkCanvas display)
    {
        if (await Confirm("Create New Drawing?", "Draw Editor", "Yes", "No"))
        {
            display.InkPresenter.StrokeContainer.Clear();
        }
    }

    public async void Open(InkCanvas display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".drw");
            StorageFile file = await picker.PickSingleFileAsync();
            using (IInputStream stream = await file.OpenSequentialReadAsync())
            {
                await display.InkPresenter.StrokeContainer.LoadAsync(stream);
            }
        }
        catch
        {

        }
    }

    public async void Save(InkCanvas display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Drawing", new List<string>() { ".drw" });
            picker.DefaultFileExtension = ".drw";
            picker.SuggestedFileName = "Drawing";
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await display.InkPresenter.StrokeContainer.SaveAsync(stream);
                }
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
        catch
        {

        }
    }
}
