using Microsoft.Graphics.Canvas;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3MediaPlayerElementSnapshotSample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker();
            savePicker.FileTypeChoices.Add("PNG", [".png"]);

            // ‚±‚ÌƒLƒ‚‚¢‚¨‚Ü‚¶‚È‚¢–³‚­‚È‚é‚Æ‚¢‚¢‚È(https://qiita.com/hayashida-katsutoshi/items/21b8f99c462c01bb7c6d)
            InitializeWithWindow.Initialize(savePicker, WindowNative.GetWindowHandle(this));

            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                int naturalVideoWidth = (int)Player.MediaPlayer.PlaybackSession.NaturalVideoWidth;
                int naturalVideoHeight = (int)Player.MediaPlayer.PlaybackSession.NaturalVideoHeight;

                CanvasDevice canvasDevice = CanvasDevice.GetSharedDevice();

                var frameServerDest = new SoftwareBitmap(BitmapPixelFormat.Rgba8, naturalVideoWidth, naturalVideoHeight, BitmapAlphaMode.Ignore);
                using var inputBitmap = CanvasBitmap.CreateFromSoftwareBitmap(canvasDevice, frameServerDest);
                Player.MediaPlayer.CopyFrameToVideoSurface(inputBitmap);

                var photoPath = file.Path;
                await inputBitmap.SaveAsync(photoPath, CanvasBitmapFileFormat.Png);
            }
        }
    }
}
