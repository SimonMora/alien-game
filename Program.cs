using AlienGame;
using System;
using System.IO;
using System.Media;
using Tao.Sdl;
using System.Collections.Generic;
using System.Linq;

namespace AlienGame
{
    
    class AlienGame
    {

        static Image bgmenu = Engine.LoadImage("assets/backgrounds/background_menu.png");
        static Image bg1 = Engine.LoadImage("assets/backgrounds/background_first_level.png");

        static Player player = initializePlayer();
        static Planet[] planets = initializePlanets();

        static GameStage gamestage;

        static Font titleFont;
        static Font menuFont;
        static Font smallInfoFont;

        static short menuOptions = 0;
        static short　dynamicTitle = 0;
        static short fontSize = 30;

        static List<Position> collitionList = new List<Position>();
        static Image collitionImage = Engine.LoadImage("assets/objects/collition_image.png");
        private static int updateCount;

        static void Main(string[] args)
        {
            Engine.Initialize();
            gamestage = GameStage.Menu;
            try
            {
                titleFont = Engine.LoadFont("assets/fonts/Magnite.otf", 50);
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
                Sdl.SDL_Delay(50);  
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
                        player.speed = 40;
                    }
                    else
                    {
                        player.speed = 20;
                    }

                    if (Engine.KeyPress(Engine.KEY_ESP))
                    {
                        if (player.ammo > 0) 
                        {
                            player.ammoList.Add(new Bullet(player.posX, player.posY + 18));
                            player.ammo--;
                        }
                    }
            
                    break;
            }
            
        }

        static void Update()
        {
            updateCount++;
            if (player.ammoList.Count > 0)
            {
                for (var i = 0; i < player.ammoList.Count; i++)
                {
                    var bullet = player.ammoList[i];
                    if (bullet.posX > 1080) 
                    {
                        player.ammoList.RemoveAt(i);
                    } 
                    else
                    {
                        var newPosX = bullet;
                        newPosX.posX += 30;
                        player.ammoList[i] = newPosX;
                        for (var j = 0; j < planets.Length; j++)
                        {
                            var planet = planets[j];

                            IEnumerable<int> planetRangeY = Enumerable.Range(planet.posY, 100);
                            if (planetRangeY.Contains(bullet.posY))
                            {
                                IEnumerable<int> planetRangeX = Enumerable.Range(planet.posX, 100);
                                if (planetRangeX.Contains(bullet.posX)) 
                                {
                                    collitionList.Add(new Position(bullet.posX, bullet.posY));
                                    if (planet.aliensList != null & planet.aliensList.Count == 0)
                                    {
                                        planet.aliensList = new List<Alien> { new Alien(planet.posX + 50, planet.posY + 50) };
                                    } 
                                    planet.health -= 5;
                                    planets[j] = planet;
                                    try
                                    {
                                        player.ammoList.RemoveAt(i);
                                        if (planet.health == 0) 
                                        {
                                            planets[j] = new Planet();
                                        }
                                    }
                                    catch (ArgumentOutOfRangeException e) 
                                    {
                                        player.ammoList = new List<Bullet>();
                                    }
                                    catch (Exception e) 
                                    {
                                        System.Console.WriteLine(e);
                                    }
                                }
                            }

                            var updatedList = planet.aliensList;
                            foreach (var alien in planet.aliensList)
                            {
                                IEnumerable<int> alienRangeY = Enumerable.Range(alien.posY, 56);
                                if (alienRangeY.Contains(bullet.posY))
                                {
                                    if (alien.posX == bullet.posX)
                                    {
                                        collitionList.Add(new Position(bullet.posX, bullet.posY));
                                        updatedList.Remove(alien);
                                    }
                                }
                            }
                            planet.aliensList = updatedList;
                        }
                    }   
                    
                }
            }
            foreach (var planet in planets) 
            {
                if (planet.aliensList != null && planet.aliensList.Count > 0)
                {
                    var newAlienList = new List<Alien>();
                    foreach (var alien in planet.aliensList)
                    {
                        var uAlien = alien;
                        uAlien.posX -= 35;
                        newAlienList.Add(uAlien);
                    }
                    planet.aliensList.Clear();
                    planet.aliensList.AddRange(newAlienList);
                    if (updateCount % 25 == 0)
                    {
                        planet.aliensList.Add(new Alien(planet.posX + 50, planet.posY + 50));
                    } 
                }

                foreach (var alien in planet.aliensList)
                {
                    IEnumerable<int> playerRangeY = Enumerable.Range(player.posY - 37, 37);
                    if (playerRangeY.Contains(alien.posY + 25))
                    {
                        IEnumerable<int> playerRangeX = Enumerable.Range(player.posX, 44);
                        if (playerRangeX.Contains(alien.posX))
                        {
                            player.tripulation--;
                            collitionList.Add(new Position(player.posX, player.posY));
                            if (player.tripulation == 0)
                            {
                                gamestage = GameStage.GameOverScreen;
                            }
                        }
                    }
                }
            }
        }

        private static bool isEven(int v)
        {
            return v % 2 == 0;
        }

        static void Render()
        {
            Engine.Clear();

            switch (gamestage) 
            {
                case GameStage.Menu:
                    Engine.Draw(bgmenu, 0, 0);
                    Engine.DrawText("Alien returns", 300, 150, 255, 0, 0, titleFont);
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
                case GameStage.GameOverScreen:
                    Engine.Draw(bgmenu, 0, 0);
                    Engine.DrawText("Game Over!!", 350, 300, 255, 0, 0, titleFont);
                    break;
                case GameStage.FirstLevel:
                    Engine.Draw(bg1, 0, 0);
                    Engine.Draw(player.image, player.posX, player.posY);
                    for (int i = 0; i < planets.Length; i++)
                    {
                        Planet planet = planets[i];
                        if (planet.image != null) 
                        {
                            Engine.Draw(planet.image, planet.posX, planet.posY);
                            if (planet.aliensList.Count > 0)
                            {
                                foreach (var alien in planet.aliensList)
                                {
                                    Engine.Draw(alien.image, alien.posX, alien.posY);
                                }
                            }
                        }

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
                    Engine.DrawText("Tripulation: " + player.tripulation, 950, 690, 255, 255, 255, smallInfoFont);
                    Engine.DrawText("Ammo: " + player.ammo, 1000, 700, 255, 255, 255, smallInfoFont);
                    
                    if (player.ammoList.Count > 0)
                    {
                        foreach (var bullet in player.ammoList)
                        {
                            Engine.Draw(bullet.image, bullet.posX, bullet.posY);
                        }
                    }
                    if (collitionList.Count > 0)
                    {
                        foreach (var collition in collitionList) 
                        {
                            Engine.Draw(collitionImage, collition.posX, collition.posY);
                        }

                        collitionList.Clear();
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
            player.speed = 20;
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
                planet.aliensList = new List<Alien>();
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