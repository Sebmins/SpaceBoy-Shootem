using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();
        static List<Ammo> ammo = new List<Ammo>();
        static List<Speed> speed = new List<Speed>();
        static List<Bosses> bosses = new List<Bosses>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int Count { get { return entities.Count; } }
        public static void Add(Entity entity)
        {
            if (!isUpdating)
            { 
            AddEntity(entity);
            AddAmmo(entity);
            AddSpeed(entity);
            AddBosses(entity);
                
            }
            else
                addedEntities.Add(entity);
        }

        public static void removeDecals()
        {
            bullets.Clear();
            enemies.Clear();
            bosses.Clear();
           // entities.Clear();
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);

        }

        private static void AddAmmo(Entity entammo)
        {
            if (entammo is Ammo)
            {
                ammo.Add(entammo as Ammo);
            }
        }

        private static void AddSpeed(Entity entspeed)
        {
            if(entspeed is Speed && GameRoot.state23 == true)
            {
                speed.Add(entspeed as Speed);
            }
        }
        
        private static void AddBosses(Entity entbosses)
        {
            if(entbosses is Bosses && GameRoot.state3 == true)
            {
                bosses.Add(entbosses as Bosses);
            }
        }


        public static void Update()
        {

            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
           
        }

        #region collision
        static void HandleCollisions()
        {
            for (int i = 0; i < bosses.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, bosses[i]))
                {
                    PlayerShip.Instance.Kill();
                    EnemySpawner.Reset();
                    break;
                }
            }

            // handle collisions between bullets and enemies
            for (int i = 0; i < bosses.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(bosses[i], bullets[j]))
                    {
                        bosses[i].WasHit();
                        bullets[j].IsExpired = true;
                    }
                }

            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }

            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                    }
                }

            for (int i = 0; i < ammo.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, ammo[i]))
                {
                    PlayerShip.ammoCounter += 20;

                    ammo.Remove(ammo[i]);
                }
            }

            for (int i = 0; i < speed.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, speed[i]))
                {
                    Speed.speedup = true;

                    speed.Remove(speed[i]);
                    
                }
            }

            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    PlayerShip.Instance.Kill();
                    enemies.ForEach(x => x.WasRemoved());
                    EnemySpawner.Reset();
                    break;
                }
            }
        }

        public static void clear()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].WasRemoved();
            }

            for (int j = 0; j < bullets.Count; j++)
            {
                bullets[j].IsExpired = true;
            }
            for (int i = 0; i < ammo.Count; i++)
            {
                ammo.Remove(ammo[i]);
            }
            for(int i = 0; i < speed.Count; i++)
            {
               speed.Remove(speed[i]);
            }
            for(int i= 0; i < bosses.Count; i++)
            {
                bosses[i].WasRemoved();
            }

        }

        #endregion

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);

            foreach (var ammoies in ammo)
                ammoies.DrawAmmo(spriteBatch);

            foreach (var speedoies in speed)
                speedoies.DrawSpeed(spriteBatch);

         //   foreach (var bossies in bosses)
             //   bossies.DrawBosses(spriteBatch);
        }

    }
}
