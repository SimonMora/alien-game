using AlienGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utils
{
    internal class Helpers
    {

        public static Boolean CheckPlanetsEmpty(Planet[] planets)
        {
            var listEmptyPlanets = new List<Planet>();
            foreach (var planet in planets)
            {
                if (planet.image == null)
                {
                    listEmptyPlanets.Add(planet);
                }
            }

            return listEmptyPlanets.Count() == planets.Count();
        }

        public static Planet[] InitializePlanets(int planetsNumber)
        {
            string[] files = Directory.GetFiles("assets/planets");
            Planet[] planets = new Planet[planetsNumber];
            for (int i = 0; i < planets.Length; i++)
            {
                Planet planet = new Planet();
                Random rand = new Random(DateTime.Now.GetHashCode() + i);
                planet.posX = rand.Next(500, 980);
                planet.posY = rand.Next(0, 620);
                Boolean checkPlanet = true;
                while (checkPlanet)
                {
                    if (Helpers.CheckPlanetsEmpty(planets))
                    {
                        checkPlanet = false;
                    }
                    else
                    {
                        var olPlanet = planets[0];
                        if (olPlanet.posX != planet.posX && olPlanet.posY != planet.posY)
                        {
                            checkPlanet = false;
                        }
                        else
                        {
                            if (olPlanet.posX == planet.posX)
                            {
                                planet.posX = rand.Next(500, 980);
                            }

                            if (olPlanet.posY == planet.posY)
                            {
                                planet.posX = rand.Next(0, 620);
                            }
                        }
                    }
                }

                planet.health = 100;
                planet.aliensList = null;
                planet.image = new Image(files[rand.Next(0, files.Length)]);
                checkPlanet = true;
                while (checkPlanet)
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
                goods.life = rand.Next(0, 3);
                goods.ammo = rand.Next(0, 50);
                planets[i] = planet;
            }

            return planets;
        }
    }
}
