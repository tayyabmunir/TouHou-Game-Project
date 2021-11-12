using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Helpers
{
    public class Config
    {
        public static int windowWidth = 1280;
        public static int windowHeight = 720;

        public static GameStatus status = GameStatus.Starting;

        public static Random rand = new Random();

        public int LIVES { get; set; } = 3;
        public string PLAYER_SPRITE { get; set; } = "Images/Players/hero";
        public string PLAYER_LASER_SPRITE { get; set; } = "Images/Lazers/AguaLazer";

        //how many lasers can player use
        public static int PLAYER_AVAILABLE_LASERS { get; set; } = 5;

        public int MAX_SOLDIERS { get; set;} = 6;
        public int SOLDIER_TIME { get; set; } = 1;
        public string SOLDIER_SPRITE { get; set; } = "Images/Players/soldier";
        public string SOLDIER_LASER_SPRITE { get; set; } = "Images/Lazers/BlueLazer";
        public int SOLDIER_HP { get; set; } = 1;
        public int SOLDIER_LASERMODE { get; set; } = 0;

        public int MAX_CAPOS { get; set; } = 4;
        public int CAPO_TIME { get; set; } = 5;
        public string CAPO_SPRITE { get; set; } = "Images/Players/capo";
        public string CAPO_LASER_SPRITE { get; set; } = "Images/Lazers/GreenLazer";
        public int CAPO_HP { get; set; } = 2;
        public int CAPO_LASERMODE { get; set; } = 1;

        public int MAX_UBOSS { get; set; } = 2;
        public int UBOSS_TIME { get; set; } = 10;
        public string UBOSS_SPRITE { get; set; } = "Images/Players/underboss";
        public string UBOSS_LASER_SPRITE { get; set; } = "Images/Lazers/YellowLazer";
        public int UBOSS_HP { get; set; } = 3;
        public int UBOSS_LASERMODE { get; set; } = 2;

        public int MAX_BOSS { get; set; } = 1;
        public int BOSS_TIME { get; set; } = 15;
        public string BOSS_SPRITE { get; set; } = "Images/Players/boss";
        public string BOSS_LASER_SPRITE { get; set; } = "Images/Lazers/RedLazer";
        public int BOSS_HP { get; set; } = 6;
        public int BOSS_LASERMODE { get; set; } = 3;

        //must be odd number!!
        public int CIRCLE_LASER_COUNT { get; set; } = 5;
        public int CONE_LASER_COUNT { get; set; } = 5;

        public static Vector2 DOUBLE_LASER_DENSITY { get; } = new Vector2(25, 0);
        public static Vector2 TRIPLE_LASER_DENSITY { get; } = new Vector2(30, 8);

        public static Vector2 CIRCLE_LASER_DENSITY { get; } = new Vector2(28, 25);
        public static float CIRCLE_LASER_DIRECTION_X_MODIF { get; } = 0.1f;

        public static Vector2 CONE_LASER_DENSITY { get; } = new Vector2(20, 15);
        public static float CONE_LASER_DIRECTION_X_MODIF { get; } = 0.05f;




        public static bool doneConfiguringSolders { get; set; } = false;

        public static bool doneConfiguringUnderboss { get; set; } = false;

        public static bool doneConfiguringCapo { get; set; } = false;

        public static bool doneConfiguringBoss { get; set; } = false;
    }
}
