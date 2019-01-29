using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    internal class Speed : Entity
    {
        public static bool speedup;

        int timepassed = 0;
        public Speed(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
        }

        public static Speed CreateSpeed(Vector2 position)
        {
            var ammo = new Speed(GameRoot.Ammo, position);

            return ammo;
        }

        public override void Update()
        {
            

            if (speedup == true && timepassed < 200)
            {
                timepassed = timepassed + 1;
                PlayerShip.speed = 10;
            }
            else
            {
                speedup = false;
                timepassed = 0;
                PlayerShip.speed = 5;
            }
            
        }

    }
}
