using CrazyFour.Core.Actors;
using CrazyFour.Core.Actors.Enemy;
using CrazyFour.Core.Actors.Hero;
using CrazyFour.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Lasers
{
    public class PlayerLaser : ILaser
    {
        public PlayerLaser(GraphicsDeviceManager gra, SpriteBatch spr, ContentManager con)
        {
            graphics = gra;
            spriteBatch = spr;
            content = con;
        }

        public override void Initialize(string spritePath, Vector2 pos, Vector2 dir)
        {
            position = pos;
            direction = dir;
            spriteImage = content.Load<Texture2D>(spritePath);
        }

        public override void Draw(GameTime game)
        {
            spriteBatch.Draw(spriteImage, position, Color.White);
        }

        public override void Update(GameTime game)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)game.ElapsedGameTime.TotalSeconds;

            // use controlling the speed of the game by pressing the S key
            if (kState.IsKeyDown(Keys.S))
                speed = Utilities.ConvertToPercentage(Speed.QuarterSpeed) * GameController.hz;
            else
                speed = Utilities.ConvertToPercentage(Speed.Normal) * GameController.hz;

            position += direction* 3f * speed * dt;

            if (position.Y <= 0 || position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                isActive = false;

        }

        public override bool CheckHit(GameTime gameTime, Player player)
        {
            int sum = radius + player.radius;

            if (Vector2.Distance(position, player.currentPosition) < sum)
            {
                isHit = true;
                return true;
            }

            return false;
        }
    }
}
