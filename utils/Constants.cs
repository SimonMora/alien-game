using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace utils
{
    internal class Constants
    {
        //Dimension related constants
        public static int SCREEN_WIDTH = 1080;
        public static int SCREEN_HEIGHT = 720;

        public static int PLAYER_WIDTH = 45;
        public static int PLAYER_HEIGHT = 30;

        public static int PLANET_DIMENSION = 100;

        //Speed related constants
        public static int PLAYER_SPEED = 20;
        public static int PLAYER_SPEED_MAX = 40;
        public static int BULLET_SPEED = 30;
        public static int ALIEN_SPEED_X = 35;
        public static int ALIEN_SPEED_Y = 10;

        //Fonts
        public static Font TITLE_FONT;
        public static Font MENU_FONT;
        public static Font SMALL_FONT;

        //Images
        public static Image COLLISION_IMAGE = Engine.LoadImage("assets/objects/collision_image.png");
        public static Image MENU_BACKGROUND = Engine.LoadImage("assets/backgrounds/background_menu.png");

        public static void InitializeFonts() 
        {
            TITLE_FONT = Engine.LoadFont("assets/fonts/Magnite.otf", 50);
            MENU_FONT = Engine.LoadFont("assets/fonts/Magnite.otf", 30);
            SMALL_FONT = Engine.LoadFont("assets/fonts/Magnite.otf", 15);
        }

        //Sounds
        public static SoundPlayer soundPlayer = new SoundPlayer("assets/audio/Grey-Sector-v0_85.wav");
        public static SoundPlayer soundPlayer2 = new SoundPlayer("assets/audio/MyVeryOwnDeadShip.wav");
    }
}
