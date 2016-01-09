using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Common.Tool
{
    public class BlurTool
    {
        int[] pixs;
        CommonCom.ImageOperation BlurCom;
        public BlurTool()
        {
            BlurCom = new CommonCom.ImageOperation();
        }

        public bool Blur(byte[] data, int width, int height, int radius)
        {
            // bgra
            // argb
            int[] imgs = new int[data.Length / 4];  
            for (int i = 0; i < imgs.Length; i++)
            {
                uint temp = BitConverter.ToUInt32(new byte[] { data[i * 4 + 3], data[i * 4 + 2], data[i * 4 + 1], data[i * 4] }, 0);
                imgs[i] = (int)temp;
            }

            int[] imgout = BlurCom.BlurCompute(imgs, width, height, radius);

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
            BlurCom.DeletePixs();
            return true;
        }

        public bool Blur(byte[] data, int imgw, int imgh, int x, int y, int w, int h, int radius)
        {
            // bgra
            // argb
            int[] imgs = new int[data.Length / 4];  
            for (int i = 0; i < imgs.Length; i++)
            {
                uint temp = BitConverter.ToUInt32(new byte[] { data[i * 4 + 3], data[i * 4 + 2], data[i * 4 + 1], data[i * 4] }, 0);
                imgs[i] = (int)temp;
            }

            int[] imgout = BlurCom.BlurCompute(imgs, imgw, imgh, x, y, w, h, radius);

            data = ArgbToBgra(imgout);
            BlurCom.DeletePixs();
            return true;
        }

        public bool PutPixs(byte[] data, uint width, uint height)
        {
            pixs = BgraToArgb(data);
            int res = BlurCom.PutPixs(pixs, (int)width, (int)height);
            return true;
        }

        public int[] Blur(int radius)
        {
            int[] res = BlurCom.BlurPixs(radius);
            return res;
        }

        public void DeletePixs()
        {
            BlurCom.DeletePixs();
            
        }

        public static int[] BgraToArgb(byte[] data)
        {
            // bgra
            // argb
            int[] imgs = new int[data.Length / 4];
            for (int i = 0; i < imgs.Length; i++)
            {
                uint temp = BitConverter.ToUInt32(new byte[] { data[i * 4 + 3], data[i * 4 + 2], data[i * 4 + 1], data[i * 4] }, 0);
                imgs[i] = (int)temp;
            }
            return imgs;
        }

        public static byte[] ArgbToBgra(int[] pixs)
        {
            byte[] dataout = new byte[pixs.Length * 4];
            byte[] tempPix = new byte[4];
            for (int i = 0; i < pixs.Length; i++)
            {
                // bgra
                // argb
                tempPix = BitConverter.GetBytes(pixs[i]);
                dataout[i * 4] = tempPix[3];
                dataout[i * 4 + 1] = tempPix[2];
                dataout[i * 4 + 2] = tempPix[1];
                dataout[i * 4 + 3] = tempPix[0];
            }
            return dataout;
        }
    }
}
