using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    class Boss1 : EnemySprite
    {
        private const float health = 500;
        private const float colDamage = 40;
        private const float attackcooldown = 1;
        private const float moveSpeed = 100;
        private const float moveTime = 3;
        //private const float moveCooldown = 2;
        private const float shotvx = 200;
        private const float shotvy = 50;
        private const float volleyvx = 200;
        private const float volleyvy = 600;
        private const float volleyacc = 800;
        private float movetimer = 0;
        private double attacktimer = 0;
        private Random randGen;
        private enum BossState
        {
            movel = 0,
            mover = 1,
            shots = 2,
            volley = 3,
            wait = 4
        }
        private BossState currentstate;
        private BossState lastmove;
        private bool fired = false;
        private static Texture2D[] shotTex;
        private Texture2D[] bossTex;

        public Boss1()
            : base(health, colDamage)
        {
            currentstate = BossState.movel;
            lastmove = BossState.movel;
            randGen = new Random();
            islevelboss = true;
        }

        public override void Update(GameTime gtime, Vector2 playerPos)
        {
            base.Update(gtime);

            if (currentstate == BossState.movel)
            {
                velocity.X = -moveSpeed;
                movetimer += (float)gtime.ElapsedGameTime.TotalSeconds;
                facing = Facing.Left;
                if (movetimer >= moveTime)
                {
                    currentstate = BossState.wait;
                    lastmove = BossState.movel;
                    movetimer = 0;
                }
            }
            else if (currentstate == BossState.mover)
            {
                velocity.X = moveSpeed;
                movetimer += (float)gtime.ElapsedGameTime.TotalSeconds;
                facing = Facing.Right;
                if (movetimer >= moveTime)
                {
                    currentstate = BossState.wait;
                    lastmove = BossState.mover;
                    movetimer = 0;
                }
            }
            else if (currentstate == BossState.shots)
            {
                if (fired)
                {
                    if (lastmove == BossState.mover)
                    {
                        currentstate = BossState.movel;
                    }
                    else
                    {
                        currentstate = BossState.mover;
                    }
                    fired = false;
                }
            }
            else if (currentstate == BossState.volley)
            {
                if (fired)
                {
                    if (lastmove == BossState.mover)
                    {
                        currentstate = BossState.movel;
                    }
                    else
                    {
                        currentstate = BossState.mover;
                    }
                    fired = false;
                }
            }
            else if (currentstate == BossState.wait)
            {
                velocity.X = 0;
                attacktimer += (float)gtime.ElapsedGameTime.TotalSeconds;
                if (attacktimer >= attackcooldown)
                {
                    attacktimer = 0;
                    int newstate = randGen.Next(2);
                    if (newstate == 0)
                    {
                        currentstate = BossState.shots;
                    }
                    else
                    {
                        currentstate = BossState.volley;
                    }
                }
            }
        }

        public override ArrayList getAttacks(Vector2 playerPos)
        {
            
            ArrayList attacks = new ArrayList();
            //Create shots for shots attack
            float dx = position.X - playerPos.X;
            if (currentstate == BossState.shots)
            {
                EnemyShot attack = new EnemyShot();
                EnemyShot attack2 = new EnemyShot();
                EnemyShot attack3 = new EnemyShot();
                attack.setTextures(shotTex);
                attack2.setTextures(shotTex);
                attack3.setTextures(shotTex);
                //set shots X axis data
                if (dx > 0)
                {
                    attack.velocity.X = -shotvx;
                    attack.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack2.velocity.X = -shotvx;
                    attack2.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack3.velocity.X = -shotvx;
                    attack3.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                }
                else
                {
                    attack.velocity.X = shotvx;
                    attack.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack2.velocity.X = shotvx;
                    attack2.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack3.velocity.X = shotvx;
                    attack3.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                }
                //set shots Y axis data
                attack2.velocity.Y = -shotvy;
                attack3.velocity.Y = shotvy;
                attack.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);
                attack2.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);
                attack3.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);

                //add shots
                attacks.Add(attack);
                attacks.Add(attack2);
                attacks.Add(attack3);
                fired = true;
                return attacks;
            }
            else if (currentstate == BossState.volley)
            {
                EnemyShot attack = new EnemyShot();
                EnemyShot attack2 = new EnemyShot();
                EnemyShot attack3 = new EnemyShot();
                attack.setTextures(shotTex);
                attack2.setTextures(shotTex);
                attack3.setTextures(shotTex);
                //set shots X axis data
                if (dx > 0)
                {
                    attack.velocity.X = -volleyvx;
                    attack.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack2.velocity.X = -(volleyvx + 50);
                    attack2.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack3.velocity.X = -(volleyvx - 50);
                    attack3.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                }
                else
                {
                    attack.velocity.X = volleyvx;
                    attack.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack2.velocity.X = (volleyvx + 50);
                    attack2.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                    attack3.velocity.X = (volleyvx - 50);
                    attack3.position.X = bbox.Center.X - (attack.bbox.Width / 2);
                }
                //set shots Y axis data
                attack.velocity.Y = -volleyvy;
                attack2.velocity.Y = -volleyvy;
                attack3.velocity.Y = -volleyvy;
                attack.acceleration.Y = volleyacc;
                attack2.acceleration.Y = volleyacc;
                attack3.acceleration.Y = volleyacc;
                attack.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);
                attack2.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);
                attack3.position.Y = bbox.Center.Y - (attack.bbox.Width / 2);

                //add shots
                attacks.Add(attack);
                attacks.Add(attack2);
                attacks.Add(attack3);
                fired = true;
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
