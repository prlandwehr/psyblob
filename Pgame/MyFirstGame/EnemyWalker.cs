using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    //Basic enemy class -- Homes in on player
    public class EnemyWalker : EnemySprite
    {
        //private const float actiondistance = 300;
        //private const float pursuitspeed = 150;
        private const float movespeed = 100;
        private float dx;
        private float dy;
        private float movetimeX;
        private float movetimeY;
        private double timeCounterX;
        private double timeCounterY;

        public EnemyWalker(float dx, float dy)
            : base(60, 20)
        {
            this.dx = dx;
            this.dy = dy;
            movetimeX = dx / 100;
            movetimeY = dy / 100;
            if (dx != 0)
            {
                this.velocity.X = movespeed;
            }
            if (dy != 0)
            {
                this.velocity.Y = movespeed;
            }
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            base.Update(gtime);

            timeCounterX += gtime.ElapsedGameTime.TotalSeconds;
            if (timeCounterX >= movetimeX)
            {
                velocity.X = -velocity.X;
                timeCounterX = 0;
            }
            timeCounterY += gtime.ElapsedGameTime.TotalSeconds;
            if (timeCounterY >= movetimeY)
            {
                velocity.Y = -velocity.Y;
                timeCounterY = 0;
            }
        }

        /*public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            spriteBatch.Draw(esprite, base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
        }*/

        public override System.Collections.ArrayList getAttacks(Vector2 playerPos)
        {
            return null;
        }
    }
}
