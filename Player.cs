using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienGame
{
    public struct Player
    {
        public int posX;
        public int posY;
        public Image image;
        public int ammo;
        public List<Bullet> ammoList;
        public int tripulation;
        public int speed;
        public int health;
    }

    public struct Planet
    {
        public int posX;
        public int posY;
        public Image image;
        public Goods goods;
        public int aliens;
        public int health;
    }

    public struct Goods
    {
        public int ammo;
        public int steel;
        public int food;
    }

    public struct Bullet
    {
        public int posX;
        public int posY;
        public Image image;

        public Bullet(int posX, int posY, Image image) 
        { 
            this.posX = posX;
            this.posY = posY;
            this.image = image;
        }
    }
}
