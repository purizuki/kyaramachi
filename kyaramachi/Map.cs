using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;

namespace kyaramachi
{
    class Map : Blob
    {
        //private int res = 1;
        public int Res {
            get { return this.res; }
            set
            {
                this.res = value;
                bg = setRes(res, bg);
            }
        }

        public System.Windows.Media.Color dark;
        private Bitmap map;
        private BitmapSource bg;
        public BitmapSource Bg { get { return bg; } }
        public Bitmap Src { get { return map; } }

        public Map(Bitmap src)
        {
            Color px = src.GetPixel(194, 1);
            dark = System.Windows.Media.Color.FromArgb(px.A, px.R, px.G, px.B);
            this.map = new Bitmap(
                (src.Width - 3 - 192),
                (src.Height - 2),
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            using (Graphics gfx = Graphics.FromImage(map))
            {
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.DrawImage(
                    src,
                    new Rectangle(0, 0, map.Width, map.Height),
                    new Rectangle(192 + 2, 2, (src.Width - 3 - 192), src.Height - 3),
                    GraphicsUnit.Pixel
                );
            }
            bg = CreateBitmapSourceFromGdiBitmap(map);
        }
    }
}
