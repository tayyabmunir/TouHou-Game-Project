using CrazyFour.Core.Helpers;
using CrazyFour.Core.Lasers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Factories
{
    interface ILaserFactory
    {
        ILaser GetPlayerLaser(Vector2 pos, Vector2 dir, GameTime gameTime);
        ILaser GetEnemyLaser(string spritePath, Vector2 pos, Vector2 dir, GameTime gameTime);
    }
}
