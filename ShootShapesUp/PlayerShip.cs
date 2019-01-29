using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class PlayerShip : Entity
    {
        
        public static float speed = 5;
        private MouseState MouseState;
        private static PlayerShip instance;
        public static int ammoCounter = 100;
        private static int leftOrRight = 0;

        public static PlayerShip Instance
           
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
            }
        }

        const int cooldownFrames = 8;
        int cooldownRemaining = 0;

        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }

        static Random rand = new Random();

        private PlayerShip()
        {
            image = GameRoot.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        
        public override void Update()
        {
                       
            if (IsDead)
            {
                //Console.WriteLine("UmmDead");
                --framesUntilRespawn;
                return;
            }

            MouseState = Mouse.GetState();

            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                if (MouseState.LeftButton == ButtonState.Pressed && ammoCounter>0)
                {
                    leftOrRight = leftOrRight + 1;
                    ammoCounter = ammoCounter-1;
                    cooldownRemaining = cooldownFrames;
                    float aimAngle = aim.ToAngle();
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = 30f * new Vector2((float)Math.Cos(aimAngle + randomSpread), (float)Math.Sin(aimAngle + randomSpread));

                    if (leftOrRight % 2 == 0)
                    {
                        Vector2 offset = Vector2.Transform(new Vector2(35, -20), aimQuat);
                        EntityManager.Add(new Bullet(Position + offset, vel));
                    }
                    else
                    {
                        Vector2 offset = Vector2.Transform(new Vector2(35, 20), aimQuat);
                        EntityManager.Add(new Bullet(Position + offset, vel));
                    }

                    GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                }
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;

            
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }

        public void Kill()
        {
            framesUntilRespawn = 60;
        }
    }
}
