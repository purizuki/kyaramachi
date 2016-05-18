using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace kyaramachi
{
    class Blob
    {
        protected int res = 1;

        protected BitmapSource setRes(int res, BitmapSource bsimg)
        {
            Bitmap img = GetBitmap(bsimg);
            Bitmap nimg = new Bitmap(img.Width * res, img.Height * res, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gfx = Graphics.FromImage(nimg))
            {
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.DrawImage(
                    img,
                    new Rectangle(0, 0, nimg.Width, nimg.Height),
                    new Rectangle(0, 0, img.Width, img.Height),
                    GraphicsUnit.Pixel
                );
            }
            return CreateBitmapSourceFromGdiBitmap(nimg);
        }

        protected static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              System.Windows.Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
