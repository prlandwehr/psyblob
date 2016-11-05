using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{

    //Abstract attack class
    public abstract class Attack : Psprite
    {
        //Fields
        private float damage;
        private float duration;

        //Uses field setting constructor from Psprite to set how long attack lasts
        public Attack(Vector2 pos, Vector2 vel, Vector2 acc, double l)
            : base(pos, vel, acc, l)
        {
            //
        }

        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            //
        }

        public float Damage
        {
            set
            {
                damage = value;
            }
            get
            {
                return damage;
            }
        }

        public float Duration
        {
            set
            {
                duration = value;
            }
            get
            {
                return duration;
            }
        }

        public abstract void Update(GameTime gtime, Player player);
    }
}
