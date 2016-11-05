using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace PgameNS
{
    //Level class to be implimented by game's levels
    public class Plevel
    {
        //Level fields
        protected ArrayList enemies;
        protected ArrayList ground;
        protected Player player;
        protected Texture2D background;
        protected string levelFile;
        protected string levelname;
        protected Texture2D loadscreen;
        protected Song levelbgm;
        protected Song levelintrobgm;
        protected SoundEffectInstance activebgm;
        protected bool introplayed = false;
        //public bool levelComplete = false;


        public Plevel(string levelPath)
        {
            this.enemies = new ArrayList();
            this.ground = new ArrayList();
            this.player = new Player();
            this.levelFile = levelPath;
        }

        public void addGround(GroundSprite gsprite)
        {
            ground.Add(gsprite);
        }

        public void addEnemy(EnemySprite esprite)
        {
            enemies.Add(esprite);
        }

        public ArrayList getGround()
        {
            return ground;
        }

        public ArrayList getEnemies()
        {
            return enemies;
        }

        public Player getPlayer()
        {
            return player;
        }

        public Texture2D getBackground()
        {
            return background;
        }

        public Texture2D getLoadScrn()
        {
            return loadscreen;
        }

        public string getName()
        {
            return levelname;
        }

        public EnemySprite getDelayedLevelGoal(ContentManager content, Vector2 position)
        {
            return LevelReader.readDelayedLevelGoal(levelFile, content, position);
        }

        public void initLevel(ContentManager content, int sWidth, int sHeight)
        {
            //ContentManager.RootDirectory = "Content";

            LevelReader.setPlayer(levelFile, content, player, sWidth, sHeight);

            ground = LevelReader.readGround(levelFile, content, sWidth, sHeight);

            enemies = LevelReader.readEnemies(levelFile, content, sWidth, sHeight);

            background = LevelReader.readBackground(levelFile, content);

            loadscreen = LevelReader.readLevelLoadScrn(levelFile, content);

            levelname = LevelReader.readLevelName(levelFile);

            levelbgm = LevelReader.readLevelBGM(levelFile, content);

            levelintrobgm = LevelReader.readLevelIntroBGM(levelFile, content);

            //init explosion sprite to be used by dead enemies
            Explosion aexplosion = new Explosion();
            Texture2D[] exparray = { content.Load<Texture2D>("explode1"),
                                     content.Load<Texture2D>("explode2"),
                                     content.Load<Texture2D>("explode3"),
                                     content.Load<Texture2D>("explode4")};
            aexplosion.setTextures(exparray);

        }

        public string LevelPath
        {
            get
            {
                return levelFile;
            }
        }

        public void playBGM()
        {
            //MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(levelbgm);
            //activebgm = levelbgm.Play(1.0f, 0f, 0f, true);
        }

        public void playIntroBGM()
        {
            if (!introplayed)
            {
                //MediaPlayer.Volume = 0.5f;
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Play(levelintrobgm);
                introplayed = true;
            }
        }

        public void stopBGM()
        {
            MediaPlayer.Stop();
            //activebgm.Stop();
            //activebgm.Dispose();
        }

        //******* No longer used *******//
        //create sprite objects needed for level
        //public abstract void initLevel(ContentManager content, int sWidth, int sHeight);

        //return names of graphics files needed for level
        //public abstract ArrayList getTextureNames();

        //set sprites graphics based on name/texture table
        //public abstract void setTextures(Dictionary<string, Texture2D> contentNames);

        //return names of sound files needed for level
        //public abstract ArrayList getSoundNames();

        //set sprites sounds based on name/sound table
        //public abstract void setSounds(Dictionary<string, SoundEffect> contentNames);
    }
}
