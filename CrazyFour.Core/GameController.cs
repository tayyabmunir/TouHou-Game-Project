using CrazyFour.Core.Actors;
using CrazyFour.Core.Actors.Enemy;
using CrazyFour.Core.Actors.Hero;
using CrazyFour.Core.Factories;
using CrazyFour.Core.Helpers;
using CrazyFour.Core.Lasers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyFour.Core
{
    public sealed class GameController
    {
        private static readonly object padlock = new object();
        private static GameController instance = null;
        private static LaserController lasers = new LaserController();

        public List<Texture2D> soldierSprites = new List<Texture2D>();
        public static List<IActor> enemyList = new List<IActor>();

        public static int hz = 60;
        public static float totalTime = 0f;

        private ActorFactory factory;

        Config config;
        public ConfigReader confReader = new ConfigReader();

        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new GameController();
                        }
                    }
                }

                return instance;
            }
        }

        private GameController() 
        {
            Config config = confReader.ReadJson();
        }

        public void LoadContent(ActorFactory fac)
        {
            factory = fac;
        }

        public void InitializeEnemies(GameTime game, ActorTypes type)
        {
            Config config = confReader.ReadJson();
            switch (type)
            {
                case ActorTypes.Boss:
                    if (!Config.doneConfiguringBoss)
                    {
                        if ((int)totalTime >= config.BOSS_TIME)
                        {
                            for (int i = 0; i < config.MAX_BOSS; i++)
                            {
                                var sol = (Boss)factory.GetActor(ActorTypes.Boss);
                                GameController.enemyList.Add(sol);
                            }

                            Config.doneConfiguringBoss = true;
                        }
                    }
                    break;
                case ActorTypes.Capo:
                    if (!Config.doneConfiguringCapo)
                    {
                        if ((int)totalTime >= config.CAPO_TIME)
                        {
                            for (int i = 0; i < config.MAX_CAPOS; i++)
                            {
                                var capo = (Capo)factory.GetActor(ActorTypes.Capo);
                                GameController.enemyList.Add(capo);
                            }

                            Config.doneConfiguringCapo = true;
                        }
                    }
                    break;
                case ActorTypes.Soldier:
                    if (!Config.doneConfiguringSolders)
                    {
                        if ((int)totalTime >= config.SOLDIER_TIME)
                        {
                            for (int i = 0; i < config.MAX_SOLDIERS; i++)
                            {
                                var sol = (Soldier)factory.GetActor(ActorTypes.Soldier);
                                GameController.enemyList.Add(sol);
                            }

                            Config.doneConfiguringSolders = true;
                        }
                    }
                    break;
                case ActorTypes.Underboss:
                    if (!Config.doneConfiguringUnderboss)
                    {
                        if ((int)totalTime >= config.UBOSS_TIME)
                        {
                            for (int i = 0; i < config.MAX_UBOSS; i++)
                            {
                                var sol = (Underboss)factory.GetActor(ActorTypes.Underboss);
                                GameController.enemyList.Add(sol);
                            }

                            Config.doneConfiguringUnderboss = true;
                        }
                    }
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        private bool InitializeEnemiesObjects(GameTime gameTime)
        {
            InitializeEnemies(gameTime, ActorTypes.Boss);
            InitializeEnemies(gameTime, ActorTypes.Underboss);
            InitializeEnemies(gameTime, ActorTypes.Capo);
            InitializeEnemies(gameTime, ActorTypes.Soldier);


            if (Config.doneConfiguringSolders) return true;
            else if (Config.doneConfiguringUnderboss) return true;
            else if (Config.doneConfiguringCapo) return true;
            else if (Config.doneConfiguringBoss) return true;
            else 
                return false;

        }

        public void Draw(GameTime gameTime)
        {
            if (Config.status == GameStatus.Playing)
            {
                // Updating the enemy's position
                foreach (var sol in GameController.enemyList)
                {
                    sol.Draw(gameTime);
                }

                // Updating position for the enemy lasers
                foreach (EnemyLaser enemy in LaserController.enemyLasers)
                {
                    enemy.Draw(gameTime);
                }

                // Updating position for the player lasers
                foreach (PlayerLaser player in LaserController.playerLasers)
                {
                    player.Draw(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (Config.status == GameStatus.Playing)
            {
                if(!InitializeEnemiesObjects(gameTime))
                {
                    // means no enemies have been created, so skipping
                    return;
                }

                foreach (var sol in GameController.enemyList)
                {
                    sol.Update(gameTime, player.GetPlayerPosition());
                }

                bool hit = false;

                foreach (EnemyLaser laser in LaserController.enemyLasers)
                {
                    laser.Update(gameTime);
                    hit = laser.CheckHit(gameTime, player);
                    if (!hit)
                    {
                        laser.CheckHit(gameTime, player);
                    }
                    if (hit)
                        break;
                }

                if (hit)
                    LaserController.enemyLasers.Clear();

                // Removing the enemies from our list
                GameController.enemyList.RemoveAll(r => !r.isActive || r.isHit);


                if (GameController.enemyList.Count <= 0)
                {
                    LaserController.enemyLasers.Clear();
                    LaserController.playerLasers.Clear();
                    Config.status = GameStatus.Gameover;
                } else
                {
                    lasers.ProcessLasers(gameTime);
                }
            }
        }
        
    }
}
