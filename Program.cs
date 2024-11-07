using AlienGame;
using System;
using System.IO;
using System.Media;
using Tao.Sdl;
using System.Collections.Generic;

namespace AlienGame
{
    
    class AlienGame
    {

        static Image bgmenu = Engine.LoadImage("assets/backgrounds/background_menu.png");
        static Image bg1 = Engine.LoadImage("assets/backgrounds/background_first_level.png");

        static Player player = initializePlayer();
        static Planet[] planets = initializePlanets();

        static GameStage gamestage;

        static Font menuFont;
        static Font smallInfoFont;

        static short menuOptions = 0;
        static short　dynamicTitle = 0;
        static short fontSize = 30; 

        static void Main(string[] args)
        {
            Engine.Initialize();
            gamestage = GameStage.Menu;
            try
            {
                menuFont = Engine.LoadFont("assets/fonts/Magnite.otf", 30);
                smallInfoFont = Engine.LoadFont("assets/fonts/Magnite.otf", 10);
            }
            catch (Exception e)
            { 
                System.Console.WriteLine(e.Message);
            }
            

          
            while (true)
            {
                CheckInputs();
                Update();
                Render();
                Sdl.SDL_Delay(20);  
            }
        }

        static void CheckInputs()
        {
            switch (gamestage)
            {
                case GameStage.Menu:
                    if (Engine.KeyPress(Engine.KEY_UP))
                    {
                        if (menuOptions > 0)
                        {
                            menuOptions -= 1;
                        } 
                    }

                    if (Engine.KeyPress(Engine.KEY_DOWN))
                    {
                        if (menuOptions < 5)
                        {
                            menuOptions += 1;
                        }
                    }

                    if (Engine.KeyPress(Engine.KEY_ENTER))
                    {
                        if (menuOptions == 0)
                        {
                            gamestage = GameStage.FirstLevel;
                        } else if (menuOptions >= 1 && menuOptions <= 4) 
                        { 
                            gamestage = GameStage.OptionsMenu;
                        } else
                        {
                            Environment.Exit(0);
                        }
                    }
                    break;
                case GameStage.WinScreen:
                    break;
                case GameStage.GameOverScreen:
                    break;
                default:
                    if (Engine.KeyPress(Engine.KEY_LEFT))
                    {
                        if (player.posX != 0)
                        {
                            player.posX -= player.speed;
                        }
                    }

                    if (Engine.KeyPress(Engine.KEY_RIGHT))
                    {
                        System.Console.WriteLine(player.posX);
                        if (player.posX < 1035)
                        {
                            player.posX += player.speed;
                        }
                    }

                    if (Engine.KeyPress(Engine.KEY_UP))
                    {
                        if (player.posY != 0)
                        {
                            player.posY -= player.speed;
                        }
                    }

                    if (Engine.KeyPress(Engine.KEY_DOWN))
                    {
                        if (player.posY < 680)
                        {
                            player.posY += player.speed;
                        }
                    }

                    if (Engine.KeyPress(Engine.KEY_SHIFT))
                    {
                        player.speed = 20;
                    }
                    else
                    {
                        player.speed = 10;
                    }

                    if (Engine.KeyPress(Engine.KEY_ESP))
                    {
                        if (player.ammo > 0) 
                        {
                            player.ammoList.Add(new Bullet(player.posX, player.posY, Engine.LoadImage("assets/objects/bullet_101.png")));
                            player.ammo--;
                        }
                    }
            
                    break;
            }
            
        }

        static void Update()
        {
            if (player.ammoList.Count > 0)
            {
                for (var i = 0; i < player.ammoList.Count; i++)
                {
                    if (player.ammoList[i].posX > 1080) 
                    {
                        player.ammoList.RemoveAt(i);
                    } 
                    else
                    {
                        var newPosX = player.ammoList[i];
                        newPosX.posX += 20;
                        player.ammoList[i] = newPosX;
                    }
                    
                }
            }
        }

        static void Render()
        {
            Engine.Clear();

            switch (gamestage) 
            {
                case GameStage.Menu:
                    Engine.Draw(bgmenu, 0, 0);
                    Engine.DrawText("Alien returns", 400, 150, 255, 0, 0, menuFont);
                    if (menuOptions == 0) 
                    {
                        Engine.DrawText("Start", 450, 250, 255, 255, 255, menuFont);
                    } else
                    {
                        Engine.DrawText("Start", 450, 250, 255, 0, 0, menuFont);
                    }

                    if (menuOptions >= 1 && menuOptions <= 4)
                    {
                        Engine.DrawText("Options", 450, 300, 255, 255, 255, menuFont);
                    }
                    else
                    {
                        Engine.DrawText("Options", 450, 300, 255, 0, 0, menuFont);
                    }

                    if (menuOptions == 5)
                    {
                        Engine.DrawText("Quit game", 450, 350, 255, 255, 255, menuFont);
                    }
                    else
                    {
                        Engine.DrawText("Quit game", 450, 350, 255, 0, 0, menuFont);
                    }
                    break;
                case GameStage.FirstLevel:
                    Engine.Draw(bg1, 0, 0);
                    Engine.Draw(player.image, player.posX, player.posY);
                    for (int i = 0; i < planets.Length; i++)
                    {
                        Planet planet = planets[i];
                        Engine.Draw(planet.image, planet.posX, planet.posY);
                    }
                    if (dynamicTitle < 50)
                    {
                        Engine.DrawText("Stage 1!", 500, 350, 255, 0, 0, menuFont);
                        dynamicTitle++;
                    }
                    if (dynamicTitle >= 50 && dynamicTitle <= 70)
                    {
                        Engine.DrawText("Go!", 500, 350, 255, 0, 0, menuFont);
                        dynamicTitle++; 
                    }
                    Engine.DrawText("Ammo: " + player.ammo, 1000, 700, 255, 255, 255, smallInfoFont);
                    if (player.ammoList.Count > 0)
                    {
                        foreach (var bullet in player.ammoList)
                        {
                            Engine.Draw(bullet.image, bullet.posX, bullet.posY);
                        }
                    }
                    break;
            }

            Engine.Show();
        }


        private static Player initializePlayer()
        {
            Player player = new Player();
            player.posX = 0;
            player.posY = 500;
            player.speed = 10;
            player.health = 100;
            player.ammoList = new List<Bullet>();
            player.ammo = 500;
            player.tripulation = 3;
            player.image = new Image("assets/ships/tiny_ship12.png");
            return player;
        }
        private static Planet[] initializePlanets()
        {
            string[] files = Directory.GetFiles("assets/planets");
            Planet[] planets = new Planet[new Random().Next(2,5)];
            for (int i = 0; i < planets.Length; i++) { 
                Planet planet = new Planet();
                Random rand = new Random(DateTime.Now.GetHashCode() + 1);
                planet.posX = rand.Next(200, 980);
                planet.posY = rand.Next(0, 620);
                Boolean checkPlanet = true;
                /*while (checkPlanet)
                {
                    int count = 0;
                    foreach (Planet p in planets)
                    {
                        if (p.posX == planet.posX)
                        {
                            p.posX = ra
                            break;
                        }
                        count++;
                    }
                    if (count == planets.Length)
                        checkPlanet = false;
                } */
                
                planet.health = 100;
                planet.aliens = rand.Next(0, 20);
                planet.image = new Image(files[rand.Next(0, files.Length)]);
                checkPlanet = true;
                while(checkPlanet) 
                {
                    int count = 0;
                    foreach (Planet p in planets) 
                    { 
                        if (p.image != null && p.image.Equals(planet.image))
                        {
                            planet.image = new Image(files[rand.Next(0, files.Length)]);
                            break;
                        }
                        count++;
                    }
                    if (count == planets.Length)
                        checkPlanet = false;
                }

                Goods goods = new Goods();
                goods.food = rand.Next(0, 3);
                goods.steel = rand.Next(0, 3);
                goods.ammo = rand.Next(0, 50);
                planets[i] = planet;
            }

            return planets;
        }
    }
}