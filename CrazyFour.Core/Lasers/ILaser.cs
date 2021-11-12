using CrazyFour.Core.Actors;
using CrazyFour.Core.Actors.Hero;
using CrazyFour.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Lasers
{
    public abstract class ILaser
    {
        protected Texture2D spriteImage;
        protected SpriteBatch spriteBatch;
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected bool isSpriteLoaded = false;

        public bool isActive { get; set; } = true;

        public bool isHit { get; set; } = false;

        public float speed { get; set; }
        public virtual int radius { get; } = 6;
        public Vector2 direction;
        public Vector2 position;

        public abstract void Initialize(string spritePath, Vector2 pos, Vector2 dir);

        public abstract void Draw(GameTime game);

        public abstract void Update(GameTime game);

        public abstract bool CheckHit(GameTime gameTime, Player playerPos);
    }
}
