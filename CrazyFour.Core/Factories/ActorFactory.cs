using CrazyFour.Core.Actors;
using CrazyFour.Core.Actors.Enemy;
using CrazyFour.Core.Actors.Hero;
using CrazyFour.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core.Factories
{
    public class ActorFactory : IActorFactory
    {
        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;
        public ContentManager content;

        public ActorFactory(GraphicsDeviceManager g, SpriteBatch s, ContentManager c)
        {
            graphics = g;
            spriteBatch = s;
            content = c;
        }

        public IActor GetActor(ActorTypes type)
        {
            switch(type)
            {
                case ActorTypes.Boss:
                    return new Boss(graphics, spriteBatch, content);

                case ActorTypes.Capo:
                    return new Capo(graphics, spriteBatch, content);

                case ActorTypes.Player:
                    return new Player(graphics, spriteBatch, content);

                case ActorTypes.Soldier:
                    return new Soldier(graphics, spriteBatch, content);

                case ActorTypes.Underboss:
                    return new Underboss(graphics, spriteBatch, content);

                default:
                    throw new ArgumentException();
            }
        }

    }
}
