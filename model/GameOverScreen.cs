using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.model
{
    internal class GameOverScreen
    {
        private static Image bgImage = Engine.LoadImage("assets/backgrounds/background_menu.png");
        private static Image gameOverImg = Engine.LoadImage("assets/objects/game_over.png");
        private static Font titleFont = Engine.LoadFont("assets/fonts/Magnite.otf", 50);
        public event EventHandler<GameStageEventArgs> StageChanged;

        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_ENTER) || Engine.KeyPress(Engine.KEY_ESC))
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.Menu));
            }
        }

        public static void Update()
        {

        }
        
        public static void Render()
        {
            Engine.Draw(bgImage, 0, 0);
            Engine.DrawText("Game Over..", 350, 280, 255, 0, 0, titleFont);
            Engine.Draw(gameOverImg, 0, 0);
        }
    }
}
