using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    class EnemyShot : EnemySprite
    {
        private const double lifetime = 5;
        private const float damage = 20;
        private float rotation = 0;
        private Texture2D[] textures;
        private double deltaTime = 0;
        private int texindex = 0;

        public EnemyShot()
            : base(20,damage, lifetime)
        {
            invincible = true;
            slashable = false;
            shootable = false;
            base.explodes = false;
        }

        public override void Update(GameTime gtime)
        {
            base.Update(gtime);
        }


        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            spriteBatch.Draw(textures[texindex], base.position, null, Color.White, rotation, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            Update(gtime);
            invincible = true;

            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            texindex = (int)(4 * deltaTime) % textures.Length;

            //rotation -= (float)gtime.ElapsedGameTime.TotalSeconds * 3;
        }

        public void setTextures(Texture2D[] texarray)
        {
            textures = texarray;
            setBaseSprite(texarray[0]);
        }

        public override System.Collections.ArrayList getAttacks(Vector2 playerPos)
        {
            return null;
        }
    }
}
