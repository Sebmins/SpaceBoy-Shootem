using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class AmmoCrate : Entity
    {
        private static AmmoCrate instance;

        private AmmoCrate()
        {
            image = GameRoot.Wanderer;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;

            //EntityManager.extraAmmo(AmmoCrate);
        }

        

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
