using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace model
{
    internal class WinScreen
    {
        private string state;
        private static Font titleFont = Constants.TITLE_FONT;
        private static Font smallInfoFont = Constants.SMALL_FONT;
        private static Image ellenImage = Engine.LoadImage("assets/objects/ellen-ripley copy.png"); 

        public event EventHandler<GameStageEventArgs> StageChanged;

        public WinScreen(string state)
        {
            this.state = state;
        }

        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_ENTER) || Engine.KeyPress(Engine.KEY_ESC))
            {
                if (state.Equals("partial"))
                {
                    StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.SecondLevel));
                }
                else if (state.Equals("final"))
                {
                    StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.Menu));
                }
                
            }
        }

        public void Update()
        {

        }

        public void Render()
        {
            if (state == "partial")
            {
                Engine.Draw(Constants.MENU_BACKGROUND, 0, 0);
                Engine.DrawText("Stage Clear", 300, 300, 255, 0, 0, titleFont);
                Engine.DrawText("Press enter to continue..", 380, 450, 255, 0, 0, smallInfoFont);
            } 
            else if (state == "final")
            {
                Engine.Draw(Constants.MENU_BACKGROUND, 0, 0);
                Engine.Draw(ellenImage, 300, 0);
                Engine.DrawText("Final Stage Clear", 200, 300, 255, 0, 0, titleFont);
                Engine.DrawText("Press enter to return to menu..", 380, 450, 255, 0, 0, smallInfoFont);
            }
            
        }
    }
}
