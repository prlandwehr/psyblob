using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace PgameNS
{
    class LevelGoal : EnemySprite
    {

        private Texture2D[] textures;
        private double deltaTime = 0;
        private int texindex = 0;

        public LevelGoal() : base(1,0)
        {
            invincible = true;
            slashable = false;
            shootable = false;
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            base.Update(gtime);
            invincible = true;

            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            texindex = (int)(4 * deltaTime) % textures.Length;
            //Console.Out.WriteLine(texindex);
        }

        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            spriteBatch.Draw(textures[texindex], base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
        }

        public void setTextures(Texture2D[] texarray)
        {
            textures = texarray;
            setBaseSprite(texarray[0]);
        }

        public override ArrayList getAttacks(Vector2 playerPos)
        {
            return null;
        }
    }
}
