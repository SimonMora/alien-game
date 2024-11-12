using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace model
{
    internal class MenuScreen
    {
        private int menuOptions = 0;
        private Font titleFont = Constants.TITLE_FONT;
        private Font menuFont = Constants.MENU_FONT;

        public event EventHandler<GameStageEventArgs> StageChanged;

        public MenuScreen() 
        { 
            Constants.soundPlayer2.Stop();
            Constants.soundPlayer2.Play();
        }

        public static void Update()
        { }
        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_UP))
            {
                if (menuOptions > 0)
                {
                    menuOptions -= 1;
                }
            }

            if (Engine.KeyPress(Engine.KEY_DOWN))
            {
                if (menuOptions < 3)
                {
                    menuOptions += 1;
                }
            }

            if (Engine.KeyPress(Engine.KEY_ENTER))
            {
                if (menuOptions == 0)
                {
                    StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.FirstLevel));
                }
                else if (menuOptions >= 1 && menuOptions <= 2)
                {
                    StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.Tutorial));
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        public void Render()
        {
            Engine.Draw(Constants.MENU_BACKGROUND, 0, 0);
            Engine.DrawText("Alien returns", 300, 200, 255, 0, 0, titleFont);
            if (menuOptions == 0)
            {
                Engine.DrawText("Start", 450, 300, 255, 255, 255, menuFont);
            }
            else
            {
                Engine.DrawText("Start", 450, 300, 255, 0, 0, menuFont);
            }

            if (menuOptions >= 1 && menuOptions <= 2)
            {
                Engine.DrawText("Tutorial", 450, 350, 255, 255, 255, menuFont);
            }
            else
            {
                Engine.DrawText("Tutorial", 450, 350, 255, 0, 0, menuFont);
            }

            if (menuOptions == 3)
            {
                Engine.DrawText("Quit game", 450, 400, 255, 255, 255, menuFont);
            }
            else
            {
                Engine.DrawText("Quit game", 450, 400, 255, 0, 0, menuFont);
            }
        }
    }
}
