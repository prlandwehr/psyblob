using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    class EnemyTurret : EnemySprite
    {
        //private const double lifetime = 5;
        private const float damage = 20;
        private const float attackvelocity = 175;
        private const float actiondistance = 400;
        private const float attackcooldown = 2;
        private double attacktimer = 0;
        private static Texture2D[] shotTex;

        public EnemyTurret()
            : base(50,damage)
        {
            //
        }

        public override void Update(GameTime gtime)
        {
            base.Update(gtime);
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            Update(gtime);

            //if attack not on cooldown shoot at player
            float dx = position.X - playerPos.X;
            float dy = position.Y - playerPos.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (attacking)
            {
                attacktimer += gtime.ElapsedGameTime.TotalSeconds;
                if (attacktimer >= attackcooldown)
                {
                    attacking = false;
                }
            }
        }

        public override ArrayList getAttacks(Vector2 playerPos)
        {
            float dx = position.X - playerPos.X;
            float dy = position.Y - playerPos.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            if ((!attacking) && (dist <= actiondistance || life < startinglife))
            {
                ArrayList attacks = new ArrayList();
                EnemyShot theattack = new EnemyShot();

                theattack.position = this.position;
                theattack.velocity.X = -dx / dist * attackvelocity;
                theattack.velocity.Y = -dy / dist * attackvelocity;
                theattack.setTextures(shotTex);

                attacks.Add(theattack);

                attacking = true;
                attacktimer = 0;

                return attacks;
            }
            return null;
        }

        public Texture2D[] ShotTex
        {
            set
            {
                shotTex = value;
            }
        }
    }
}
