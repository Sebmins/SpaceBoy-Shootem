using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootShapesUp
{
    internal class Ammo : Entity
    {

        public Ammo(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
        }

        public static Ammo CreateAmmo(Vector2 position)
        {
            var ammo = new Ammo(GameRoot.Ammo, position);

            return ammo;
        }


        public override void Update()
        {
        }
    }
}