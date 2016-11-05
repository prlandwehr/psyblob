using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace PgameNS
{
    public static class LevelReader
    {

        /* Player txt format:
         * *Player*
         * x
         * y
         * sprite-right[0] name
         * sprite-right[1] name
         * sprite-left[0] name
         * sprite-left[1] name
         * jumpr name
         * jumpl name
         * slash r frame 0 name
         * slash r frame 1 name
         * slash l frame 0 name
         * slash l frame 1 name
         * shot [0]
         * shot [1]
         * jump SOUND name
         * slash SOUND name
         * shot SOUND name
         */
        public static void setPlayer(string fPath, ContentManager content, /*SpriteBatch sprites,*/ Player player, int sWidth, int sHeight)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*Player*"))
                {
                    //Read player position
                    float px = (float)Convert.ToDouble(sr.ReadLine());
                    player.position.X = px;
                    
                    float py = (float)Convert.ToDouble(sr.ReadLine());
                    player.position.Y = sHeight - py;

                    //Read player sprites
                    Texture2D[] sprR = { content.Load<Texture2D>(sr.ReadLine()), content.Load<Texture2D>(sr.ReadLine()) };
                    Texture2D[] sprL = { content.Load<Texture2D>(sr.ReadLine()), content.Load<Texture2D>(sr.ReadLine()) };
                    //Texture2D sprR = content.Load<Texture2D>(sr.ReadLine());
                    //Texture2D sprL = content.Load<Texture2D>(sr.ReadLine());

                    Texture2D jumpR = content.Load<Texture2D>(sr.ReadLine());
                    Texture2D jumpL = content.Load<Texture2D>(sr.ReadLine());

                    Texture2D[] slashR = { content.Load<Texture2D>(sr.ReadLine()), content.Load<Texture2D>(sr.ReadLine()) };
                    Texture2D[] slashL = { content.Load<Texture2D>(sr.ReadLine()), content.Load<Texture2D>(sr.ReadLine()) };

                    Texture2D shotR = content.Load<Texture2D>(sr.ReadLine());
                    Texture2D shotL = content.Load<Texture2D>(sr.ReadLine());

                    //Read player sounds
                    SoundEffect jumpSound = content.Load<SoundEffect>(sr.ReadLine());
                    SoundEffect slashSound = content.Load<SoundEffect>(sr.ReadLine());
                    SoundEffect shotSound = content.Load<SoundEffect>(sr.ReadLine());

                    //set player sprites/sound
                    player.setBaseSprite(sprR, sprL);
                    player.setJumpSprite(jumpR, jumpL);
                    player.Jumpsound = jumpSound;
                    //set attack sprites/sound
                    SlashAttack s1 = new SlashAttack();
                    s1.setBaseSprite(slashR, slashL);
                    s1.Sound = slashSound;
                    ShotAttack s2 = new ShotAttack();
                    s2.setBaseSprite(shotR, shotL);
                    s2.Sound = shotSound;
                }
            }
            sr.Close();
            file.Close();
        }

        /* GroundSprite txt format
         * *GroundSprite*
         * width multiplier
         * height multiplier
         * x
         * y
         * spritename
         */
        public static ArrayList readGround(string fPath, ContentManager content, int sWidth, int sHeight)
        {
            ArrayList groundlist = new ArrayList();
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*GroundSprite*"))
                {
                    int tilex;
                    int tiley;
                    float gx;
                    float gy;

                    tilex = Convert.ToInt32(sr.ReadLine());
                    tiley = Convert.ToInt32(sr.ReadLine());
                    GroundSprite gs = new GroundSprite(tilex, tiley);

                    gx = (float)Convert.ToDouble(sr.ReadLine());
                    gy = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D groundTex = content.Load<Texture2D>(sr.ReadLine());

                    gs.position.X = gx;
                    gs.position.Y = sHeight - gy;
                    gs.setBaseSprite(groundTex);
                    groundlist.Add(gs);
                }
            }

            sr.Close();
            file.Close();
            return groundlist;
        }

        /* BasicEnemy txt format
         * *BasicEnemy*
         * x
         * y
         * enemysprite
         * 
         * Spikes format
         * *Spikes*
         * width multiplier
         * x
         * y
         * sprite
         * 
         * Turret format
         * *EnemyTurret*
         * x
         * y
         * sprite
         * shotsprite[4]
         * 
         * *EnemyWalker*
         * x
         * y
         * dx
         * dy
         * sprite
         * 
         * *LevelGoal*
         * x
         * y
         * sprite[4]
         */
        public static ArrayList readEnemies(string fPath, ContentManager content, int sWidth, int sHeight)
        {
            ArrayList enemylist = new ArrayList();
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*BasicEnemy*"))
                {
                    float ex;
                    float ey;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());

                    BasicEnemy be = new BasicEnemy();
                    be.position.X = ex;
                    be.position.Y = sHeight - ey;
                    be.setBaseSprite(eTex);
                    enemylist.Add(be);
                }
                else if (line.Contains("*Booster*"))
                {
                    float ex;
                    float ey;
                    float vx;
                    float vy;
                    float duration;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    vx = (float)Convert.ToDouble(sr.ReadLine());
                    vy = (float)Convert.ToDouble(sr.ReadLine());
                    duration = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D[] eTex = {content.Load<Texture2D>(sr.ReadLine())};

                    Booster boost = new Booster();
                    boost.position.X = ex;
                    boost.position.Y = sHeight - ey;
                    Vector2 boostvel = new Vector2(vx,vy);
                    boost.setBoost(duration, boostvel);
                    boost.setTextures(eTex);
                    enemylist.Add(boost);
                    //Console.Out.WriteLine(vy);
                }
                else if (line.Contains("*Spikes*"))
                {
                    int width;
                    float ex;
                    float ey;

                    width = Convert.ToInt32(sr.ReadLine());
                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());

                    Spikes spike = new Spikes(width);
                    spike.position.X = ex;
                    spike.position.Y = sHeight - ey;
                    spike.setBaseSprite(eTex);
                    enemylist.Add(spike);
                }
                else if (line.Contains("*EnemyTurret*"))
                {
                    float ex;
                    float ey;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());
                    Texture2D[] shotTex = new Texture2D[4];

                    for (int index = 0; index < shotTex.Length; index++)
                    {
                        shotTex[index] = content.Load<Texture2D>(sr.ReadLine());
                    }

                    //EnemyShot es = new EnemyShot();
                    //es.setBaseSprite(shotTex);

                    EnemyTurret be = new EnemyTurret();
                    be.ShotTex = shotTex;
                    be.position.X = ex;
                    be.position.Y = sHeight - ey;
                    be.setBaseSprite(eTex);
                    enemylist.Add(be);
                }
                else if (line.Contains("*EnemyWalker*"))
                {
                    float ex;
                    float ey;
                    float dx;
                    float dy;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    dx = (float)Convert.ToDouble(sr.ReadLine());
                    dy = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());

                    EnemyWalker be = new EnemyWalker(dx, dy);
                    be.position.X = ex;
                    be.position.Y = sHeight - ey;
                    be.setBaseSprite(eTex);
                    enemylist.Add(be);
                }
                else if (line.Contains("*LevelGoal*"))
                {
                    float ex;
                    float ey;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    //Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());
                    Texture2D[] eTex = new Texture2D[4];
                    for (int index = 0; index < eTex.Length; index++)
                    {
                        eTex[index] = content.Load<Texture2D>(sr.ReadLine());
                    }

                    LevelGoal be = new LevelGoal();
                    be.position.X = ex;
                    be.position.Y = sHeight - ey;
                    be.setTextures(eTex);
                    enemylist.Add(be);
                }
                else if (line.Contains("*Boss1*"))
                {
                    float ex;
                    float ey;

                    ex = (float)Convert.ToDouble(sr.ReadLine());
                    ey = (float)Convert.ToDouble(sr.ReadLine());
                    Texture2D eTex = content.Load<Texture2D>(sr.ReadLine());
                    Texture2D[] shotTex = new Texture2D[4];

                    for (int index = 0; index < shotTex.Length; index++)
                    {
                        shotTex[index] = content.Load<Texture2D>(sr.ReadLine());
                    }

                    //EnemyShot es = new EnemyShot();
                    //es.setBaseSprite(shotTex);

                    Boss1 boss = new Boss1();
                    boss.ShotTex = shotTex;
                    boss.position.X = ex;
                    boss.position.Y = sHeight - ey;
                    boss.setBaseSprite(eTex);
                    enemylist.Add(boss);
                }
            }

            sr.Close();
            file.Close();
            return enemylist;
        }

        /*
         * *DelayedLevelGoal*
         * texture[4]
         * 
         */
        public static EnemySprite readDelayedLevelGoal(string fPath, ContentManager content, Vector2 position)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*DelayedLevelGoal*"))
                {
                    Texture2D[] eTex = new Texture2D[4];
                    for (int index = 0; index < eTex.Length; index++)
                    {
                        eTex[index] = content.Load<Texture2D>(sr.ReadLine());
                    }

                    LevelGoal be = new LevelGoal();
                    be.position = position;
                    be.setTextures(eTex);
                    return be;
                }
            }
            return null;
        }

        /*
         * *Background*
         * bgspritename
         * 
         */ 
        public static Texture2D readBackground(string fPath, ContentManager content)
        {
            Texture2D background = null;
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*Background*"))
                {
                    background = content.Load<Texture2D>(sr.ReadLine());
                }
            }

            sr.Close();
            file.Close();
            return background;
        }

        /*
         * *LevelBGM*
         * name
         */
        public static Song readLevelBGM(string fPath, ContentManager content)
        {
            Song bgm = null;
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*LevelBGM*"))
                {
                    bgm = content.Load<Song>(sr.ReadLine());
                }
            }

            sr.Close();
            file.Close();
            return bgm;
        }

        /*
         * *LevelIntroBGM*
         * name
         */
        public static Song readLevelIntroBGM(string fPath, ContentManager content)
        {
            Song bgm = null;
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*LevelIntroBGM*"))
                {
                    bgm = content.Load<Song>(sr.ReadLine());
                }
            }

            sr.Close();
            file.Close();
            return bgm;
        }

        /*
         * *LevelID*
         * texture
         * 
         */ 
        public static Texture2D readLevelLoadScrn(string fPath, ContentManager content)
        {
            Texture2D background = null;
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*LevelID*"))
                {
                    line = sr.ReadLine();
                    background = content.Load<Texture2D>(sr.ReadLine());
                }
            }

            sr.Close();
            file.Close();
            return background;
        }

        /*
         * filename
         * 
         */ 
        public static Texture2D readTitleScrn(string fName, ContentManager content)
        {
            Texture2D background = null;
            background = content.Load<Texture2D>(fName);
            
            return background;
        }

        /*
         * *LevelID*
         * loadscrntexture
         * levelname
         */ 
        public static string readLevelName(string fPath)
        {
            string levelname = "";
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*LevelID*"))
                {
                    levelname = sr.ReadLine();
                }
            }

            sr.Close();
            file.Close();
            return levelname;
        }

        public static Vector2 readResolution(string fPath)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;

            Vector2 res = new Vector2();

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*ScreenRes*"))
                {
                    float width = (float)Convert.ToDouble(sr.ReadLine());
                    float height = (float)Convert.ToDouble(sr.ReadLine());
                    res.X = width;
                    res.Y = height;
                }
            }
            sr.Close();
            file.Close();
            return res;
        }

        public static bool readFullScreen(string fPath)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;

            bool fs = false;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*FullScreen*"))
                {
                    line = sr.ReadLine();
                    if (Convert.ToInt32(line) != 0)
                    {
                        fs = true;
                    }
                    else
                    {
                        fs = false;
                    }
                }
            }
            sr.Close();
            file.Close();
            return fs;
        }

        public static double readVolume(string fPath)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;

            double vol = 1;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*SFXVolume*"))
                {
                    line = sr.ReadLine();
                    vol = Convert.ToDouble(line);
                }
            }
            sr.Close();
            file.Close();
            return vol;
        }

        public static double readBGMVolume(string fPath)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;

            double vol = 1;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*BGMVolume*"))
                {
                    line = sr.ReadLine();
                    vol = Convert.ToDouble(line);
                }
            }
            sr.Close();
            file.Close();
            return vol;
        }

        public static bool readGamepadSetting(string fPath)
        {
            FileStream file = new FileStream(fPath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string line;

            bool fs = false;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("*Gamepad*"))
                {
                    line = sr.ReadLine();
                    if (Convert.ToInt32(line) != 0)
                    {
                        fs = true;
                    }
                    else
                    {
                        fs = false;
                    }
                }
            }
            sr.Close();
            file.Close();
            return fs;
        }
    }
}
