using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;

namespace kyaramachi
{
    class Character : Blob
    {
        private Bitmap src;
        private BitmapSource sprite;
        public BitmapSource Sprite { get { return sprite; } }
        private int[] loc = new int[2];
        public int[] Loc { get { return loc; } }
        public int Res
        {
            get { return this.res; }
            set
            {
                this.res = value;
                sprite = setRes(res, sprite);
            }
        }

        public Character(Bitmap src, int pos, int[] loc)
        {
            this.loc = loc;
            this.src = new Bitmap(
                192,
                32,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            using (Graphics gfx = Graphics.FromImage(this.src))
            {
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.DrawImage(
                    src,
                    new Rectangle(0, 0, this.src.Width, this.src.Height),
                    new Rectangle(1, 1, 192, 32),
                    GraphicsUnit.Pixel
                );
            }
            sprite = setFrame(1);
        }

        private BitmapSource setFrame(int frame)
        {
            Bitmap result = new Bitmap(
                16 * res,
                32 * res,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            using (Graphics gfx = Graphics.FromImage(result))
            {
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.DrawImage(
                    src,
                    new Rectangle(0, 0, result.Width, result.Height),
                    new Rectangle(16 * (frame - 1), 0, 16, 32),
                    GraphicsUnit.Pixel
                );
            }
            return CreateBitmapSourceFromGdiBitmap(result);
        }

        int i = 2;
        int prev = 2;
        int last = 0;
        public void Move(int dir, bool stop = false)
        {
            if(last != dir)
            {
                i = 2;
                prev = 2;
            }
            last = dir;

            dir *= 3;

            //sprite = stop? setFrame(dir + i) : setFrame(dir + 1);
            if(stop)
            {
                i = 1;
            }
            sprite = setFrame(dir + i);

            if (i == 1)
            {
                if (prev == 2) i = 3;
                if (prev == 3) i = 2;
                prev = i;
            }
            else if (i != 1)
            {
                i = 1;
            }
        }
    }
}
