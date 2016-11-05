using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PgameNS
{
    //Player class
    public class Player : Psprite
    {
        //PLAYER FIELDS

        //Life fields
        private float life;
        private const float maxlife = 100;
        private float lives;
        //Physics fields
        private const float basegrav = 600;
        //public float currentgrav;
        private const float runspeed = 200;
        private const float jumpspeed = 400;
        private const float bboxScale = 0.8f;
        //public Rectangle bbox;
        public Rectangle hitbox;
        private bool jumping;
        private const int maxjumps = 2;
        private int cjumps;
        //Graphics fields
        private Texture2D[] baseR;
        private Texture2D[] baseL;
        //private Texture2D basesprite;
        //private Texture2D basespriteL;
        private Texture2D jumpr;
        private Texture2D jumpl;
        Color cOverlay = Color.White;
        //Sound fields;
        private static SoundEffect jumpsound;
        //State fields
        public enum Weapon
        {
            Pyrokinesis = 0,
            EnergyShot = 1
        }
        public Weapon cWeapon;
        private enum Pstate
        {
            normal = 0,
            boost = 1
        }
        private Pstate pstate;
        private bool invincible;
        private double invincibleTime;
        private const double invConst = 2;
        private const double animRate = 3;
        private double deltaTime = 0;
        //boost fields
        private double boosttime;
        private double boostduration;
        private Vector2 boostvelocity;
        //Attack fields
        private bool attacking;
        private const double attackCooldown = 0.3;
        private double cooldowntime;

        //Base constructor
        public Player()
            : base()
        {
            life = maxlife;
            lives = 3;
            jumping = false;
            attacking = false;
            cjumps = 0;
            invincible = false;
            invincibleTime = invConst;
            cooldowntime = attackCooldown;
            facing = Facing.Right;
            cWeapon = Weapon.Pyrokinesis;
            base.acceleration.Y = basegrav;
            pstate = Pstate.normal;
        }

        public void setBoost(double boostdur, Vector2 boostvel)
        {
            boosttime = 0;
            boostduration = boostdur;
            boostvelocity = boostvel;
            pstate = Pstate.boost;
            this.velocity = boostvelocity;

            cjumps = 0;
        }

        //Set the player's base sprite to given textures
        public void setBaseSprite(Texture2D[] sprite, Texture2D[] spriteL)
        {
            baseR = sprite;
            baseL = spriteL;
            bbox = new Rectangle((int)position.X, (int)position.Y, (int)sprite[0].Width, (int)sprite[0].Height);
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(sprite[0].Width * bboxScale), (int)(sprite[0].Height * bboxScale));
        }

        public void setJumpSprite(Texture2D sprite, Texture2D spriteL)
        {
            jumpr = sprite;
            jumpl = spriteL;
        }

        //Draw the player's base sprite
        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            int index = 0;
            if (velocity.X != 0 && !jumping)
            {
                index = (int)(animRate * deltaTime) % baseR.Length;
            }

            //If player is invincible tint red
            if (invincible)
            {
                cOverlay = Color.Red;
            }
            else
            {
                cOverlay = Color.White;
            }
            //Draw based on player condition
            if (facing == Facing.Right && jumping)
            {
                spriteBatch.Draw(jumpr, base.position, null, cOverlay, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else if (facing == Facing.Right && !jumping)
            {
                spriteBatch.Draw(baseR[index], base.position, null, cOverlay, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else if (facing == Facing.Left && !jumping)
            {
                spriteBatch.Draw(baseL[index], base.position, null, cOverlay, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else if (facing == Facing.Left && jumping)
            {
                spriteBatch.Draw(jumpl, base.position, null, cOverlay, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
        }

        //Basic player update method
        public override void Update(GameTime gtime)
        {
            //base and position related updates
            base.Update(gtime);
            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            hitbox.X = (int)(position.X + ((baseR[0].Width - bbox.Width) / 2));
            hitbox.Y = (int)(position.Y + ((baseR[0].Height - bbox.Height) / 2));

            //update dt. dt used for animation calculations, etc.
            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            //update boost
            if (pstate == Pstate.boost)
            {
                boosttime += gtime.ElapsedGameTime.TotalSeconds;
                this.velocity.X = boostvelocity.X;
                if (boosttime >= boostduration)
                {
                    pstate = Pstate.normal;
                }
            }
            
            //temp. invincibility updates
            if (invincible)
            {
                invincibleTime -= gtime.ElapsedGameTime.TotalSeconds;
                if (invincibleTime <= 0)
                {
                    invincible = false;
                }
            }
            //attack status updates
            if (attacking)
            {
                cooldowntime -= gtime.ElapsedGameTime.TotalSeconds;
                if (cooldowntime <= 0)
                {
                    attacking = false;
                    cooldowntime = attackCooldown;
                }
            }
        }

        //Update method that also updates player's attacks
        public void Update(GameTime gtime, ArrayList attacks)
        {
            //run basic update
            this.Update(gtime);
            //remove time expired attacks
            if (attacks.Count > 0)
            {
                foreach (Psprite item in attacks)
                {
                    if (!item.isAlive())
                    {
                        attacks.Remove(item);
                        //attacks.Clear();
                        break;
                    }
                }
            }
            /*
            //if no attacks left not attacking
            else
            {
                attacking = false;
            }
             */
        }

        //Get + set for life related fields
        public float Plife
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

        public float Plives
        {
            set
            {
                lives = value;
            }
            get
            {
                return lives;
            }
        }

        /*public float Pmaxlife
        {
            set
            {
                maxlife = value;
            }
            get
            {
                return maxlife;
            }
        }*/

        public SoundEffect Jumpsound
        {
            set
            {
                jumpsound = value;
            }
            get
            {
                return jumpsound;
            }
        }

        //Method for right movement
        public void rightPressed()
        {
            if (pstate != Pstate.boost)
            {
                velocity.X = runspeed;
            }
            else
            {
                velocity.X = boostvelocity.X + runspeed;
            }
            facing = Facing.Right;
        }

        //Method for left movement
        public void leftPressed()
        {
            if (pstate != Pstate.boost)
            {
                velocity.X = -runspeed;
            }
            else
            {
                velocity.X = boostvelocity.X - runspeed;
            }
            facing = Facing.Left;
        }

        //No movement input
        public void moveNotPressed()
        {
            if (pstate != Pstate.boost)
            {
                velocity.X = 0;
            }
        }

        //Method for jump button
        public void jumpPressed()
        {
            if (pstate != Pstate.boost)
            {
                if (!jumping && cjumps < maxjumps)
                {
                    jumpsound.Play();
                    velocity.Y = -jumpspeed;
                    jumping = true;
                    cjumps++;
                }
            }
            else
            {
                if (!jumping && cjumps < maxjumps)
                {
                    jumpsound.Play();
                    if (velocity.Y < 0)
                    {
                        velocity.Y -= jumpspeed;
                    }
                    else
                    {
                        velocity.Y = -jumpspeed;
                    }
                    jumping = true;
                    cjumps++;
                }
            }
        }

        //Method for attack button
        public Psprite attackPressed()
        {
            //If not attacking generate attack based on current weapon
            if (!attacking)
            {
                if (cWeapon == Weapon.Pyrokinesis)
                {
                    Psprite attack = new SlashAttack();
                    if (facing == Facing.Right)
                    {
                        attack.position.X = this.position.X + baseR[0].Width;
                        attack.position.Y = this.position.Y;
                    }
                    else
                    {
                        //UPDATE FOR PROPER SCALING
                        attack.position.X = this.position.X - baseR[0].Width - 80;
                        attack.position.Y = this.position.Y;
                    }
                    attacking = true;
                    return attack;
                }
                else if (cWeapon == Weapon.EnergyShot)
                {
                    Psprite attack = new ShotAttack();
                    if (facing == Facing.Right)
                    {
                        attack.position.X = this.position.X + baseR[0].Width;
                        attack.position.Y = this.position.Y;
                        attack.facing = Facing.Right;
                        //attack.velocity.X = 225;
                    }
                    else
                    {
                        //UPDATE FOR PROPER SCALING
                        attack.position.X = this.position.X - 40;
                        attack.position.Y = this.position.Y;
                        attack.facing = Facing.Left;
                        //attack.velocity.X = -225;
                    }
                    attacking = true;
                    return attack;
                }
            }
            //otherwise return null
            return null;
        }

        //Method for setting the jumping bool
        public void jumpNotPressed()
        {
            jumping = false;
            //If player has upward velocity but isn't jumping set Y velocity to zero
            if (pstate != Pstate.boost)
            {
                if (velocity.Y < 0)
                {
                    velocity.Y = 0;
                }
            }
        }

        //Method for reseting jumping fields
        public void jumpsReset()
        {
            jumping = false;
            cjumps = 0;
        }

        //Method called when player is hit by an enemy
        public void playerHit(float damage)
        {
            if (!invincible && damage > 0)
            {
                this.life -= damage;
                //Console.WriteLine("hit for: " + damage);
                invincible = true;
                invincibleTime = invConst;
            }
        }

    }
}
