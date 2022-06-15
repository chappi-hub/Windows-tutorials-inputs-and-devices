//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

#define NOT_RUNNING_ON_SURFACE

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Begin "Step 2: Use InkCanvas to support basic inking"
//using directives for inking functionality.
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml.Shapes;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Diagnostics;
// End "Step 2: Use InkCanvas to support basic inking"

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GettingStarted_Ink
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string persistFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\page1.gif";
        

        public MainPage()
        {
            

            this.InitializeComponent();


#if NOT_RUNNING_ON_SURFACE
            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Touch |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;
#endif
            inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;

            // Begin "Step 3: Support inking with touch and mouse"
            // TODO: Eingabe soll default nur für stift gelten, evtl zusätzlich einstellbar auch für die anderen
            //inkCanvas.InkPresenter.InputDeviceTypes =
            //    Windows.UI.Core.CoreInputDeviceTypes.Mouse |
            //    Windows.UI.Core.CoreInputDeviceTypes.Touch |
            //    Windows.UI.Core.CoreInputDeviceTypes.Pen;
            // End "Step 3: Support inking with touch and mouse"

        }

        private async void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            await PersistCanvasContent(); 
        }

        private void ChangeCostumPen_Click(object sender, RoutedEventArgs e)
        {
            (sender as CostumPenButton).Activate();
        }

        private async void Export_Click(object sender, RoutedEventArgs e)
        {
            await PersistCanvasContent();

            //// Get all strokes on the InkCanvas.
            //IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            //// Strokes present on ink canvas.
            //if (currentStrokes.Count > 0)
            //{
            //    // Let users choose their ink file using a file picker.
            //    // Initialize the picker.
            //    Windows.Storage.Pickers.FileSavePicker savePicker =
            //        new Windows.Storage.Pickers.FileSavePicker();
            //    savePicker.SuggestedStartLocation =
            //        Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            //    savePicker.FileTypeChoices.Add(
            //        "GIF",
            //        new List<string>() { ".gif" });
            //    savePicker.DefaultFileExtension = ".gif";
            //    savePicker.SuggestedFileName = "InkSample";

            //    // Show the file picker.
            //    Windows.Storage.StorageFile file =
            //        await savePicker.PickSaveFileAsync();
            //    // When chosen, picker returns a reference to the selected file.
            //    if (file != null)
            //    {
            //        // Prevent updates to the file until updates are 
            //        // finalized with call to CompleteUpdatesAsync.
            //        Windows.Storage.CachedFileManager.DeferUpdates(file);
            //        // Open a file stream for writing.
            //        IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            //        // Write the ink strokes to the output stream.
            //        using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
            //        {
            //            await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
            //            await outputStream.FlushAsync();
            //        }
            //        stream.Dispose();

            //        // Finalize write so other apps can update file.
            //        Windows.Storage.Provider.FileUpdateStatus status =
            //            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

            //        if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            //        {
            //            // File saved.
            //        }
            //        else
            //        {
            //            // File couldn't be saved.
            //        }
            //    }
            //    // User selects Cancel and picker returns null.
            //    else
            //    {
            //        // Operation cancelled.
            //    }
            //}
        }

        private async Task PersistCanvasContent()
        {
            if (!File.Exists(persistFile))
            {
                File.Create(persistFile).Close();
            }

            StorageFile file = await StorageFile.GetFileFromPathAsync(persistFile);

            Windows.Storage.CachedFileManager.DeferUpdates(file);
            IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
            {
                await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
            stream.Dispose();

                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
        }

        private async Task LoadFormerCanvasContent()
        {
            if (!File.Exists(persistFile)) return;

            StorageFile file = await StorageFile.GetFileFromPathAsync(persistFile);

            Windows.Storage.CachedFileManager.DeferUpdates(file);

            IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            using (IInputStream inputStream = stream.GetInputStreamAt(0))
            {
                await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
            }
            stream.Dispose();

            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        private async void inkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFormerCanvasContent();

        }

        private async void inkCanvas_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(persistFile);

            Windows.Storage.CachedFileManager.DeferUpdates(file);
            // Open a file stream for writing.
            IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            // Write the ink strokes to the output stream.
            using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
            {
                await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
            stream.Dispose();

            // Finalize write so other apps can update file.
            Windows.Storage.Provider.FileUpdateStatus status =
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
        }

        private void inkCanvas_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        }
    }
}
