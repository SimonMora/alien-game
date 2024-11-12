using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace model
{
    internal class GameOverScreen
    {
        private static Image gameOverImg = Engine.LoadImage("assets/objects/game_over.png");
        private static Font titleFont = Constants.TITLE_FONT;
     
        public event EventHandler<GameStageEventArgs> StageChanged;

        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_ENTER) || Engine.KeyPress(Engine.KEY_ESC))
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.Menu));
            }
        }

        public void Update()
        {

        }
        
        public void Render()
        {
            Engine.Draw(Constants.MENU_BACKGROUND, 0, 0);
            Engine.DrawText("Game Over..", 400, 280, 255, 0, 0, titleFont);
            Engine.Draw(gameOverImg, 0, 0);
        }
    }
}
