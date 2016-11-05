using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PgameNS
{
    //The sprite superclass, has basic fields and physics functions
    public abstract class Psprite
    {
        //************ lasposition and appearloc are currently unused *****************//
        //Position vector for sprite
        public Vector2 position;
        //Last position of sprite. Currently unusued
        public Vector2 lastposition;
        //Velocity vector
        public Vector2 velocity;
        //Acceleration vector
        public Vector2 acceleration;
        //Scale for changing vectors based on screen size
        private float scale;
        //The amount of time in seconds the sprite is slated to exist
        //-1 to indicate infinite;
        private double lifetime;
        //The time the sprite has existed thus far
        private double lived;
        //Is the sprite alive
        private bool alive;
        //Where in a level the sprite appears
        public Vector2 appearloc;
        //
        public enum Facing
        {
            Left = 0,
            Right = 1
        }
        public Facing facing;
        //bounding box for collisions
        public Rectangle bbox;

        //Default constructor
        public Psprite()
        {
            this.position = new Vector2(0, 0);
            this.lastposition = new Vector2(0, 0);
            this.velocity = new Vector2(0, 0);
            this.acceleration = new Vector2(0, 0);
            this.appearloc = new Vector2(0, 0);
            this.lifetime = -1;
            this.lived = 0;
            this.alive = true;
            this.scale = 1;
            this.bbox = new Rectangle();
        }

        /*public Psprite(double l)
            : this()
        {
            this.lifetime = l;
        }*/

        //Settings based constructor
        public Psprite(Vector2 pos, Vector2 vel, Vector2 acc, double l)
        {
            this.position = pos;
            this.lastposition.X = pos.X;
            this.lastposition.Y = pos.Y;
            this.velocity = vel;
            this.acceleration = acc;
            this.appearloc = new Vector2(0, 0);
            this.lifetime = l;
            this.lived = 0;
            this.alive = true;
            this.scale = 1;
        }

        //Update the sprite based on time passed since last update
        public virtual void Update(GameTime gtime)
        {
            //update position and velocity
            lastposition.X = position.X;
            lastposition.Y = position.Y;
            position += velocity * (float)gtime.ElapsedGameTime.TotalSeconds;
            velocity += acceleration * (float)gtime.ElapsedGameTime.TotalSeconds;

            //Modify time since creation and check if expired
            lived += gtime.ElapsedGameTime.TotalSeconds;
            if (lifetime != -1 && lived > lifetime)
            {
                alive = false;
            }
        }

        //Method for rendering, abstract
        public abstract void draw(SpriteBatch spriteBatch, float swidth, float sheight);

        //get+set methods

        public float Pscale
        {
            set
            {
                scale = value;
            }
            get
            {
                return scale;
            }
        }

        /*public void setLife(double l)
        {
            lifetime = l;
        }*/

        public bool isAlive()
        {
            return alive;
        }

        
    }
}
