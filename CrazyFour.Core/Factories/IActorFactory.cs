using CrazyFour.Core.Actors;
using CrazyFour.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;


namespace CrazyFour.Core.Factories
{
    interface IActorFactory
    {
        IActor GetActor(ActorTypes type);
    }
}
