using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ControlTest.Tool
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImageTool : Page
    {
        public ImageTool()
        {
            this.InitializeComponent();
        }

        private async void btnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            // 实例化 RenderTargetBitmap 并对指定的 UIElement 截图，同时指定截图的宽和高
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(imgSrc, (int)imgSrc.ActualWidth, (int)imgSrc.ActualHeight);
            
            // 由于 RenderTargetBitmap 继承自 ImageSource，所以可以直接显示（WriteableBitmap 也继承了 ImageSource）
            image.Source = renderTargetBitmap;


            // 获取截图的像素数据
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();


            byte[] data = pixelBuffer.ToArray();    // bgra
            int[] imgs = new int[data.Length / 4];  // argb
            for (int i = 0; i < imgs.Length; i++)
            {
                uint temp = BitConverter.ToUInt32(new byte[]{ data[i * 4 + 3], data[i * 4 + 2], data[i * 4 + 1], data[i * 4]  }, 0);
                imgs[i] = (int)temp;
                //switch (i%4)
                //{
                //    case 0://Blue
                //        //data[i] = 100;
                        
                //        break;
                //    case 1://Green
                //        data[i] = 255;
                //        break;
                //    case 2://Red
                //        //data[i] = 100;
                //        break;
                //    case 3://Alpha
                //        //data[i] = 225;
                //        break;

                //    default:
                //        break;
                //}
            }
            
            CommonCom.ImageOperation io = new CommonCom.ImageOperation();
            int[] imgout = io.BlurCompute(imgs, renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight, 10);

            byte[] dataout = new byte[imgout.Length * 4];
            byte[] tempPix = new byte[4];
            for (int i = 0; i < imgout.Length; i++)
            {
                // bgra
                // argb
                tempPix = BitConverter.GetBytes(imgout[i]);
                dataout[i * 4] = tempPix[3];
                dataout[i * 4 + 1] = tempPix[2];
                dataout[i * 4 + 2] = tempPix[1];
                dataout[i * 4 + 3] = tempPix[0];
            }

            // 实例化一个 FileSavePicker 用于保存图片
            var savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".png";
            savePicker.FileTypeChoices.Add(".png", new List<string> { ".png" });
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.SuggestedFileName = "snapshot.png";
            var saveFile = await savePicker.PickSaveFileAsync();
            if (saveFile == null)
                return;

            // 对二进制图像数据做 png 编码处理（关于更多的图像处理的示例请参见：http://www.cnblogs.com/webabcd/archive/2013/05/27/3101069.html）
            using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);

                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    dataout);

                // 保存 png 图片
                await encoder.FlushAsync();
            }
        }

    }
}
