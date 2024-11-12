using AlienGame;
using utils; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace model
{
    internal class SecondLevelScreen
    {
        private Planet[] planets;
        private Player player;
        private List<Position> collisionList = new List<Position>();

        private Image bg1 = Engine.LoadImage("assets/backgrounds/background_second_level.png");
        private Image collisionImage = Engine.LoadImage("assets/objects/collision_image.png");

        private Font titleFont = Engine.LoadFont("assets/fonts/Magnite.otf", 50);
        private Font menuFont = Engine.LoadFont("assets/fonts/Magnite.otf", 30);
        private Font smallInfoFont = Engine.LoadFont("assets/fonts/Magnite.otf", 15);

        private int dynamicTitle = 0;
        private int updateCount = 0;
        private int NUMBER_OF_PLANETS = 4;

        public event EventHandler<GameStageEventArgs> StageChanged;

        public SecondLevelScreen() { }

        public SecondLevelScreen(Player player)
        {
            this.player = player;
            this.planets = Helpers.InitializePlanets(NUMBER_OF_PLANETS);
        }

        public void CheckInputs()
        {
            if (dynamicTitle > 50)
            {
                if (Engine.KeyPress(Engine.KEY_LEFT))
                {
                    if (player.posX != 0)
                    {
                        player.posX -= player.speed;
                    }
                }

                if (Engine.KeyPress(Engine.KEY_RIGHT))
                {
                    if (player.posX < Constants.SCREEN_WIDTH - Constants.PLAYER_WIDTH)
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
                    if (player.posY < Constants.SCREEN_HEIGHT - Constants.PLAYER_WIDTH)
                    {
                        player.posY += player.speed;
                    }
                }

                if (Engine.KeyPress(Engine.KEY_SHIFT))
                {
                    player.speed = Constants.PLAYER_SPEED_MAX;
                }
                else
                {
                    player.speed = Constants.PLAYER_SPEED;
                }

                if (Engine.KeyPress(Engine.KEY_ESP))
                {
                    if (player.ammo > 0)
                    {
                        player.ammoList.Add(new Bullet(player.posX, player.posY + (Constants.PLAYER_HEIGHT / 2)));
                        player.ammo--;
                    }
                }
            }
        }

        public void Update()
        {

            if (Helpers.CheckPlanetsEmpty(this.planets))
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.WinScreen));
            }

            if (player.health == 0)
            {
                StageChanged?.Invoke(this, new GameStageEventArgs(GameStage.GameOverScreen));
            }

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

                            IEnumerable<int> planetRangeY = Enumerable.Range(planet.posY, Constants.PLANET_DIMENSION);
                            if (planetRangeY.Contains(bullet.posY))
                            {
                                IEnumerable<int> planetRangeX = Enumerable.Range(planet.posX, Constants.PLANET_DIMENSION);
                                if (planetRangeX.Contains(bullet.posX))
                                {
                                    collisionList.Add(new Position(bullet.posX, bullet.posY));
                                    if (planet.aliensList == null)
                                    {
                                        planet.aliensList = new List<Alien> { new Alien(planet.posX + 50, planet.posY + 50) };
                                    }
                                    planet.health -= 5;

                                    try
                                    {
                                        player.ammoList.RemoveAt(i);
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

                            if (planet.aliensList != null && planet.aliensList.Count > 0)
                            {
                                var updatedList = new List<Alien>();
                                foreach (var alien in planet.aliensList)
                                {
                                    IEnumerable<int> alienRangeY = Enumerable.Range(alien.posY, 56);
                                    if (alienRangeY.Contains(bullet.posY))
                                    {
                                        IEnumerable<int> alienRangeX = Enumerable.Range(alien.posX - 56, 112);
                                        if (alienRangeX.Contains(bullet.posX))
                                        {
                                            collisionList.Add(new Position(bullet.posX, bullet.posY));
                                        }
                                        else
                                        {
                                            updatedList.Add(alien);
                                        }
                                    }
                                    else
                                    {
                                        updatedList.Add(alien);
                                    }
                                }
                                planet.aliensList = updatedList;
                            }

                            if (planet.health == 0)
                            {
                                planets[j] = new Planet();
                            }
                            else
                            {
                                planets[j] = planet;
                            }
                        }
                    }

                }
            }
            foreach (var planet in planets)
            {
                if (updateCount % 25 == 0 && planet.aliensList != null)
                {
                    planet.aliensList.Add(new Alien(planet.posX, planet.posY));
                }

                if (planet.aliensList != null && planet.aliensList.Count > 0)
                {
                    var newAlienList = new List<Alien>();
                    foreach (var alien in planet.aliensList)
                    {
                        var uAlien = alien;
                        uAlien.posX -= Constants.ALIEN_SPEED_X;
                        if (Math.Abs(uAlien.posY - player.posY) > 10)
                        {
                            if (uAlien.posY > player.posY)
                            {
                                uAlien.posY -= Constants.ALIEN_SPEED_Y;
                            }
                            else
                            {
                                uAlien.posY += Constants.ALIEN_SPEED_Y;
                            }
                        }

                        if (uAlien.posX > 0)
                        {
                            newAlienList.Add(uAlien);
                        }
                    }
                    planet.aliensList.Clear();
                    planet.aliensList.AddRange(newAlienList);

                    foreach (var alien in planet.aliensList)
                    {
                        IEnumerable<int> playerRangeY = Enumerable.Range(player.posY - 10, 57);
                        if (playerRangeY.Contains(alien.posY))
                        {
                            IEnumerable<int> playerRangeX = Enumerable.Range(player.posX, Constants.PLAYER_WIDTH);
                            if (playerRangeX.Contains(alien.posX))
                            {
                                player.health--;
                                collisionList.Add(new Position(player.posX, player.posY));
                                newAlienList.Remove(alien);
                            }
                        }
                    }
                    planet.aliensList.Clear();
                    planet.aliensList.AddRange(newAlienList);
                }
            }
        }

        public void Render()
        {
            Engine.Draw(bg1, 0, 0);
            for (int i = 0; i < planets.Length; i++)
            {
                Planet planet = planets[i];
                if (planet.image != null)
                {
                    Engine.Draw(planet.image, planet.posX, planet.posY);
                    if (planet.aliensList != null && planet.aliensList.Count > 0)
                    {
                        foreach (var alien in planet.aliensList)
                        {
                            Engine.Draw(alien.image, alien.posX, alien.posY);
                        }
                    }
                }
            }
            Engine.Draw(player.image, player.posX, player.posY);
            if (dynamicTitle < 30)
            {
                Engine.DrawText("Stage 2!", 400, 350, 255, 0, 0, titleFont);
                dynamicTitle++;
            }
            if (dynamicTitle >= 30 && dynamicTitle <= 50)
            {
                Engine.DrawText("Go!", 500, 350, 255, 0, 0, titleFont);
                dynamicTitle++;
            }
            Engine.DrawText("health: " + player.health, 0, 10, 255, 255, 255, smallInfoFont);
            Engine.DrawText("ammo: " + player.ammo, 0, 30, 255, 255, 255, smallInfoFont);

            if (player.ammoList.Count > 0)
            {
                foreach (var bullet in player.ammoList)
                {
                    Engine.Draw(bullet.image, bullet.posX, bullet.posY);
                }
            }
            if (collisionList.Count > 0)
            {
                foreach (var collision in collisionList)
                {
                    Engine.Draw(collisionImage, collision.posX, collision.posY);
                }

                collisionList.Clear();
            }
        }
    }
}
