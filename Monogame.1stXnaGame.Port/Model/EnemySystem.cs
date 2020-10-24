using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Model
{
    //Spelmotor för fiender
    class EnemySystem
    {
        //Publika variabler
        public BossA m_bossA;
        public ObjectArray m_objectArray;

        //Privata variabler
        private Enemy m_enemyA;
        private int m_enemyAcount;
        private Vector3[] m_enemyPositionAndType; //position x, position y, typ av fiende (1,2,3)
        private Vector2 m_startLocationsBossA;
        private TileGenerator m_tileGenerator;

        //Konstruktor; skapar ny fiende-array beroende på aktiv map
        public EnemySystem(int a_activeMap, TileGenerator a_tileGenerator)
        {
            m_tileGenerator = a_tileGenerator;

            if(a_activeMap == 0)
            {
                m_enemyAcount = 8;
                m_enemyPositionAndType = new Vector3[m_enemyAcount];
                m_enemyPositionAndType[0] = new Vector3(25f, 17f, 1f);
                m_enemyPositionAndType[1] = new Vector3(32f, 15f, 1f);
                m_enemyPositionAndType[2] = new Vector3(46f, 19f, 1f);
                m_enemyPositionAndType[3] = new Vector3(60f, 19f, 1f);
                m_enemyPositionAndType[4] = new Vector3(65f, 19f, 1f);
                m_enemyPositionAndType[5] = new Vector3(78f, 18f, 1f);
                m_enemyPositionAndType[6] = new Vector3(114f, 19f, 1f);
                m_enemyPositionAndType[7] = new Vector3(118f, 19f, 1f);
            }
            else if (a_activeMap == 1)
            {
                m_enemyAcount = 10;
                m_enemyPositionAndType = new Vector3[m_enemyAcount];
                m_enemyPositionAndType[0] = new Vector3(20f, 19f, 1f);
                m_enemyPositionAndType[1] = new Vector3(30f, 19f, 1f);
                m_enemyPositionAndType[2] = new Vector3(51f, 13f, 1f);
                m_enemyPositionAndType[3] = new Vector3(51f, 13f, 2f);
                m_enemyPositionAndType[4] = new Vector3(60f, 13f, 1f);
                m_enemyPositionAndType[5] = new Vector3(53f, 19f, 1f);
                m_enemyPositionAndType[6] = new Vector3(70f, 19f, 1f);
                m_enemyPositionAndType[7] = new Vector3(51f, 19f, 2f);
                m_enemyPositionAndType[8] = new Vector3(85f, 19f, 2f);
                m_enemyPositionAndType[9] = new Vector3(40f, 6f, 2f);


                m_startLocationsBossA = new Vector2(185f, 18.5f);
                m_bossA = new BossA(m_startLocationsBossA);

            }
            else if (a_activeMap == 2)
            {
                m_enemyAcount = 19;
                m_enemyPositionAndType = new Vector3[m_enemyAcount];
                m_enemyPositionAndType[0] = new Vector3(26f, 17f, 3f);
                m_enemyPositionAndType[1] = new Vector3(60f, 10f, 3f);
                m_enemyPositionAndType[2] = new Vector3(85f, 14f, 3f);
                m_enemyPositionAndType[3] = new Vector3(82f, 14f, 3f);
                m_enemyPositionAndType[4] = new Vector3(123f, 17f, 3f);
                m_enemyPositionAndType[5] = new Vector3(137f, 17f, 3f);
                m_enemyPositionAndType[6] = new Vector3(185f, 19f, 3f);
                m_enemyPositionAndType[7] = new Vector3(240f, 17f, 3f);
                m_enemyPositionAndType[8] = new Vector3(245f, 17f, 3f);
                m_enemyPositionAndType[9] = new Vector3(55f, 10f, 1f);
                m_enemyPositionAndType[10] = new Vector3(250f, 17f, 1f);
                m_enemyPositionAndType[11] = new Vector3(105f, 17f, 1f);
                m_enemyPositionAndType[12] = new Vector3(150f, 19f, 1f);
                m_enemyPositionAndType[13] = new Vector3(155f, 19f, 1f);
                m_enemyPositionAndType[14] = new Vector3(160f, 19f, 1f);
                m_enemyPositionAndType[15] = new Vector3(104f, 17f, 2f);
                m_enemyPositionAndType[16] = new Vector3(155f, 17f, 2f);
                m_enemyPositionAndType[17] = new Vector3(170f, 17f, 2f);
                m_enemyPositionAndType[18] = new Vector3(220f, 17f, 2f);
            }

            m_objectArray = new ObjectArray(m_enemyAcount);
            int i = 0;

            while (i < m_enemyAcount)
            {
                m_enemyA = new Enemy(m_enemyPositionAndType[i]);
                m_objectArray.Add(m_enemyA);
                i++;
            }

        }

        //Uppdaterar fiender
        internal void EnemyUpdateSimulation(float a_elapsedTime, Player a_player)
        {
            Vector2 playerPosition = new Vector2(a_player.PlayerPositionX, a_player.PlayerPositionY);

            foreach (Enemy enemy in m_objectArray.Get())
            {
                if (a_player.m_isJumping && a_player.SpeedY < 0)
                {
                    if (EnemyAndPlayerCollide(playerPosition, enemy))
                    {
                        enemy.isAlive = false;
                    }
                }
                else if (a_player.m_isFalling)
                {
                    if (EnemyAndPlayerCollide(playerPosition, enemy))
                    {
                        enemy.isAlive = false;
                    }
                }
                else if (EnemyAndPlayerCollide(playerPosition, enemy) && enemy.isAlive && !a_player.m_gotHit)
                {
                    a_player.LostLife();
                }

                if (EnemyReachTurningPoint(enemy))
                {
                    enemy.isMovingLeft = !enemy.isMovingLeft;
                    enemy.isMovingRight = !enemy.isMovingRight;
                    
                }

                //Uppdaterar resterande simulering
                enemy.UpdateSimulation(a_elapsedTime);
            }

            //Uppdatering av boss
            if (m_bossA != null)
            {
                //Bestämmer "pull" - avstånd
                if (a_player.PlayerPositionX > m_startLocationsBossA.X - 10)
                {
                    m_bossA.UpdateSimulation(a_elapsedTime, a_player, m_tileGenerator);
                    m_bossA.isPulled = true;
                }
                else if (m_bossA.isPulled)
                {
                    m_bossA.UpdateSimulation(a_elapsedTime, a_player, m_tileGenerator);
                }
            }
        }

        //Kollisionsregler för fiender
        public bool EnemyReachTurningPoint(Enemy a_enemy)
        {
            foreach (Vector2 tilePosition in m_tileGenerator.GetOccupiedCoords)
            {
                //Om fiende är på marken
                if ((a_enemy.EnemyPositionX > tilePosition.X - 0.5) && (a_enemy.EnemyPositionX < tilePosition.X + 0.5) &&
                    (a_enemy.EnemyPositionY > tilePosition.Y - 1.5) && (a_enemy.EnemyPositionY < tilePosition.Y))
                {
                    return false;
                }
                //Kollision i x-led    
                else if ((a_enemy.EnemyPositionX < tilePosition.X + 1) && (a_enemy.EnemyPositionX > tilePosition.X - 1) &&
                         (a_enemy.EnemyPositionY == tilePosition.Y) || a_enemy.EnemyPositionX < 0)
                {
                    return true;
                }
            }
            //Annars vid en kant
            //Fiende av typ 2 vänder ej vid en kant
            if (a_enemy.enemyType != 2)
            {
                return true;
            }
            return false;
            
        }

        //Kollisionsregler för fiende/spelare
        public bool EnemyAndPlayerCollide(Vector2 a_currentPlayerPos, Enemy a_enemyA)
        {
                if ((a_currentPlayerPos.X < a_enemyA.EnemyPositionX + 1.4) && (a_currentPlayerPos.X > a_enemyA.EnemyPositionX - 1.4) &&
                    (a_currentPlayerPos.Y < a_enemyA.EnemyPositionY + 1.3) && (a_currentPlayerPos.Y > a_enemyA.EnemyPositionY - 1.3))
                {
                    return true;
                }
            return false;
        }

        //Återställer aktiva fienders liv samt position
        internal void ResetEnemies()
        {
            foreach (Enemy enemy in m_objectArray.Get())
            {
                enemy.isAlive = true;
                enemy.EnemyPosition = enemy.GetStartPosition;
            }

            if (m_bossA != null)
            {
                m_bossA.Reset();
            }
       }
    }


}



























       //private EnemyA m_enemyA;
       // public BossA m_bossA;
       // public ObjectArray m_objectArray;
       // private int m_enemyAcount;
       // private Vector2[] m_startLocationsEnemyA;
       // private Vector2 m_startLocationsBossA;
       // private TileGenerator m_tileGenerator;

       // //Konstruktor; skapar ny partikelarray via ArrayKlass
       // public EnemySystem(string a_map, TileGenerator a_tileGenerator)
       // {
       //     m_tileGenerator = a_tileGenerator;

       //     if(a_map == "Map1")
       //     {
       //         m_enemyAcount = 8;
       //         m_startLocationsEnemyA = new Vector2[m_enemyAcount];
       //         m_startLocationsEnemyA[0] = new Vector2(25f, 17f);
       //         m_startLocationsEnemyA[1] = new Vector2(32f, 15f);
       //         m_startLocationsEnemyA[2] = new Vector2(46f, 19f);
       //         m_startLocationsEnemyA[3] = new Vector2(60f, 19f);
       //         m_startLocationsEnemyA[4] = new Vector2(65f, 19f);
       //         m_startLocationsEnemyA[5] = new Vector2(78f, 18f);
       //         m_startLocationsEnemyA[6] = new Vector2(114f, 19f);
       //         m_startLocationsEnemyA[7] = new Vector2(118f, 19f);
       //     }
       //     else if (a_map == "Map2")
       //     {
                //m_enemyAcount = 6;
                //m_startLocationsEnemyA = new Vector2[m_enemyAcount];
                //m_startLocationsEnemyA[0] = new Vector2(20f, 19f);
                //m_startLocationsEnemyA[1] = new Vector2(30f, 19f);
                //m_startLocationsEnemyA[2] = new Vector2(51f, 13f);
                //m_startLocationsEnemyA[3] = new Vector2(60f, 13f);
                //m_startLocationsEnemyA[4] = new Vector2(53f, 19f);
                //m_startLocationsEnemyA[4] = new Vector2(70f, 19f);

       //         m_startLocationsBossA = new Vector2(193f, 18.5f);
       //         m_bossA = new BossA(m_startLocationsBossA);

       //     }

       //     m_objectArray = new ObjectArray(m_enemyAcount);
       //     int i = 0;

       //     while (i < m_enemyAcount)
       //     {
       //         m_enemyA = new EnemyA(m_startLocationsEnemyA[i]);
       //         m_objectArray.Add(m_enemyA);
       //         i++;
       //     }

       // }

       // internal void EnemyUpdateSimulation(float a_elapsedTime, Player a_player)
       // {
       //     Vector2 playerPosition = new Vector2(a_player.PlayerPositionX, a_player.PlayerPositionY);

       //     foreach (EnemyA enemyA in m_objectArray.Get())
       //     {
       //         if (a_player.m_isJumping && a_player.SpeedY < 0)
       //         {
       //             if (EnemyAndPlayerCollide(playerPosition, enemyA))
       //             {
       //                 enemyA.isAlive = false;
       //             }
       //         }
       //         else if (a_player.m_isFalling)
       //         {
       //             if (EnemyAndPlayerCollide(playerPosition, enemyA))
       //             {
       //                 enemyA.isAlive = false;
       //             }
       //         }
       //         else if (EnemyAndPlayerCollide(playerPosition, enemyA) && enemyA.isAlive && !a_player.m_gotHit)
       //         {
       //             a_player.LostLife();
       //         }

       //         if (EnemyReachTurningPoint(enemyA))
       //         {
       //             enemyA.isMovingLeft = !enemyA.isMovingLeft;
       //             enemyA.isMovingRight = !enemyA.isMovingRight;
                    
       //         }

       //         enemyA.UpdateSimulation(a_elapsedTime);

       //         if (m_bossA != null)
       //         {
       //             if (a_player.PlayerPositionX > m_startLocationsBossA.X - 10)
       //             {
       //                 m_bossA.UpdateSimulation(a_elapsedTime, a_player, m_tileGenerator);
       //                 m_bossA.isPulled = true;
       //             }
       //             else if(m_bossA.isPulled)
       //             {
       //                 m_bossA.UpdateSimulation(a_elapsedTime, a_player, m_tileGenerator);
       //             }
       //         }
       //     }
       // }

       // public bool EnemyReachTurningPoint(EnemyA a_enemyA)
       // {
       //     foreach (Vector2 tilePosition in m_tileGenerator.GetOccupiedCoords)
       //     {
       //         if ((a_enemyA.EnemyAPositionX > tilePosition.X - 0.5) && (a_enemyA.EnemyAPositionX < tilePosition.X + 0.5) &&
       //             (a_enemyA.EnemyAPositionY > tilePosition.Y - 1.5) && (a_enemyA.EnemyAPositionY < tilePosition.Y))
       //         {
       //             return false;
       //         }
       //         else if ((a_enemyA.EnemyAPositionX < tilePosition.X + 1) && (a_enemyA.EnemyAPositionX > tilePosition.X - 1) &&
       //                  (a_enemyA.EnemyAPositionY == tilePosition.Y))
       //         {
       //             return true;
       //         }
       //     }

       //     return true;
       // }

       // public bool EnemyAndPlayerCollide(Vector2 a_currentPlayerPos, EnemyA a_enemyA)
       // {
       //         if ((a_currentPlayerPos.X < a_enemyA.EnemyAPositionX + 1.4) && (a_currentPlayerPos.X > a_enemyA.EnemyAPositionX - 1.4) &&
       //             (a_currentPlayerPos.Y < a_enemyA.EnemyAPositionY + 1.3) && (a_currentPlayerPos.Y > a_enemyA.EnemyAPositionY - 1.3))
       //         {
       //             return true;
       //         }
       //     return false;
       // }

       // internal void ResetEnemies()
       // {
       //     foreach (EnemyA enemyA in m_objectArray.Get())
       //     {
       //         enemyA.isAlive = true;
       //         enemyA.EnemyAPosition = enemyA.GetStartPosition;
       //     }

       //     if (m_bossA != null)
       //     {
       //         m_bossA.BossAPosition = m_startLocationsBossA;
       //         m_bossA.m_life = 10;
       //         m_bossA.isPulled = false;
       //     }
       //}