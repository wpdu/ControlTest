using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ControlTest.Sensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Camera : Page
    {
        ObservableCollection<ImageModel> Images { get; set; }
        List<ImageModel> ImagesToShow;

        public Camera()
        {
            this.InitializeComponent();
            Images = new ObservableCollection<ImageModel>();
            ImagesToShow = new List<ImageModel>();
            lv.ItemsSource = Images;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPicture();
        }

        private async void AddPicture()
        {
            ImageModel imgModel = await Capture();
            if (imgModel != null)
                Images.Add(imgModel);
        }

        private void Img_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                ImageModel imgmodel = img.DataContext as ImageModel;
                if (imgmodel != null)
                {
                    ImagesToShow = new List<ImageModel>();
                    ImagesToShow.Add(imgmodel);
                    Frame.Navigate(typeof(BaseControl.FlipViewTest), ImagesToShow);

                    //bool result = await Windows.System.Launcher.LaunchFileAsync(imgmodel.ImgFile);
                    //Images.Remove(imgmodel);
                }
            }
        }
        
        private static async Task<ImageModel> Capture()
        {
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            // Get the desired camera by panel
            DeviceInformation desiredDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
            if (desiredDevice ==  null)
            {
                Debug.Write("no Camrea");
                return null;
            }
            CameraCaptureUI cameraUI = new CameraCaptureUI();
            cameraUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            cameraUI.PhotoSettings.AllowCropping = false;
            //cameraUI.PhotoSettings.CroppedSizeInPixels = new Size(1280,720);

            StorageFile photo = await cameraUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo == null)
                return null;

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            decoder = await BitmapDecoder.CreateAsync( await decoder.GetThumbnailAsync());

            SoftwareBitmap bitmapBgr8 = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource imgsource = new SoftwareBitmapSource();
            await imgsource.SetBitmapAsync(bitmapBgr8);
            ImageModel imgModel = new ImageModel() { ImgFile = photo, ThumbnailImage = imgsource };
            
            return imgModel;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ImagesToShow = Images.ToList();
            Frame.Navigate(typeof(BaseControl.FlipViewTest), ImagesToShow);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ImagesToShow != null && ImagesToShow.Count != Images.Count)
            {
                Images.Clear();
                ImagesToShow.ForEach(c => Images.Add(c));
            }
            base.OnNavigatedTo(e);
        }
    }

    public class ImageModel : BaseModel
    {

        private SoftwareBitmapSource _thumbnailImage = null;
        /// <summary>
        /// 图像缩略图
        /// </summary>
        public SoftwareBitmapSource ThumbnailImage
        {
            get
            {
                return _thumbnailImage;
            }
            set
            {
                Set(ref _thumbnailImage, value);
            }
        }


        private BitmapImage _image = null;
        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                Set(ref _image, value);
            }
        }

        private StorageFile _imgFile = null;
        public StorageFile ImgFile
        {
            get
            {
                return _imgFile;
            }
            set
            {
                Set(ref _imgFile, value);
            }
        }
    }
 }
