using CrazyFour.Core.Actors;
using CrazyFour.Core.Helpers;
using CrazyFour.Core.Lasers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core
{
    public class LaserController
    {
        public static List<EnemyLaser> enemyLasers = new List<EnemyLaser>();
        public static List<PlayerLaser> playerLasers = new List<PlayerLaser>();

        public void ProcessLasers(GameTime gameTime)
        {
            foreach (PlayerLaser laser in playerLasers)
            {
                bool wasHit = false;
                foreach (IActor enemy in GameController.enemyList)
                {
                    wasHit = CheckCollision(gameTime, laser, enemy);
                    if (wasHit)
                    {
                        break;
                    }
                }

                if (!wasHit)
                {
                    foreach (EnemyLaser enemyLaser in enemyLasers)
                    {
                        wasHit = CheckLaserCollision(gameTime, laser, enemyLaser);
                        if (wasHit)
                        {
                            break;
                        }
                    }
                }

                if (!wasHit)
                {
                    laser.Update(gameTime);
                }
            }

            RemoveLasers();
        }

        public static void AddLaser(ILaser laser)
        {
            EnemyLaser isEnemy = laser as EnemyLaser;

            if (isEnemy == null)
            {
                playerLasers.Add(laser as PlayerLaser);
            }
            else
            {
                enemyLasers.Add(isEnemy);
            }
        }

        public bool CheckCollision(GameTime gameTime, PlayerLaser laser, IActor actor)
        {
            int sum = actor.radius + laser.radius;

            if (actor.hitCounter > 0 && Vector2.Distance(laser.position, actor.currentPosition) < sum)
            {
                --actor.hitCounter;
                laser.isHit = true;
                if (actor.hitCounter <= 0)
                {
                    actor.hitCounter = 0;
                    actor.isHit = true;
                }
                return true;
            }
            return false;
        }

        public bool CheckLaserCollision(GameTime gameTime, PlayerLaser laser, EnemyLaser enemyLaser)
        {
            int sum = enemyLaser.radius + laser.radius;

            if (Vector2.Distance(laser.position, enemyLaser.position) < sum)
            {
                enemyLaser.isHit = true;
                laser.isHit = true;
                return true;
            }
            return false;
        }


        public void RemoveLasers()
        {
            // Removing any player lasors that have gone out of window
            playerLasers.RemoveAll(r => r.isActive is false || r.isHit);

            // Removing any enemy lasors that have done out of the window
            enemyLasers.RemoveAll(r => r.isActive is false || r.isHit);
        }

    }
}
