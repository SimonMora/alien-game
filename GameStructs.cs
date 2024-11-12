using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienGame { 

    public struct Player
    {
        public int posX;
        public int posY;
        public Image image;
        public int ammo;
        public List<Bullet> ammoList;
        public int speed;
        public int health;
    }

    public struct Position 
    { 
        public int posX; 
        public int posY; 
    
        public Position(int x, int y)
        {
            this.posX = x;
            this.posY = y; 
        }
    }

    public struct Planet
    {
        public int posX;
        public int posY;
        public Image image;
        public Goods goods;
        public List<Alien> aliensList;
        public int health;
    }

    public struct Goods
    {
        public int ammo;
        public int life;
        public int food;
    }

    public struct Bullet
    {
        public int posX;
        public int posY;
        public Image image;

        public Bullet(int posX, int posY) 
        { 
            this.posX = posX;
            this.posY = posY;
            this.image = Engine.LoadImage("assets/objects/bullet_101.png");
        }
    }

    public struct Alien 
    {
        public int posX;
        public int posY;
        public Image image;

        public Alien(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
            this.image = Engine.LoadImage("assets/objects/alien_image.png");
        }
    }
}
