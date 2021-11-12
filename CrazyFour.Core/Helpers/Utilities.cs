using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CrazyFour.Core.Helpers
{
    public class Utilities
    {
        private static Random rand = new Random();

        public static String TicksToTime(double t)
        {
            int totalSeconds = (int)Math.Ceiling(t);

            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            string time = minutes.ToString("D2") + ":" + seconds.ToString("D2");

            return time;
        }

        public static float ConvertToPercentage(Speed speedType)
        {
            float ret = (((float)speedType) / 100);
            return ret;
        }

        public static Vector2 GetReturnPosition(GraphicsDeviceManager grap, Vector2 playerPos, int radius)
        {
            int newX;
            int newY;
            int mid = grap.PreferredBackBufferWidth / 2;

            if (playerPos.X <= mid)
            {
                newX = Config.rand.Next(mid, grap.PreferredBackBufferWidth + 1);
                newY = Config.rand.Next(-150, (radius * -1));
            }
            else
            {
                newX = Config.rand.Next(0, mid);
                newY = Config.rand.Next(-150, (radius * -1));
            }


            return new Vector2(newX, newY);
        }
    }
}
