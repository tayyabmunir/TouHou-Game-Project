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
    public class EnemyLaser : ILaser
    {

        public EnemyLaser(GraphicsDeviceManager gra, SpriteBatch spr, ContentManager con)
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

            position += direction * 2f * speed * dt;

            // preping for removal
            if (position.Y < 0)
                isActive = false;
        }

        public override bool CheckHit(GameTime gameTime, Player player)
        {
            int sum = radius + player.radius;
            float dis = Vector2.Distance(position, player.GetPlayerTruePosition());

            if (dis <= sum)
            {
                player.isHit = true;
                player.Lives -= 1;
                player.hitTime = DateTime.Now;

                if (player.Lives <= 0)
                    player.isDead = true;

                return true;
            }

            return false;
        }
    }
}
