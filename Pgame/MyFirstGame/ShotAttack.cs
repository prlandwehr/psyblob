using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PgameNS
{
    class ShotAttack : Attack
    {
        //FIELDS
        private static Texture2D basesprite;
        private static Texture2D basespritel;
        private static SoundEffect sound;
        private bool soundplayed = false;
        private const float speed = 350;
        private double deltaTime = 0;

        //Uses field setting constructor from Psprite to set how long attack lasts
        public ShotAttack()
            : base(Vector2.Zero, Vector2.Zero, Vector2.Zero, 2.0)
        {
            base.Damage = 10;
        }

        //Standard update method, coordinant portion needs updating
        public override void Update(GameTime gtime)
        {
            base.Update(gtime);

            deltaTime += gtime.ElapsedGameTime.TotalSeconds;

            if (!soundplayed)
            {
                sound.Play();
                soundplayed = true;
                //Console.WriteLine("sound played?");
            }

            if (facing == Facing.Right)
            {
                velocity.X = speed;
            }
            else
            {
                velocity.X = -speed;
            }

            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            //
            bbox.Width = (int)(basesprite.Width);
            bbox.Height = (int)(basesprite.Height);
            //
        }

        public override void Update(GameTime gtime, Player player)
        {
            this.Update(gtime);
        }

        //Set attack texture, needs animation support
        public void setBaseSprite(Texture2D texR, Texture2D texL)
        {
            basesprite = texR;
            basespritel = texL;
            bbox = new Rectangle((int)position.X, (int)position.Y, (int)texR.Width, (int)texR.Height);
        }

        //Draw method needs animation support
        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            //double index = 2 * timeExisted / .25;
            //deltaT * number of frames / timefor complete loop % number of frames
            int index = ((int)(deltaTime * 2 / .4)) % 2;
            if (index == 0)
            {
                spriteBatch.Draw(basesprite, base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(basespritel, base.position, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
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
