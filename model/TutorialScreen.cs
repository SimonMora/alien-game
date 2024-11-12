using AlienGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utils;

namespace model
{
    internal class TutorialScreen
    {
        private Font titleFont = Constants.TITLE_FONT;
        private Font smallFont = Constants.SMALL_FONT;
        private Image spaceKey = Engine.LoadImage("assets/keys/space-key-50.png");
        private Image shiftKey = Engine.LoadImage("assets/keys/shift-50.png");
        private Image arrowsKey = Engine.LoadImage("assets/keys/arrow-keys-100.png");
        private Image bgImage = Engine.LoadImage("assets/backgrounds/tutorial_bg.jpg");
        private Image planetImage = Engine.LoadImage("assets/planets/Ice 1.png");
        private Image alienImage = Engine.LoadImage("assets/objects/alien_image_example.png");

        public event EventHandler<GameStageEventArgs> StageChanged;

        public static void Update()
        { }
        public void CheckInputs()
        {
            if (Engine.KeyPress(Engine.KEY_ESC))
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.Menu));
            }
        }

        public void Render()
        {
            Engine.Draw(bgImage, 0, 0);
            Engine.DrawText("Tutorial", 350, 80, 255, 255, 255, titleFont);
            Engine.Draw(arrowsKey, 200, 200);
            Engine.DrawText("Move with arrows", 170, 340, 255, 255, 255, smallFont);
            Engine.Draw(shiftKey, 480, 220);
            Engine.DrawText("Speed with shift", 430, 340, 255, 255, 255, smallFont);
            Engine.Draw(spaceKey, 700, 220);
            Engine.DrawText("Shoot with space bar", 680, 340, 255, 255, 255, smallFont);
            Engine.Draw(planetImage, 300, 450);
            Engine.DrawText("Shoot to the planets", 250, 570, 255, 255, 255, smallFont);
            Engine.Draw(alienImage, 700, 450);
            Engine.DrawText("Shoot or avoid aliens", 650, 570, 255, 255, 255, smallFont);
        }
    }
}
