﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSLUploader.Classes
{
    public class Screenshot
    {
        public static FileInfo CaptureDesktop()
        {
            List<Bitmap> bitmaps = new List<Bitmap>();

            foreach(var screen in Screen.AllScreens.OrderBy(x => x.Bounds.X).ToList())
            {
                bitmaps.Add(GetBitmapFromScreen(screen));
            }

            Bitmap image = JoinBitmaps(bitmaps);
            return SaveBitmap(image);
        }
        

        private static Bitmap GetBitmapFromScreen(Screen screen)
        {
            Bitmap bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
            using(Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size);
            }
            return bmp;
        }

        private static Bitmap JoinBitmaps(List<Bitmap> bitmaps)
        {
            Bitmap output = null;

            for(int i=0; i<bitmaps.Count; i++)
            {
                if(i == 0)
                {
                    output = bitmaps[i];
                }
                else
                {
                    output = Join2Bitmap(output, bitmaps[i]);
                }
            }

            return output;
        }

        private static Bitmap Join2Bitmap(Bitmap first, Bitmap second)
        {
            int outputImageWidth = first.Width + second.Width;
            int outputImageHeight = first.Height > second.Height ? first.Height : second.Height;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using(Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(first, new Rectangle(new Point(), first.Size), new Rectangle(new Point(), first.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(second, new Rectangle(new Point(first.Width,0), second.Size), new Rectangle(new Point(), second.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }

        private static FileInfo SaveBitmap(Bitmap bitmap)
        {
            FileInfo f = new FileInfo(Path.Combine(Path.GetTempPath(), "kslu_temp_" + DateTime.Now.Ticks + ".png"));
            bitmap.Save(f.FullName);
            return f;
        }
    }
}