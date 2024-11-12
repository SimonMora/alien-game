using AlienGame;
using System;
using System.IO;
using System.Media;
using Tao.Sdl;
using System.Collections.Generic;
using System.Linq;
using model;
using utils;

namespace AlienGame
{
    
    class AlienGame
    {

        static Player player;

        static GameStage gamestage;

        static short fontSize = 30;

        private static GameOverScreen gameoverScreen;
        private static WinScreen winScreen;
        private static MenuScreen menuScreen;
        private static FirstLevelScreen firstLevelScreen;
        private static SecondLevelScreen secondLevelScreen;
        private static TutorialScreen tutorialScreen;


        static void Main(string[] args)
        {
            Engine.Initialize();
            Constants.InitializeFonts();
            gamestage = GameStage.Menu;
            player = InitializePlayer();

            tutorialScreen = new TutorialScreen();
            gameoverScreen = new GameOverScreen();
            menuScreen = new MenuScreen();

            gameoverScreen.StageChanged += gs_Change;
            menuScreen.StageChanged += gs_Change;
            tutorialScreen.StageChanged += gs_Change;

            while (true)
            {
                CheckInputs();
                Update();
                Render();
                Sdl.SDL_Delay(50);  
            }
        }

        static void CheckInputs()
        {
            switch (gamestage)
            {
                case GameStage.Menu:
                    menuScreen.CheckInputs();
                    break;
                case GameStage.WinScreen:
                    winScreen.CheckInputs();
                    break;
                case GameStage.GameOverScreen:
                    gameoverScreen.CheckInputs();
                    break;
                case GameStage.FirstLevel:
                    firstLevelScreen.CheckInputs();
                    break;
                case GameStage.SecondLevel:
                    secondLevelScreen.CheckInputs();
                    break;
                case GameStage.Tutorial:
                    tutorialScreen.CheckInputs();
                    break;
                default:
                    break;
            }
            
        }

        static void Update()
        {
            switch (gamestage)
            { 
                case GameStage.FirstLevel:
                    firstLevelScreen.Update();
                    break;
                case GameStage.SecondLevel:
                    secondLevelScreen.Update();
                    break;
                default:
                    break;
            }
            
        }

        static void Render()
        {
            Engine.Clear();

            switch (gamestage) 
            {
                case GameStage.Menu:
                    menuScreen.Render();
                    break;
                case GameStage.GameOverScreen:
                    gameoverScreen.Render();
                    break;
                case GameStage.WinScreen:
                    winScreen.Render();
                    break;
                case GameStage.FirstLevel:
                    firstLevelScreen.Render();
                    break;
                case GameStage.SecondLevel:
                    secondLevelScreen.Render();
                    break;
                case GameStage.Tutorial:
                    tutorialScreen.Render();
                    break;
                default:
                    break;
            }

            Engine.Show();
        }

        private static Player InitializePlayer()
        {
            Player player = new Player();
            player.posX = 0;
            player.posY = 500;
            player.speed = Constants.PLAYER_SPEED;
            player.health = 3;
            player.ammoList = new List<Bullet>();
            player.ammo = 300;
            player.image = new Image("assets/ships/tiny_ship12.png");
            return player;
        }

        private static void gs_Change(object sender, GameStageEventArgs args)
        {
            if (args.GameStage == GameStage.GameOverScreen)
            {
                player = InitializePlayer();
                firstLevelScreen = new FirstLevelScreen(player);
                firstLevelScreen.StageChanged += gs_Change;
            }
            if (args.GameStage == GameStage.WinScreen)
            {
                if (sender.ToString().Equals("model.FirstLevelScreen"))
                {
                    winScreen = new WinScreen("partial");
                    winScreen.StageChanged += gs_Change;

                    player = args.Player;
                    player.posY = 500;
                    player.ammoList.Clear();
                } 
                else
                {
                    winScreen = new WinScreen("final");
                    winScreen.StageChanged += gs_Change;

                    player = InitializePlayer();
                }
            }
            if (args.GameStage == GameStage.FirstLevel) 
            {
                Constants.soundPlayer.Stop();
                firstLevelScreen = new FirstLevelScreen(player);
                firstLevelScreen.StageChanged += gs_Change;
            }
            if (args.GameStage == GameStage.SecondLevel)
            {
                secondLevelScreen = new SecondLevelScreen(player);
                secondLevelScreen.StageChanged += gs_Change;
            }
            if (args.GameStage == GameStage.Menu && !sender.ToString().Equals("model.TutorialScreen"))
            {
                menuScreen = new MenuScreen();
                menuScreen.StageChanged += gs_Change;
            }
            gamestage = args.GameStage;
        }
    }
}

class GameStageEventArgs : EventArgs
{
    public GameStage GameStage { get; set; }
    public Player Player { get; set; }

    public GameStageEventArgs(GameStage gameStage)
    {
        GameStage = gameStage;
    }

    public GameStageEventArgs(GameStage gameStage, Player player)
    {
        GameStage = gameStage;
        Player = player;
    }
}