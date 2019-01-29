using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        static float inverseSpawnChance = 60;
        static float ammoChance = 60;
        public static int Count = 0;

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (rand.Next(1, 500) == 1)
                {
                    EntityManager.Add(Speed.CreateSpeed(GetSpawnPosition()));
                }
                if (rand.Next(1,250) == 1)
                {
                    EntityManager.Add(Ammo.CreateAmmo(GetSpawnPosition()));
                }
                if (rand.Next((int)inverseSpawnChance) == 0)
                {
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));
                }
                if (rand.Next(1, 2) == 1 && GameRoot.state3 == true && Count < 1)
                {
                    Count = Count + 1;
                    EntityManager.Add(Bosses.CreateBosses(GetSpawnPosition()));
                }
            }

            // slowly increase the spawn rate as time progresses
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;
            if (ammoChance > 20)
                ammoChance -= 0.5f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next((int)GameRoot.ScreenSize.X) - 100, rand.Next((int)GameRoot.ScreenSize.Y) - 100);
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}
