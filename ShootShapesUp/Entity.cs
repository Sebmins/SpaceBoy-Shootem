using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    abstract class Entity
    {
        protected Texture2D image;
        // The tint of the image. This will also allow us to change the transparency.
        protected Color color = Color.White;

        public Vector2 Position, Velocity;
        public float Orientation;
        public float Radius = 20;   // used for circular collision detection
        public bool IsExpired;      // true if the entity was destroyed and should be deleted.

        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, 1f, 0, 0);
        }
        public virtual void DrawAmmo(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameRoot.Ammo, Position, Color.White);
        }
        public virtual void DrawSpeed(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameRoot.Speed, Position, Color.White);
        }
        public virtual void DrawBosses(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameRoot.Boss, Position, Color.White);
        }
    }
}
