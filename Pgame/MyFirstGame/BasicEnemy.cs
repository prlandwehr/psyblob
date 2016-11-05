using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    //Basic enemy class -- Homes in on player
    public class BasicEnemy : EnemySprite
    {
        private const float actiondistance = 300;
        private const float pursuitspeed = 150;

        public BasicEnemy()
            : base(40, 20)
        {
            //
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            base.Update(gtime);

            //enemy homes in on player if within actionable distance
            float dx = position.X - playerPos.X;
            float dy = position.Y - playerPos.Y;
            float dist = (float) Math.Sqrt(dx * dx + dy * dy);
            if (dist <= actiondistance || life < startinglife)
            {
                velocity.X = -dx / dist * pursuitspeed;
                velocity.Y = -dy / dist * pursuitspeed;
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
