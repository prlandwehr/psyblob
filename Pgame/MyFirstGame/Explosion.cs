using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace PgameNS
{
    class Explosion : EnemySprite
    {
        private static Texture2D[] textures;
        private double deltaTime = 0;
        private int texindex = 0;
        private const float damage = 0;
        private const double lifetime = .5;
        private const float animationRate = 8; //frames per second

        public Explosion()
            : base(20,damage, lifetime)
        {
            invincible = true;
            slashable = false;
            shootable = false;
            base.explodes = false;
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

        public override ArrayList getAttacks(Vector2 playerPos)
        {
            return null;
        }

        public void setTextures(Texture2D[] texarray)
        {
            textures = texarray;
            setBaseSprite(texarray[0]);
        }
    }
}
