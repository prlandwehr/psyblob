#region File Description
//-----------------------------------------------------------------------------
// Pgame.cs
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Soopah.Xna.Input;

namespace PgameNS
{
    /// <summary>
    /// Pgame class. Responsible for Control IO, Rendering, Sound, holds objects for player and current level
    /// </summary>
    public class Pgame : Microsoft.Xna.Framework.Game
    {
        //GAME FIELDS
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private string[] levelFileList = {"level1.txt", "level2.txt", "boss1.txt"};
        //Starting resolution
        private int startingwidth = 800;
        private int startingheight = 600;
        private bool startfullscreen = false;
        private double bgmvolume = 1;
        private double volume = 1;
        //Used for switching resolutions and scaling sprites properly
        //private float lastscale;
        //private float currentscale;
        private double levelTimeElapsed = 0;
        private bool yPressed = false;
        private bool startPressed = false;
        private bool paused = false;
        private bool gamepadConnected = true;
        private double points;
        private string levelname;

        //Game sprites/objects
        //to add - enemy bullet sprites, item sprites, status sprites
        private Player player;
        private ArrayList groundlist;
        private ArrayList enemylist;
        private ArrayList pAttacks;
        private Plevel currentLevel;
        private int currentLevelIndex = 0;
        private Texture2D background;
        private Texture2D loadscreen;
        private Texture2D titlescrn;
        private SpriteFont Font1;
        private Vector2 FontPos;
        private Rectangle screenRect;

        //state enum
        private enum PGameState
        {
            Title = 0,
            IntroScrn = 1,
            Game = 2,
            LevelComplete = 3
        }
        private PGameState gamestate;
        
        //CONSTRUCTOR
        public Pgame()
        {
            //Initialize graphics object and content directory
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set initial screen paramaters
            Vector2 res = LevelReader.readResolution("settings.txt");
            startingwidth = (int)res.X;
            startingheight = (int)res.Y;
            //Console.Out.WriteLine(startingwidth + " " + startingheight);
            startfullscreen = LevelReader.readFullScreen("settings.txt");
            volume = LevelReader.readVolume("settings.txt");
            bgmvolume = LevelReader.readBGMVolume("settings.txt");
            gamepadConnected = LevelReader.readGamepadSetting("settings.txt");

            graphics.PreferredBackBufferHeight = startingheight;
            graphics.PreferredBackBufferWidth = startingwidth;
            //this.lastscale = 1;
            //this.currentscale = 1;
            graphics.IsFullScreen = startfullscreen;

            screenRect = new Rectangle();
            screenRect.X = 0;
            screenRect.Y = 0;
            screenRect.Width = startingwidth;
            screenRect.Height = startingheight;

            //Init attacks;
            pAttacks = new ArrayList();

            gamestate = PGameState.Title;

            //Set initial player, level position
            //levelloc = Vector2.Zero;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        //Graphical content is loaded here
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Set level, init
            currentLevel = new Plevel(levelFileList[currentLevelIndex]);
            currentLevel.initLevel(this.Content, startingwidth, startingheight);

            //Load content from level
            player = currentLevel.getPlayer();
            groundlist = currentLevel.getGround();
            enemylist = currentLevel.getEnemies();
            background = currentLevel.getBackground();

            levelname = currentLevel.getName();
            loadscreen = currentLevel.getLoadScrn();

            //Load TitleScreen
            if (gamestate == PGameState.Title)
            {
                titlescrn = LevelReader.readTitleScrn("title", Content);
            }
            

            //FONT LOAD
            Font1 = Content.Load<SpriteFont>("SpriteFont1");
            FontPos = Vector2.Zero;
            FontPos.Y += 20;

            //Set master volume
            SoundEffect.MasterVolume = (float)volume;
            MediaPlayer.Volume = (float)bgmvolume;

            levelTimeElapsed = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        //Game state update method
        //Current order: Gameover?, Input, updates, collision, scrolling
        protected override void Update(GameTime gameTime)
        {
            //Pause-unpause
            /*if (gamestate == PGameState.Game && PCgamepad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                startPressed = true;
            }
            else if (startPressed)
            {
                startPressed = false;
                if (!paused)
                {
                    paused = true;
                }
                else
                {
                    paused = false;
                }
            }*/

            if (gamestate == PGameState.Game && !paused)
            {
                levelTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

                //Handle player death
                if (player.Plife <= 0)
                {
                    paused = true;
                    player.Plife = 100;
                    processDeath();
                    paused = false;
                }

                //Checks controler/key input and respond as needed
                if (gamepadConnected)
                {
                    processGamepadInput();
                }
                else
                {
                    processInput();
                }

                //Update enemy roster
                //Currently enemies are not dynamically spawned, only removes dead enemies
                foreach (EnemySprite item in enemylist)
                {
                    if (item.Life <= 0 || !(item.isAlive()))
                    {
                        if(item.explodes)
                        {
                            Explosion anExp = new Explosion();
                            anExp.position.X = item.bbox.Center.X - 50;
                            anExp.position.Y = item.bbox.Center.Y - 50;
                            enemylist.Add(anExp);
                        }
                        if (item.islevelboss)
                        {
                            EnemySprite lgoal = currentLevel.getDelayedLevelGoal(this.Content, item.position);
                            enemylist.Add(lgoal);
                        }
                        enemylist.Remove(item);
                        break;
                    }
                }
                ArrayList newAttacks = null;
                foreach (EnemySprite item in enemylist)
                {
                    newAttacks = item.getAttacks(player.position);
                    if (newAttacks != null)
                    {
                        foreach (EnemySprite attack in newAttacks)
                        {
                            enemylist.Add(attack);
                        }
                        break;
                    }
                }

                //Update attacks
                foreach (Attack item in pAttacks)
                {
                    item.Update(gameTime, player);
                }

                //Update player sprite
                player.Update(gameTime, pAttacks);

                //Update enemies
                foreach (EnemySprite item in enemylist)
                {
                    item.Update(gameTime, player.position);
                }

                //Update ground
                foreach (GroundSprite item in groundlist)
                {
                    item.Update(gameTime);
                }

                //Checks for collisions and respond as needed
                processCollision();

                //Scroll level
                processScrolling();

                //Debug - position
                //Console.WriteLine("(" + levelloc.X + "," + levelloc.Y + ")");

                base.Update(gameTime);
            }
            else if (gamestate == PGameState.Title)
            {
                if (gamepadConnected)
                {
                    processGamepadInput();
                }
                else
                {
                    processInput();
                }
            }
            else if (gamestate == PGameState.IntroScrn)
            {
                if (gamepadConnected)
                {
                    processGamepadInput();
                }
                else
                {
                    processInput();
                }
            }
        }

        //Draws each frame
        protected override void Draw(GameTime gameTime)
        {
            //default bg color
            graphics.GraphicsDevice.Clear(Color.White/*CornflowerBlue*/);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            if (gamestate == PGameState.Game)
            {

                //Draw BG
                spriteBatch.Draw(background, screenRect, Color.White);
                //spriteBatch.Draw(background, Vector2.Zero, Color.White);

                //Draw player sprite
                player.draw(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                //draw ground sprites
                foreach (GroundSprite item in groundlist)
                {
                    item.draw(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                }

                //draw enemy sprites
                foreach (EnemySprite item in enemylist)
                {
                    item.draw(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                }

                //draw player attacks
                foreach (Psprite item in pAttacks)
                {
                    item.draw(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                }

                //Draw text
                spriteBatch.DrawString(Font1, "Life: " + player.Plife
                    + "\n" + "Weapon: " + player.cWeapon.ToString()
                    + "\n" + "Lives: " + player.Plives
                    + "\n" + "Time: " + (int)levelTimeElapsed, Vector2.Zero, Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);
                //spriteBatch.DrawString(Font1, "Weapon: " + player.cWeapon.ToString(), FontPos, Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);

            }
            else if (gamestate == PGameState.IntroScrn)
            {
                //Vector2 center = Vector2.Zero;
                //center.X = screenRect.Center.X;
                //center.Y = screenRect.Center.Y;
                spriteBatch.Draw(loadscreen, screenRect, Color.White);
                //spriteBatch.DrawString(Font1, levelname, center, Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);
            }
            else if (gamestate == PGameState.Title)
            {
                spriteBatch.Draw(titlescrn, screenRect, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Scrolls level, current at static 1/3 left 2/3 right ratio
        //effects ground and enemy sprites, should be modified to include enemy attacks
        private void processScrolling()
        {
            float rightbound = (55.0f / 100.0f) * graphics.PreferredBackBufferWidth;
            float leftbound = (45.0f / 100.0f) * graphics.PreferredBackBufferWidth;
            float upbound = (55.0f / 100.0f) * graphics.PreferredBackBufferHeight;
            float downbound = (65.0f / 100.0f) * graphics.PreferredBackBufferHeight;
            float pdeltaR = player.position.X - rightbound;
            float pdeltaL = player.position.X - leftbound;
            float pdeltaU = player.position.Y - upbound;
            float pdeltaD = player.position.Y - downbound;
            //scroll right
            if (pdeltaR > 0)
            {
                //levelloc.X += pdeltaR;
                player.position.X = rightbound;
                player.bbox.X = (int)rightbound;
                foreach (GroundSprite item in groundlist) 
                {
                    item.position.X -= pdeltaR;
                    item.bbox.X -= (int)pdeltaR;
                }
                foreach (EnemySprite item in enemylist)
                {
                    item.position.X -= pdeltaR;
                    item.bbox.X -= (int)pdeltaR;
                }
                foreach (Attack item in pAttacks)
                {
                    if (item is ShotAttack)
                    {
                        item.position.X -= pdeltaR;
                        item.bbox.X -= (int)pdeltaR;
                    }
                }
            }
            else if (pdeltaL < 0) 
            {
                //levelloc.X += pdeltaL;
                player.position.X = leftbound;
                player.bbox.X = (int)leftbound;
                foreach (GroundSprite item in groundlist) 
                {
                    item.position.X -= pdeltaL;
                    item.bbox.X -= (int)pdeltaL;
                }
                foreach (EnemySprite item in enemylist)
                {
                    item.position.X -= pdeltaL;
                    item.bbox.X -= (int)pdeltaL;
                }
                foreach (Attack item in pAttacks)
                {
                    if (item is ShotAttack)
                    {
                        item.position.X -= pdeltaL;
                        item.bbox.X -= (int)pdeltaL;
                    }
                }
            }
            //scroll up/down
            if (pdeltaD > 0)
            {
                //levelloc.Y += pdeltaD;
                player.position.Y = downbound;
                player.bbox.Y = (int)downbound;
                foreach (GroundSprite item in groundlist)
                {
                    item.position.Y -= pdeltaD;
                    item.bbox.Y -= (int)pdeltaD;
                }
                foreach (EnemySprite item in enemylist)
                {
                    item.position.Y -= pdeltaD;
                    item.bbox.Y -= (int)pdeltaD;
                }
                foreach (Attack item in pAttacks)
                {
                    if (item is ShotAttack)
                    {
                        item.position.Y -= pdeltaD;
                        item.bbox.Y -= (int)pdeltaD;
                    }
                }
            }
            else if (pdeltaU < 0)
            {
                //levelloc.Y += pdeltaU;
                player.position.Y = upbound;
                player.bbox.Y = (int)upbound;
                foreach (GroundSprite item in groundlist)
                {
                    item.position.Y -= pdeltaU;
                    item.bbox.Y -= (int)pdeltaU;
                }
                foreach (EnemySprite item in enemylist)
                {
                    item.position.Y -= pdeltaU;
                    item.bbox.Y -= (int)pdeltaU;
                }
                foreach (Attack item in pAttacks)
                {
                    if (item is ShotAttack)
                    {
                        item.position.Y -= pdeltaU;
                        item.bbox.Y -= (int)pdeltaU;
                    }
                }
            }

        }

        //Process collisions between player and ground/platform objects
        //needs to be modified to handle attacks/enemies
        private void processCollision()
        {
            //Ground collision section
            foreach (GroundSprite item in groundlist)
            {
                if (player.bbox.Intersects(item.bbox))
                {
                    double y = player.bbox.Center.Y - item.bbox.Center.Y;
                    double x = player.bbox.Center.X - item.bbox.Center.X;
                    double intAngle = Math.Atan(y/x);
                    double gx = item.bbox.Right - item.bbox.Center.X;
                    double gy = item.bbox.Top - item.bbox.Center.Y;
                    double gAngle = Math.Atan(gy / gx);
                    //COLLISION WITH TOP HALF OF GROUND
                    if (y < 0)
                    {
                        //if on top of ground
                        if (Math.Abs(intAngle) >= Math.Abs(gAngle) )
                        {
                            //If falling stop falling
                            if (player.velocity.Y > 0)
                            {
                                player.velocity.Y = 0;
                            }
                            player.position.Y = item.bbox.Top - player.bbox.Height + 1;
                            player.jumpsReset();
                        }
                        //If to the side of ground
                        else
                        {
                            if (player.position.X < item.position.X)
                            {
                                player.position.X = item.bbox.Left - player.bbox.Width;
                            }
                            else
                            {
                                player.position.X = item.bbox.Right;
                            }
                        }
                    }
                    //COLLISION WITH BOTTOM HALF OF GROUND
                    else if(y > 0)
                    {
                        //If bellow the ground
                        if (Math.Abs(intAngle) >= Math.Abs(gAngle) )
                        {
                            //If moving up, stop moving up
                            if (player.velocity.Y < 0)
                            {
                                player.velocity.Y = 0;
                            }
                            player.position.Y = item.bbox.Bottom + 1;
                        }
                        //If to the side of ground
                        else
                        {
                            if (player.position.X < item.position.X)
                            {
                                player.position.X = item.bbox.Left - player.bbox.Width;
                            }
                            else
                            {
                                player.position.X = item.bbox.Right;
                            }
                        }
                    }
                }
            }
            //Enemy loop
            foreach (EnemySprite item in enemylist)
            {
                //Attack+enemy collision here
                foreach (Attack attack in pAttacks)
                {
                    if (attack.bbox.Intersects(item.bbox))
                    {
                        //item.Life -= attack.Damage;
                        item.enemyHit(attack.Damage);
                        //If bullet hits despawn it
                        if (attack is ShotAttack && item.shootable)
                        {
                            pAttacks.Remove(attack);
                            break;
                        }
                        //Console.WriteLine("Hit for " + attack.Damage);
                    }
                }
                //Player+enemy collision here
                if (player.hitbox.Intersects(item.bbox))
                {
                    if (item is LevelGoal)
                    {
                        levelComplete();
                    }
                    else if(item is Booster)
                    {
                        Booster boost = (Booster)item;
                        player.setBoost(boost.Bduration, boost.Bvelocity);
                    }
                    else
                    {
                        player.playerHit(item.CollisionDmg);
                    }
                    //player.Plife -= item.CollisionDmg;
                }
            }
        }

        //Gets player input and calls appropriate functions
        private void processInput()
        {
            //Current Keyboard state
            KeyboardState keys = Keyboard.GetState();
            GamePadState xboxpad = GamePad.GetState(PlayerIndex.One);

            if (gamestate == PGameState.Game)
            {
                // Allows the game to exit manually
                if (xboxpad.Buttons.Back == ButtonState.Pressed
                    || keys.IsKeyDown(Keys.Escape))
                {
                    UnloadContent();
                    this.Exit();
                }

                //Movement processing section
                if (keys.IsKeyDown(Keys.Right) || xboxpad.DPad.Right == ButtonState.Pressed)
                {
                    player.rightPressed();
                }
                else if (keys.IsKeyDown(Keys.Left) || xboxpad.DPad.Left == ButtonState.Pressed)
                {
                    player.leftPressed();
                }
                else
                {
                    player.moveNotPressed();
                }

                //Jump processing section
                if (keys.IsKeyDown(Keys.Space) || xboxpad.Buttons.A == ButtonState.Pressed)
                {
                    player.jumpPressed();
                }
                else
                {
                    player.jumpNotPressed();
                }

                //Attack processing section
                if (keys.IsKeyDown(Keys.Z) || xboxpad.Buttons.X == ButtonState.Pressed)
                {
                    Psprite attack = player.attackPressed();
                    if (attack != null)
                    {
                        pAttacks.Add(attack);
                    }
                }
                //Weapon switch
                if (keys.IsKeyDown(Keys.X) || xboxpad.Buttons.Y == ButtonState.Pressed)
                {
                    if (!yPressed)
                    {
                        if (player.cWeapon == Player.Weapon.EnergyShot)
                        {
                            player.cWeapon = Player.Weapon.Pyrokinesis;
                        }
                        else if (player.cWeapon == Player.Weapon.Pyrokinesis)
                        {
                            player.cWeapon = Player.Weapon.EnergyShot;
                        }
                    }
                    yPressed = true;
                }
                else
                {
                    yPressed = false;
                }

                //Screen Resize Block
                if (keys.IsKeyDown(Keys.F1))
                {
                    scaleScreen(800, 600, false);
                }
                else if (keys.IsKeyDown(Keys.F2))
                {
                    scaleScreen(1024, 768, false);
                }
                else if (keys.IsKeyDown(Keys.F3))
                {
                    scaleScreen(640, 480, false);
                }
                else if (keys.IsKeyDown(Keys.F4))
                {
                    scaleScreen(1680, 1050, true);
                }
            }
            else if (gamestate == PGameState.IntroScrn)
            {
                currentLevel.playIntroBGM();
                if (keys.IsKeyDown(Keys.Enter))
                {
                    startPressed = true;
                }
                else if (startPressed)
                {
                    startPressed = false;
                    gamestate = PGameState.Game;
                    currentLevel.playBGM();
                }
            }
            else if (gamestate == PGameState.Title)
            {
                if (keys.IsKeyDown(Keys.Enter))
                {
                    startPressed = true;
                }
                else if (startPressed)
                {
                    startPressed = false;
                    gamestate = PGameState.IntroScrn;
                }
            }
        }

        private void processGamepadInput()
        {
            //Current Keyboard state
            KeyboardState keys = Keyboard.GetState();
            PCgamepad.MyGamePadState pad = PCgamepad.GetState(PlayerIndex.One);
            GamePadState xboxpad = GamePad.GetState(PlayerIndex.One);

            if (gamestate == PGameState.Game)
            {
                // Allows the game to exit manually
                if (xboxpad.Buttons.Back == ButtonState.Pressed
                    || keys.IsKeyDown(Keys.Escape)
                    || pad.Buttons.Select == ButtonState.Pressed)
                {
                    UnloadContent();
                    this.Exit();
                }

                //Movement processing section
                if (keys.IsKeyDown(Keys.Right) || xboxpad.DPad.Right == ButtonState.Pressed
                    || pad.DPad.Right == ButtonState.Pressed)
                {
                    player.rightPressed();
                }
                else if (keys.IsKeyDown(Keys.Left) || xboxpad.DPad.Left == ButtonState.Pressed
                    || pad.DPad.Left == ButtonState.Pressed)
                {
                    player.leftPressed();
                }
                else
                {
                    player.moveNotPressed();
                }

                //Jump processing section
                if (keys.IsKeyDown(Keys.Space) || xboxpad.Buttons.A == ButtonState.Pressed
                    || pad.Buttons.Cross == ButtonState.Pressed)
                {
                    player.jumpPressed();
                }
                else
                {
                    player.jumpNotPressed();
                }

                //Attack processing section
                if (keys.IsKeyDown(Keys.Z) || xboxpad.Buttons.X == ButtonState.Pressed
                    || pad.Buttons.Square == ButtonState.Pressed)
                {
                    Psprite attack = player.attackPressed();
                    if (attack != null)
                    {
                        pAttacks.Add(attack);
                    }
                }
                //Weapon switch
                if (keys.IsKeyDown(Keys.X) || xboxpad.Buttons.Y == ButtonState.Pressed
                    || pad.Buttons.Triangle == ButtonState.Pressed)
                {
                    if (!yPressed)
                    {
                        if (player.cWeapon == Player.Weapon.EnergyShot)
                        {
                            player.cWeapon = Player.Weapon.Pyrokinesis;
                        }
                        else if (player.cWeapon == Player.Weapon.Pyrokinesis)
                        {
                            player.cWeapon = Player.Weapon.EnergyShot;
                        }
                    }
                    yPressed = true;
                }
                else
                {
                    yPressed = false;
                }

                //Screen Resize Block
                if (keys.IsKeyDown(Keys.F1))
                {
                    scaleScreen(800, 600, false);
                }
                else if (keys.IsKeyDown(Keys.F2))
                {
                    scaleScreen(1024, 768, false);
                }
                else if (keys.IsKeyDown(Keys.F3))
                {
                    scaleScreen(640, 480, false);
                }
                else if (keys.IsKeyDown(Keys.F4))
                {
                    scaleScreen(1680, 1050, true);
                }
            }
            else if (gamestate == PGameState.IntroScrn)
            {
                currentLevel.playIntroBGM();
                if (pad.Buttons.Start == ButtonState.Pressed)
                {
                    startPressed = true;
                }
                else if (startPressed)
                {
                    startPressed = false;
                    gamestate = PGameState.Game;
                    currentLevel.playBGM();
                }
            }
            else if (gamestate == PGameState.Title)
            {
                if (pad.Buttons.Start == ButtonState.Pressed)
                {
                    startPressed = true;
                }
                else if (startPressed)
                {
                    startPressed = false;
                    gamestate = PGameState.IntroScrn;
                }
            }
        }

        //Redraws the screen at new size based on new and current multiple of the base resolution
        private void scaleScreen(int width, int height, bool fullscrn)
        {
            paused = true;
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            //lastscale = newscale;
            //currentscale = newscale;

            screenRect.X = 0;
            screenRect.Y = 0;
            screenRect.Width = graphics.PreferredBackBufferWidth;
            screenRect.Height = graphics.PreferredBackBufferHeight;

            graphics.IsFullScreen = fullscrn;

            graphics.ApplyChanges();
            paused = false;
        }

        private void processDeath()
        {
            Console.WriteLine(player.Plives);
            player.Plives -= 1;
            //used to set lives to correct value after reset
            float lives = player.Plives;
            if (player.Plives <= 0)
            {
                currentLevel.stopBGM();
                gamestate = PGameState.Title;
                currentLevelIndex = 0;
                UnloadContent();
                LoadContent();
            }
            else
            {
                UnloadContent();
                LoadContent();
                player.Plives = lives;
                currentLevel.playBGM();
            }
        }

        private void levelComplete()
        {
            currentLevel.stopBGM();
            currentLevelIndex++;
            if (currentLevelIndex == levelFileList.Length)
            {
                gamestate = PGameState.Title;
                currentLevelIndex = 0;
            }
            else
            {
                gamestate = PGameState.IntroScrn;
            }
            UnloadContent();
            LoadContent();
        }
    }
}
