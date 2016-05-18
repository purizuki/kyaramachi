using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace kyaramachi
{
    class Control
    {
        private Key? conLock = null;
        private Key? charLock = null;
        private Game game;

        public Control(Game game)
        {
            this.game = game;
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == conLock)
            {
                conLock = null;
                Debug.WriteLine("unlocked {0}", e.Key);
            }
        }
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (conLock == e.Key) return;
            conLock = e.Key;
            Debug.WriteLine("locked {0}", e.Key);

            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right || e.Key == Key.Left)
                Walk(e.Key);
        }

        async private void Walk(Key key)
        {
            charLock = key;
            Debug.WriteLine("called Walk for {0}", key);
            int dir = 0;
            switch (key)
            {
                case Key.Down:
                    dir = 0;
                    break;
                case Key.Right:
                    dir = 1;
                    break;
                case Key.Up:
                    dir = 2;
                    break;
                case Key.Left:
                    dir = 3;
                    break;
            }

            game.Character.Move(dir);
            game.Commit();
            await Task.Delay(167);
            while (conLock == key && charLock == key) {
                game.Character.Move(dir);
                await Task.Run(() => game.Move(key));
            }
            if (charLock == key)
            {
                game.Character.Move(dir, true);
                game.Commit();
            }
        }
    }
}
