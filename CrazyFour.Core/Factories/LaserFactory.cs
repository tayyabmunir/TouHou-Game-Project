using CrazyFour.Core.Helpers;
using CrazyFour.Core.Lasers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Factories
{
    class LaserFactory : ILaserFactory
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager content;
        Config config;
        public ConfigReader confReader = new ConfigReader();

        public LaserFactory(GraphicsDeviceManager gra, SpriteBatch spr, ContentManager con)
        {
            Config config = confReader.ReadJson();
            graphics = gra;
            spriteBatch = spr;
            content = con;
        }

        public ILaser GetPlayerLaser(Vector2 pos, Vector2 dir, GameTime gameTime)
        {
            Config config = confReader.ReadJson();
            ILaser actor = new PlayerLaser(graphics, spriteBatch, content);
            actor.Initialize(config.PLAYER_LASER_SPRITE, pos, dir);
            actor.Update(gameTime);
            return actor;
        }
        public ILaser GetEnemyLaser(string spritePath, Vector2 pos, Vector2 dir, GameTime gameTime)
        {
            ILaser actor = new EnemyLaser(graphics, spriteBatch, content);
            actor.Initialize(spritePath, pos, dir);
            actor.Update(gameTime);
            return actor;
        }
    }
}
