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

namespace CrazyFour.Core.Actors.Enemy
{
    public class Soldier : IActor
    {
        private float speed;
        private float initCounter = 10f;
        private float counter = 0.5f;
        private bool returning = false;
        private Vector2 returnPosition;
        Config config;
        public ConfigReader confReader = new ConfigReader();

        public Soldier(GraphicsDeviceManager g, SpriteBatch s, ContentManager c)
        {
            Config config = confReader.ReadJson();
            graphics = g;
            spriteBatch = s;
            content = c;
            radius = 16;
            inGame = true;
            isActive = true;
            hitCounter = config.SOLDIER_HP;

            LoadSprite(LoadType.Ship, config.SOLDIER_SPRITE);

            // Randomizing starting point
            int width = Config.rand.Next(GetRadius(), graphics.PreferredBackBufferWidth - GetRadius());
            int height = Config.rand.Next(GetRadius() * -1, 0);

            defaultPosition = new Vector2(width, height);
            currentPosition = defaultPosition;
            laserFireOffset = new Vector2(0, 15);
            SetLaserMode((LaserMode)config.SOLDIER_LASERMODE);
        }

        public Vector2 GetSoldierPosition()
        {
            return position;
        }

        public override void Draw(GameTime gameTime)
        {
            if (inGame)
            {
                spriteBatch.Draw(GetSprite(), currentPosition, Color.White);
            }
            else
                spriteBatch.Draw(GetSprite(), defaultPosition, Color.White);
        }

        public override void Update(GameTime gameTime, Vector2? pp)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerPosition = (Vector2)pp;
            playerPosition.X += 25;     // player's radius

            if (inGame)
            {
                // use controlling the speed of the game by pressing the S key
                if (kState.IsKeyDown(Keys.S))
                    speed = Utilities.ConvertToPercentage(Speed.QuarterSpeed) * GameController.hz;
                else
                    speed = Utilities.ConvertToPercentage(Speed.HalfSpeed) * GameController.hz;

                // Checking to see if we are out of scope, if so, we remove from memory
                if(currentPosition.Y < (GetRadius() * -1))
                    isActive = false;


                Vector2 move = playerPosition - currentPosition;

                // Checking to see if we are returning due to hitting the mid point of the screen
                if(returning)
                    move = returnPosition - currentPosition;
                else if (currentPosition.Y >= (graphics.PreferredBackBufferHeight / 2))
                {
                    returnPosition = Utilities.GetReturnPosition(graphics, defaultPosition, radius);
                    move = returnPosition - currentPosition;
                    returning = true;
                }

                move.Normalize();
                currentPosition += move * speed * dt;
                position = currentPosition;

                counter -= dt;
                if (counter <= 0)
                {
                    FireLaser(gameTime);
                    counter = initCounter / 10;
                }

            }
        }
        protected override void CreateLaser(Vector2 pos, Vector2 dir, GameTime gameTime)
        {
            Config config = confReader.ReadJson();
            LaserFactory factory = new LaserFactory(graphics, spriteBatch, content);
            ILaser lazer = factory.GetEnemyLaser(config.SOLDIER_LASER_SPRITE, pos, dir, gameTime);
            LaserController.AddLaser(lazer);
        }
    }
}
