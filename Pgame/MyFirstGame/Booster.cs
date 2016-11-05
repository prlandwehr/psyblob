using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace PgameNS
{
    class Booster : EnemySprite
    {

        private Texture2D[] textures;
        private double deltaTime = 0;
        private int texindex = 0;
        private double animationRate = 4;
        //boost fields
        private double boostduration;
        private Vector2 boostvelocity;

        public Booster()
            : base(1, 0)
        {
            invincible = true;
            slashable = false;
            shootable = false;
        }

        public void setBoost(double duration, Vector2 bvelocity)
        {
            boostduration = duration;
            boostvelocity = bvelocity;
        }

        public double Bduration
        {
            get
            {
                return boostduration;
            }
        }

        public Vector2 Bvelocity
        {
            get
            {
                return boostvelocity;
            }
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            base.Update(gtime);
            invincible = true;

            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            texindex = (int)(animationRate * deltaTime) % textures.Length;
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
