using CrazyFour.Core.Factories;
using CrazyFour.Core.Helpers;
using CrazyFour.Core.Lasers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Actors.Hero
{
    public class Player : IActor
    {
        private float speed;
        private float initCounter = 5f;
        private float counter = 0.5f;
        
        public ConfigReader confReader = new ConfigReader();
       

        // I'd say that firing should be automatic
        private bool isFiring = true;
        private bool autoFire = true;
        private bool toggler = false;

        public int Lives { get; set; }

        public DateTime hitTime { get; set; }

        public Player(GraphicsDeviceManager g, SpriteBatch s, ContentManager c)
        {
            Config config = confReader.ReadJson();
            Lives = config.LIVES;
            graphics = g;
            spriteBatch = s;
            content = c;

            radius = 15;

            // defining the default speed
            speed = 4 * GameController.hz;
            laserDirection = new Vector2(0, -1);

            LoadSprite(LoadType.Ship, config.PLAYER_SPRITE);
        }

        public Vector2 GetPlayerPosition()
        { 
            return position; 
        }

        public Vector2 GetPlayerTruePosition()
        {
            Vector2 pos = GetPlayerPosition();
            Vector2 nPos = new Vector2();

            nPos.X = pos.X + GetSprite().Width / 2;
            nPos.Y = pos.Y + GetSprite().Height / 2;

            return nPos;
        }

        public bool SetDefaultPosition()
        {
            currentPosition = defaultPosition;
            position = defaultPosition;
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!inGame)
            {
                defaultPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - (int)(GetSprite().Width / 2), graphics.PreferredBackBufferHeight - GetSprite().Height);
                spriteBatch.Draw(GetSprite(), defaultPosition, Color.White);
                inGame = true;
                position = defaultPosition;
            }
            else
                spriteBatch.Draw(GetSprite(), new Vector2(position.X, position.Y), Color.White);
        }

        public override void Update(GameTime gameTime, Vector2? pp)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // use controlling the speed of the game by pressing the S key
            if (kState.IsKeyDown(Keys.S))
                speed = Utilities.ConvertToPercentage(Speed.QuarterSpeed) * GameController.hz;
            else
                speed = Utilities.ConvertToPercentage(Speed.HalfSpeed) * GameController.hz;

            // Moving the player
            if (kState.IsKeyDown(Keys.Right) && position.X < graphics.PreferredBackBufferWidth + 1 - GetSprite().Width)
                position.X += 3f * speed * dt;

            if (kState.IsKeyDown(Keys.Left) && position.X > 0)
                position.X -= 3f * speed * dt;

            if (kState.IsKeyDown(Keys.Down) && position.Y < graphics.PreferredBackBufferHeight + 1 - GetSprite().Height)
                position.Y += 3f * speed * dt;

            if (kState.IsKeyDown(Keys.Up) && position.Y > (graphics.PreferredBackBufferHeight / 2))
                position.Y -= 3f * speed * dt;
            

            if (kState.IsKeyDown(Keys.K) && toggler == false)
            {
                if (autoFire == true)
                {
                    autoFire = false;
                    toggler = true;
                }
                else
                {
                    autoFire = true;
                    toggler = true;
                }
            }

            if (kState.IsKeyUp(Keys.K))
            {
                toggler = false;
            }
            
            // Firing projectile but making sure we fire only one at a time
            if (!autoFire)
            {
                if (!isFiring)
                {
                    if (kState.IsKeyDown(Keys.Space))
                    {
                        isFiring = true;
                        FireLaser(gameTime);
                    }
                }

                //releasing the flag once we fire one
                if (kState.IsKeyUp(Keys.Space))
                {
                    isFiring = false;
                }
            }
            else
            {
                counter -= dt;
                if (counter <= 0)
                {
                    FireLaser(gameTime);
                    counter = initCounter / 10;
                }
            }

            //detecting pressing 1-5 keys to switch laser type
            if (kState.IsKeyDown(Keys.D1))
            {
                SetLaserMode(LaserMode.Single);
            } else if (kState.IsKeyDown(Keys.D2) && Config.PLAYER_AVAILABLE_LASERS >= 2)
            {
                SetLaserMode(LaserMode.Double);
            }
            else if (kState.IsKeyDown(Keys.D3) && Config.PLAYER_AVAILABLE_LASERS >= 3)
            {
                SetLaserMode(LaserMode.Triple);
            }
            else if (kState.IsKeyDown(Keys.D4) && Config.PLAYER_AVAILABLE_LASERS >= 4)
            {
                SetLaserMode(LaserMode.Cricle);
            }
            else if (kState.IsKeyDown(Keys.D5) && Config.PLAYER_AVAILABLE_LASERS >= 5)
            {
                SetLaserMode(LaserMode.Cone);
            }
        }

        protected override void CreateLaser(Vector2 pos, Vector2 dir, GameTime gameTime)
        {
            LaserFactory factory = new LaserFactory(graphics, spriteBatch, content);
            ILaser lazer = factory.GetPlayerLaser(pos, dir, gameTime);
            LaserController.AddLaser(lazer);
        }
    }
}
