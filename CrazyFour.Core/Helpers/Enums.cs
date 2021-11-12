using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Helpers
{
    public enum ActorTypes
    {
        Player,
        Boss,
        Underboss,
        Capo,
        Soldier
    }

    public enum Speed
    {
        Normal = 100,
        ThreeQuarterSpeed = 75,
        HalfSpeed = 50,
        QuarterSpeed = 25
    }

    public enum LoadType
    {
        Ship,
        Laser
    }

    public enum Direction
    {
        Top = -1,
        Right = 1,
        Bottom = 1,
        Left = -1
    }

    public enum GameStatus
    {
        Starting,
        Playing,
        Hit,
        Died,
        Gameover,
        NotSet
    }

    public enum LaserMode
    {
        Single,
        Double,
        Triple,
        Cricle,
        Cone
    }
}
