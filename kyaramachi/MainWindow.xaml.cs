using System.Windows;

namespace kyaramachi
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Game game = new Game("map.png", win, imgBg, imgChara);
            KeyUp += game.Control.KeyUp;
            KeyDown += game.Control.KeyDown;

            game.setRes(4);

        }
    }
}
