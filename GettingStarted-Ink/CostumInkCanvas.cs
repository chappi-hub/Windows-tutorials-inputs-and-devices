using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;

namespace GettingStarted_Ink
{
    internal class CostumInkCanvas : InkCanvas, IDisposable
    {
        static int GlobalPageNumber;
        int PageNumber;
        string persistFile;
        public CostumInkCanvas()
        {
            PageNumber = GlobalPageNumber++;
            persistFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + $@"\{PageNumber}.gif";

            this.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            this.Loaded += CostumInkCanvas_Loaded;
        }

        private async void CostumInkCanvas_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadFormerCanvasContent();
        }

        public async void Dispose()
        {
            GlobalPageNumber--;
            StorageFile file = await StorageFile.GetFileFromPathAsync(persistFile);
            await file.DeleteAsync();
        }

        private async void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            await PersistCanvasContent();
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
                await this.InkPresenter.StrokeContainer.SaveAsync(outputStream);
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
                await this.InkPresenter.StrokeContainer.LoadAsync(inputStream);
            }
            stream.Dispose();

            await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
        }
    }
}
