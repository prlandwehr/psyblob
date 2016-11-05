using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    public class Spikes : EnemySprite
    {
        private int widthMult;

        public Spikes(int width)
            : base(100, 100)
        {
            invincible = true;
            slashable = false;
            shootable = false;
            widthMult = width;
        }

        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            //spriteBatch.Draw(basesprite, base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            Vector2 currentpos;
            for (int i = 0; i < widthMult; i++)
            {
                currentpos = base.position + new Vector2(basesprite.Width * i, 0);
                //Only draw if onscreen
                if (currentpos.X > swidth || currentpos.X < -bbox.Width || currentpos.Y > sheight || currentpos.Y < -bbox.Height)
                {
                    //offscreen
                }
                else
                {
                    spriteBatch.Draw(basesprite, currentpos, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
                }
            }
        }

        public override void Update(GameTime gtime)
        {
            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            //
            bbox.Width = (int)(basesprite.Width);
            bbox.Height = (int)(basesprite.Height);
        }


        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            //
            bbox.Width = (int)(basesprite.Width * widthMult);
            bbox.Height = (int)(basesprite.Height);
        }

        public override System.Collections.ArrayList getAttacks(Vector2 playerPos)
        {
            return null;
        }
    }
}
