using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    //Class for ground and platforms. Can be tiled based on fields
    public class GroundSprite : Psprite
    {
        private Texture2D basesprite;
        //width and height multiples for tileing
        private int width;
        private int height;
        //textureName currently unused - meant for loading proper texture
        private string textureName;
        //public Rectangle bbox;

        //Base constructor
        public GroundSprite() : base()
        {
            width = 1;
            height = 1;
        }

        //Tiled constructor
        public GroundSprite(int w, int h)
            : base()
        {
            width = w;
            height = h;
        }

        //Set texture, bbox
        public void setBaseSprite(Texture2D sprite)
        {
            basesprite = sprite;
            bbox = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Width * width, (int)sprite.Height * height);
        }

        //Draws groundsprite, tilling as defined by height/width
        public override void draw(SpriteBatch spriteBatch, float swidth, float sheight)
        {
            //spriteBatch.Draw(basesprite, base.getPosition(), Color.White);
            Vector2 currentpos;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    currentpos = base.position + new Vector2(basesprite.Width * i, basesprite.Height * j);
                    //Only draw if onscreen
                    if (currentpos.X > swidth || currentpos.X < -bbox.Width || currentpos.Y > sheight || currentpos.Y < -bbox.Height)
                    {
                        //offscreen
                    }
                    else
                    {
                        spriteBatch.Draw(basesprite, currentpos, null, Color.White, 0f, Vector2.Zero, base.Pscale, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        //Update position, bbox
        public override void Update(GameTime gtime)
        {
            base.Update(gtime);
            bbox.X = (int)position.X;
            bbox.Y = (int)position.Y;
            //
            bbox.Width = (int)(width * basesprite.Width);
            bbox.Height = (int)(height * basesprite.Height);
            //
        }

        //Get+Set access to fields
        public int Width
        {
            set
            {
                width = value;
            }
            get
            {
                return width;
            }
        }

        public int Height
        {
            set
            {
                height = value;
            }
            get
            {
                return height;
            }
        }

        public string TextureName
        {
            set
            {
                textureName = value;
            }
            get
            {
                return textureName;
            }
        }
    }
}
