using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.TransferStudentTracker.Utilities
{
    public class HashCalculations
    {
        public static ulong GetImageHash(Bitmap bitmap)
        {
            // Copy the image and convert to grayscale
            Bitmap sourcebm = new Bitmap(bitmap);
            Image<Gray, float> sourceimage = new Image<Gray, float>(sourcebm);

            // Apply a convolution filter
            CvInvoke.Blur(sourceimage, sourceimage, new Size(4, 4), new Point(-1, -1));

            // Resize to 64x64 pixels
            Image<Gray, float> resimage = new Image<Gray, float>(new Size(64, 64));
            CvInvoke.Resize(sourceimage, resimage, new Size(64, 64));

            // DCT
            IntPtr compleximage = CvInvoke.cvCreateImage(resimage.Size, Emgu.CV.CvEnum.IplDepth.IplDepth32F, 1);
            CvInvoke.Dct(resimage, resimage, Emgu.CV.CvEnum.DctType.Forward);

            Image<Gray, float> dctimage = Image<Gray, float>.FromIplImagePtr(resimage);

            // Calculate the mean
            double mean = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    mean += dctimage[y, x].Intensity;
                }
            }
            mean -= dctimage[0, 0].Intensity;
            mean /= 63;

            // Calculate the hash
            ulong hash = 0;
            ulong index = 1;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Gray color = dctimage[y, x];
                    if (color.Intensity > mean)
                    {
                        hash |= index;
                    }

                    index <<= 1;
                }
            }

            return hash;
        }

        public static int GetHashDistance(ulong hash1, ulong hash2)
        {
            ulong index = 1;
            int distance = 0;
            for (int i = 0; i < 64; i++)
            {
                if ((hash1 & index) != (hash2 & index))
                {
                    distance++;
                }

                index <<= 1;
            }

            return distance;
        }
    }
}
