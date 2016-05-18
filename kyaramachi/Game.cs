using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace kyaramachi
{
    /****TO DO*********

        [x] Game
        [x] Map
        [x] Character
        [x] Blob
        [ ] Controls

    *******************/

    class Game
    {
        private Map map;
        private Character chara;
        private Grid win;
        private Image bg;
        private Image player;
        public Map Map {
            get { return map; }
        }
        public Character Character
        {
            get { return chara; }
        }
        private Control control;
        public Control Control { get { return control; } }

        public Game(string path, Grid win, Image bg, Image chara)
        {
            control = new Control(this);
            System.Drawing.Bitmap src = new System.Drawing.Bitmap("map.png");
            this.player = chara;
            this.win = win;
            this.bg = bg;

            this.map = new Map(src);
            win.Background = new System.Windows.Media.SolidColorBrush(map.dark);
            this.bg.Source = map.Bg;

            int[] start = spawn(Map.Src, new int[] { 0, 0 }, Map.Src.Width, Map.Src.Height);
            this.chara = new Character(src, 1, start);
            player.Source = this.chara.Sprite;

            System.Windows.Thickness m = this.bg.Margin;
            m.Left -= this.chara.Loc[0];
            m.Top -= this.chara.Loc[1];
            this.bg.Margin = m;
            //setRes(1);
        }

        public void setRes(int res)
        {
            win.Width *= res;
            win.Height *= res;

            map.Res = res;
            bg.Source = map.Bg;

            chara.Res = res;
            player.Source = chara.Sprite;

            System.Windows.Thickness m = bg.Margin;
            m.Left *= res;
            m.Top *= res;
            bg.Margin = m;
        }

        public bool check(System.Drawing.Bitmap map, int[] pos)
        {
            /****
              Implement
            ****/                                       // v Don't leave this v
            bool res = (map.GetPixel(pos[0], pos[1]) != System.Drawing.Color.Black);

            return res;
        }
        public int[] spawn(System.Drawing.Bitmap map, int[] start, int width, int height)
        {
            Random r = new Random();
            int[] spawn = null;
            while (spawn == null)
            {
                int[] coords = new int[2];
                coords[0] = r.Next(start[0], width / 16)* 16;
                coords[1] = r.Next(start[1], height / 16) * 16;
                if (check(map, coords)) spawn = coords;
            }
            return spawn;
        }

        public void Move(Key key)
        {
            Commit();
            for (int j = 0; j < 16; j++)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Thickness nM = bg.Margin;
                    switch (key)
                    {
                        case Key.Down:
                            nM.Top -= Map.Res;
                            break;
                        case Key.Right:
                            nM.Left -= Map.Res;
                            break;
                        case Key.Up:
                            nM.Top += Map.Res;
                            break;
                        case Key.Left:
                            nM.Left += Map.Res;
                            break;
                    }
                    bg.Margin = nM;
                    Debug.WriteLine(nM);
                    //Debug.WriteLine("[{0},{1}]", pos[0], pos[1]);
                }));
                //Task.Delay(20);
                Thread.Sleep(11);
            }
        }

        public void Commit()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                player.Source = chara.Sprite;
            }));
        }
    }
}
