using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    //Sprite class for enemies - intended to be inherited by specific enemy type classes
    public abstract class EnemySprite : Psprite
    {
        //Fields
        protected float life;
        protected float collisiondmg;
        protected float startinglife;
        protected Texture2D basesprite;
        protected bool invincible;
        protected double invincibleTime;
        protected const double invConst = .2;
        //invulnerability booleans
        public bool shootable = true;
        public bool slashable = true;
        //
        public bool attacking = true;
        public bool explodes = true; //does explosion happen when sprite dies
        public bool islevelboss = false;

        public EnemySprite()
            : base()
        {
            life = 1;
            collisiondmg = 1;
            invincible = false;
            invincibleTime = invConst;
        }

        //Constructor with a specified life value
        public EnemySprite(float eLife, float cdmg)
            : base()
        {
            this.startinglife = eLife;
            this.life = eLife;
            this.collisiondmg = cdmg;
            invincible = false;
            invincibleTime = invConst;
        }

        //specify life, damage, and lifetime
        public EnemySprite(float eLife, float cdmg, double ltime)
            : base(Vector2.Zero,Vector2.Zero,Vector2.Zero, ltime)
        {
            this.startinglife = eLife;
            this.life = eLife;
            this.collisiondmg = cdmg;
            invincible = false;
            invincibleTime = invConst;
        }

        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            if (invincible)
            {
                spriteBatch.Draw(basesprite, base.position, null, Color.Red, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(basesprite, base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
        }

        public void setBaseSprite(Texture2D sprite)
        {
            basesprite = sprite;
            bbox = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Width, (int)sprite.Height);
        }

        public override void Update(GameTime gtime)
        {
            base.Update(gtime);
            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            //
            //bbox.Width = (int)(basesprite.Width);
            //bbox.Height = (int)(basesprite.Height);
            //
            if (invincible)
            {
                invincibleTime -= gtime.ElapsedGameTime.TotalSeconds;
                if (invincibleTime <= 0)
                {
                    invincible = false;
                }
            }
        }

        public abstract void Update(GameTime gtime, Vector2 playerPos);

        //return list containing attack sprite to be added to the game
        public abstract ArrayList getAttacks(Vector2 playerPos);

        public virtual void enemyHit(float damage)
        {
            if (!invincible)
            {
                this.life -= damage;
                //Console.WriteLine("enemy hit for: " + damage);
                invincible = true;
                invincibleTime = invConst;
            }
        }

        public float Life
        {
            set
            {
                life = value;
            }
            get
            {
                return life;
            }
        }

        public float CollisionDmg
        {
            set
            {
                //
            }
            get
            {
                return collisiondmg;
            }
        }

    }
}
