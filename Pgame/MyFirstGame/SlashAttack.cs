using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PgameNS
{
    public class SlashAttack : Attack
    {
        //FIELDS
        private static Texture2D[] textRight;
        private static Texture2D[] textLeft;
        private static Texture2D basesprite;
        private static Texture2D basespritel;
        private static SoundEffect sound;
        private bool soundplayed = false;
        private const float slashDamage = 20;
        private const double slashDuration = 0.3;
        //used for animation
        private double deltaTime;
        //private float damage;
        //public Rectangle bbox;

        //Uses field setting constructor from Psprite to set how long attack lasts
        public SlashAttack()
            : base(Vector2.Zero, Vector2.Zero, Vector2.Zero, slashDuration)
        {
            base.Damage = slashDamage;
            deltaTime = 0;
        }

        //Standard update method, coordinant portion needs updating
        public override void Update(GameTime gtime)
        {
            base.Update(gtime);
        }

        //Update method which takes player position in order to position slash correctly
        public override void Update(GameTime gtime, Player player)
        {
            base.Update(gtime);

            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            if (!soundplayed)
            {
                sound.Play();
                soundplayed = true;
                //Console.WriteLine("sound played?");
            }

            if (player.facing == Facing.Right)
            {
                this.facing = Facing.Right;
                this.position.X = player.position.X + player.bbox.Width;
                this.position.Y = player.position.Y;

                this.bbox.X = (int)position.X;
                this.bbox.Y = (int)position.Y;
                this.bbox.Width = (int)(basesprite.Width);
                this.bbox.Height = (int)(basesprite.Height);
            }
            else
            {
                this.facing = Facing.Left;
                this.position.X = player.position.X - basespritel.Width;
                this.position.Y = player.position.Y;

                this.bbox.X = (int)position.X;
                this.bbox.Y = (int)position.Y;
                this.bbox.Width = (int)(basespritel.Width);
                this.bbox.Height = (int)(basespritel.Height);
            }
        }

        //Set attack texture, needs animation/facing support
        public void setBaseSprite(Texture2D[] texR, Texture2D[] texL)
        {
            textLeft = texL;
            textRight = texR;
            bbox = new Rectangle((int)position.X, (int)position.Y, (int)texR[0].Width, (int)texR[0].Height);
            
            basesprite = texR[0];
            basespritel = texL[0];
            //bbox = new Rectangle((int)position.X, (int)position.Y, (int)texR.Width, (int)texR.Height);
        }

        //Draw method needs animation support
        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            //calculates which sprite to draw based on time since attacks start and draws
            int index = (int)(deltaTime * textRight.Length / slashDuration) % textRight.Length;
            //Console.WriteLine(index);
            //if (index >= textRight.Length)
            //{
            //    index = 0;
            //}
            if (this.facing == Facing.Right)
            {
                spriteBatch.Draw(textRight[index], base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(textLeft[index], base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
        }

        public SoundEffect Sound
        {
            set
            {
                sound = value;
            }
            get
            {
                return sound;
            }
        }
    }
}
