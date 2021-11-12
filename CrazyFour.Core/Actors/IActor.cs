using CrazyFour.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Actors
{
    public abstract class IActor
    {
        protected Texture2D spriteImage;
        protected Texture2D laserImage;
        protected SpriteBatch spriteBatch;
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;

        protected Vector2 position;
        protected bool isSpriteLoaded = false;
        protected Vector2 defaultPosition;
        
        public Vector2 currentPosition;
        protected Vector2 soldierPosition;
        protected Vector2 playerPosition;

        public bool inGame = false;
        public bool isHit = false;
        public bool isActive = true;
        public bool isDead = false;

        Config config;
        public ConfigReader confReader = new ConfigReader();

        //direction of laser movement
        protected Vector2 laserDirection = new Vector2(0, 1);
        //starting offset of the laser
        protected Vector2 laserFireOffset = new Vector2(0, 0);
        
        //how close to each other laser sequence is?
        protected Vector2 laserDensity =  new Vector2(20f, 0);

        //type of laser
        protected LaserMode laserMode = LaserMode.Single;

        public virtual int hitCounter { get; set; } = 0;

        public virtual int radius { get; set; } = 0;

        public virtual void Initialize(GraphicsDeviceManager g, SpriteBatch s, ContentManager c)
        {
            Config config = confReader.ReadJson();
            graphics = g;
            spriteBatch = s;
            content = c;
        }

        public virtual bool LoadSprite(LoadType type, String img)
        {
            switch(type)
            {
                case LoadType.Ship:
                    spriteImage = content.Load<Texture2D>(img);
                    isSpriteLoaded = true;
                    break;
                case LoadType.Laser:
                    laserImage = content.Load<Texture2D>(img);
                    break;
            }
            
            return true;
        }

        public int GetRadius()
        {
            if (spriteImage != null)
            {
                int width = spriteImage.Width;
                int height = spriteImage.Height;
                int rad = 0;

                if (width > height)
                    rad = Convert.ToInt32(Math.Ceiling((decimal)(width / 2)));
                else
                    rad = Convert.ToInt32(Math.Ceiling((decimal)(height / 2)));

                return rad;
            }

            throw new ArgumentNullException("Must set the sprite image first.");
        }

        public Texture2D GetSprite()
        {
            if (spriteImage != null)
                return spriteImage;

            throw new ArgumentException("No sprite defined");
        }

        public abstract void Update(GameTime gameTime, Vector2? playerPosition);

        public abstract void Draw(GameTime gameTime);

        protected void FireLaser(GameTime gameTime)
        {
            Config config = confReader.ReadJson();
            switch (laserMode)
            {
                case LaserMode.Single:
                    SeriesOfLaser(gameTime, 1);
                    break;
                case LaserMode.Double:
                    SeriesOfLaser(gameTime, 2);
                    break;
                case LaserMode.Triple:
                    SeriesOfLaser(gameTime, 3);
                    break;
                case LaserMode.Cricle:
                    CircleLaser(gameTime, config.CIRCLE_LASER_COUNT);
                    break;
                case LaserMode.Cone:
                    ConeLaser(gameTime, config.CONE_LASER_COUNT);
                    break;
            }
        }
        protected abstract void CreateLaser(Vector2 pos, Vector2 dir, GameTime gameTime);
        protected void SeriesOfLaser(GameTime gameTime, int count)
        {
            if (count <= 0)
            {
                return;
            }

            bool isEven = count % 2 == 0;
            
            //find center
            int half = count / 2;
            for (int i = 0; i < count; ++i)
            {
                Vector2 dir = laserDirection;
                Vector2 pos = laserFireOffset;
                pos += new Vector2(position.X - 6 + GetSprite().Width / 2, position.Y);
                
                if (isEven)
                {
                    //centerize the laser series by adding offset on X
                    pos.X -= laserDensity.X/2;
                }

                if (i == half)
                {
                    //do nothing keep it in the center
                } else if (i < half)
                {
                    //move next laser to the right
                    pos.X += (i + 1) * laserDensity.X;
                    pos.Y += (i + 1) * laserDensity.Y;
                } else if (i > half)
                {
                    //move next laser to the left
                    pos.X -= (i - half) * laserDensity.X;
                    pos.Y += (i - half) * laserDensity.Y;
                }
                CreateLaser(pos, dir, gameTime);
            }
        }
        protected virtual void CircleLaser(GameTime gameTime, int count, int curveDirX = 1, int curveDirY = 1)
        {
            if (count <= 0)
            {
                return;
            }

            bool isEven = count % 2 == 0;

            int half = count / 2;

            for (int i = 0; i < count; ++i)
            {
                Vector2 dir = laserDirection;
                int threes = (1 + i / 3);
                Vector2 pos = new Vector2(position.X - 6 + GetSprite().Width / 2, position.Y) + laserFireOffset;
                if (isEven)
                {
                    //centerize the laser series by adding offset on X
                    pos.X -= laserDensity.X / 2;
                }

                if (i == half)
                {
                    dir.X = 0;
                    //do nothing keep it in the center
                }
                else if (i < half)
                {
                    //move next laser to the right
                    pos.X += (i + 1) * laserDensity.X;
                    pos.Y += curveDirY * (i + 1) * laserDensity.Y;
                    dir.X = curveDirX * (i + 1) * laserDirection.X;
                }
                else if (i > half)
                {
                    //move next laser to the left
                    pos.X -= (i-half) * laserDensity.X;
                    pos.Y += curveDirY * (i - half) * laserDensity.Y;
                    dir.X = -curveDirX * (i - half) * laserDirection.X;
                }
                CreateLaser(pos, dir, gameTime);
            }
        }
        protected virtual void ConeLaser(GameTime gameTime, int count)
        {
            CircleLaser(gameTime, count, -1);
        }
        public virtual void SetLaserMode(LaserMode laserMode)
        {
            this.laserMode = laserMode;
            switch (laserMode)
            {
                case LaserMode.Single:
                    laserDensity.Y = 0f;
                    laserDirection.X = 0;
                    break;

                case LaserMode.Double:
                    laserDensity = Config.DOUBLE_LASER_DENSITY;
                    laserDirection.X = 0;
                    break;

                case LaserMode.Triple:
                    laserDensity = Config.TRIPLE_LASER_DENSITY;
                    laserDirection.X = 0;
                    break;

                case LaserMode.Cricle:
                    laserDensity = Config.CIRCLE_LASER_DENSITY;
                    laserDirection.X = Config.CIRCLE_LASER_DIRECTION_X_MODIF;
                    break;

                case LaserMode.Cone:
                    laserDensity = Config.CONE_LASER_DENSITY;
                    laserDirection.X = Config.CONE_LASER_DIRECTION_X_MODIF;
                    break;
            }
        }
    }
}
