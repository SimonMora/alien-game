using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.model
{
    internal class WinScreen
    {
        private static Image bgImage = Engine.LoadImage("assets/backgrounds/background_menu.png");
        private static Font titleFont = Engine.LoadFont("assets/fonts/Magnite.otf", 50);
        private static Font smallInfoFont = Engine.LoadFont("assets/fonts/Magnite.otf", 15);
        public event EventHandler<GameStageEventArgs> StageChanged;

        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_ENTER) || Engine.KeyPress(Engine.KEY_ESC))
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.SecondLevel));
            }
        }

        public static void Update()
        {

        }

        public static void Render()
        {
            Engine.Draw(bgImage, 0, 0);
            Engine.DrawText("Stage Clear", 300, 300, 255, 0, 0, titleFont);
            Engine.DrawText("Press enter to continue..", 350, 500, 255, 0, 0, smallInfoFont);
        }
    }
}
