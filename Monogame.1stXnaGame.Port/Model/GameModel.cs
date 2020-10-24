using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Model
{   
    //Huvudklass för hantering av spelregler
    class GameModel
    {
        //Publika variabler
        //Används av Controller & View klasser
        public Player m_player;
        public EnemySystem m_enemySystem;
        public TileGenerator m_tileGenerator;
        public string[] m_maps = new string[3]{ "Map1", "Map2", "Map3"};
        public List<string> m_mapPaths;
        public int m_activeMapNr;

        //Privata variablar
        private Random m_rand = new Random();
        private Stopwatch m_modelWatch = new Stopwatch();
        private Vector2 m_playerStartposition;
        private bool m_canMoveRight= true;
        private bool m_canMoveLeft = true;
        private bool m_didCollide = false;
        private bool m_hitRoof = false;
        private bool m_gameOver = false;
        private bool m_gameIsRunning = false;

        //Konstruktor
        public GameModel()
        {
            m_playerStartposition = new Vector2(5f, 18f);
            m_player = new Player(m_playerStartposition);       
            m_tileGenerator = new TileGenerator();
        }

        //Uppdaterar spelmotor
        internal void UpdateSimulation(float a_elapsedTime)
        {
            m_player.m_isFalling = false;

            //Om spelaren dör
            if (m_player.m_lives <= 0 || m_player.PlayerPositionY > Level.LEVEL_HEIGHT)
            {
                m_gameOver = true;
            }
            else
            {   
                //Om spelaren kommer till banans gräns åt vänster
                if (m_player.PlayerPositionX <= 0)
                {
                    m_player.PlayerPositionX = m_player.LastPosition.X;
                }
                //Om spelare och tile kolliderar
                if (PlayerAndTileCollide())
                {
                    m_player.PlayerPositionX = m_player.LastPosition.X;
                    m_player.PlayerPositionY = m_player.LastPosition.Y;
                    m_didCollide = true;
                }
                else
                {
                    m_didCollide = false;
                    m_player.LastPosition = new Vector2(m_player.PlayerPositionX, m_player.PlayerPositionY);
                }
                //Om spelaren hoppar
                if (m_player.m_isJumping)
                {
                    //Skapar hopp
                    m_player.PlayerPositionY -= m_player.SpeedY;//minskar Y med Y-speed..
                    m_player.SpeedY -= a_elapsedTime / 1.8f;    //minskar y-speed, när speed blir negativt ökar Y igen

                    //(Buggfix)Om hoppet når sin maxhöjd utan kollision
                    if (m_player.SpeedY < 0)
                    {
                        if (!m_didCollide)
                        {
                            m_canMoveRight = true;
                            m_canMoveLeft = true;
                        }

                    }
                    //Reducerar hoppets maxhöjd vid kollision med tak
                    else if (m_hitRoof)
                    {
                        m_player.SpeedY = 0;
                    }
                    //Om spelaren landar på marken
                    if (PlayerStandsOnGround())
                    {
                        m_player.SpeedY = 0.2f;
                        m_player.m_isJumping = false;
                    }
                }
                //Spelaren faller
                else if (IsFalling())
                {
                    m_player.FallFree();
                    m_player.m_isFalling = true;
                }
                //Uppdaterar fiendesystem samt resterande simulation för spelaren
                m_enemySystem.EnemyUpdateSimulation(a_elapsedTime, m_player);
                m_player.UpdateSimulation(a_elapsedTime);
            }

        }

        //Kollisionssystem för spelare/tiles
        public bool PlayerAndTileCollide()
        {
            foreach(Vector2 tilePosition in m_tileGenerator.GetOccupiedCoords)
            {
                //Radien för kollissionsystemet ska rent matematiskt vara 1.5 modellkordenater, men jag väljer här att finsjustera det till 1.3/1.4
                //då spelartexturen har lite tomrum innanför sin textur  
                if ((m_player.PlayerPositionX < tilePosition.X + 1.4) && (m_player.PlayerPositionX > tilePosition.X - 1.4) &&
                    (m_player.PlayerPositionY < tilePosition.Y + 1.3) && (m_player.PlayerPositionY > tilePosition.Y - 1.3))
                {
                    //Om man hoppar in i tiles mot höger kan man inte fortsätta röra sig åt höger
                    if (m_player.PlayerPositionX > m_player.LastPosition.X) 
                    {
                        m_canMoveRight = false;
                    }
                    //Om man hoppar in i tiles mot vänster kan man inte fortsätta röra sig åt vänster
                    else if (m_player.PlayerPositionX < m_player.LastPosition.X)
                    {
                        m_canMoveLeft = false;
                    }
                    //Spelaren träffar tiles i Y led
                    else if (m_player.PlayerPositionY < m_player.LastPosition.Y)
                    {
                        m_hitRoof = true;
                    }
                    return true;
                }
            }
            return false;
        }

        //Kollisionssystem för Spelare/Fiende
        public bool PlayerAndEnemyCollide()
        {
            foreach (Enemy enemy in m_enemySystem.m_objectArray.Get())
            {
                if ((m_player.PlayerPositionX < enemy.EnemyPositionX + 1.4) && (m_player.PlayerPositionX > enemy.EnemyPositionX - 1.4) &&
                    (m_player.PlayerPositionY < enemy.EnemyPositionY + 1.3) && (m_player.PlayerPositionY > enemy.EnemyPositionY - 1.3))
                {
                    return true;
                }  
            }
            return false; 
        }

        //Kollisionsmetod för kontroll av att spelaren befinner sig på marken
        public bool PlayerStandsOnGround()
        {
            foreach (Vector2 tilePosition in m_tileGenerator.GetOccupiedCoords)
            {
                if ((m_player.PlayerPositionX > tilePosition.X - 1.4) && (m_player.PlayerPositionX < tilePosition.X + 1.4) &&
                    (m_player.PlayerPositionY > tilePosition.Y - 1.5) && (m_player.PlayerPositionY < tilePosition.Y))
                {
                     m_canMoveRight = true;
                     m_canMoveLeft = true;
                     m_hitRoof = false;
                     return true;
                }
            }

            return false;
        }

        //Retunerar true om spelaren kommer till bossen
        internal bool BossEncountered()
        {
            return m_activeMapNr == 1 && m_player.PlayerPositionX > 162.4;
        }

        internal bool CanMoveDown()
        {
            return false;
        }

        internal bool CanMoveUp()
        {
            return false;
        }

        internal bool CanJump()
        {
            return true;
        }

        internal bool CanMoveRight()
        {
            return m_canMoveRight;
        }

        internal bool CanMoveLeft()
        {
            return m_canMoveLeft;
        }

        internal bool IsFalling()
        {
            if (!PlayerStandsOnGround() && !m_player.m_isJumping)
            {
                return true;
            }

            return false;
         
        }

        internal bool GameOver()
        {
            return m_gameOver;
        }

        internal bool GameIsRunning()
        {
            return m_gameIsRunning;
        }

        //Spelaren når slutet på sista banan
        internal bool GameCompleted()
        {
            if (m_activeMapNr == 2 && m_player.PlayerPositionX > m_tileGenerator.GetWidth)
            {
                return true;
            }
            return false;
        }

        //Spelaren klarar en bana
        internal bool MapCompleted()
        {
            //Bana 1
            if (m_activeMapNr == 0 && m_player.PlayerPositionX > m_tileGenerator.GetWidth)
            {
                m_player.m_lives++;
                return true;
            }
            //Bana 2
            else if(m_activeMapNr == 1 && !m_enemySystem.m_bossA.isAlive)
            {
                m_modelWatch.Start();

                if(m_modelWatch.ElapsedMilliseconds >= 12000)
                {
                    m_modelWatch.Stop();
                    m_modelWatch.Reset();
                    m_player.m_lives++;
                    return true;
                }

            }

            return false;
        }

        //Om spelaren väljer "Continue"
        internal void Continue()
        {
            m_player.PlayerPositionX = m_playerStartposition.X;
            m_player.PlayerPositionY = m_playerStartposition.Y;
            m_player.m_lives = 3;
            m_enemySystem.ResetEnemies();
            m_gameOver = false;
        }

        //Startar nästa map 
        internal void StartNextLevel()
        {
            m_player.PlayerPositionX = m_playerStartposition.X;
            m_player.PlayerPositionY = m_playerStartposition.Y;
            m_activeMapNr++;
            LoadActiveMap();
        }

        //Startar mett nytt spel
        internal void StartNewGame()
        {
            if (GameCompleted() || m_activeMapNr > 0)
            {
                m_activeMapNr = 0;
                LoadActiveMap();
            }
           
            m_enemySystem.ResetEnemies();  
            m_player.m_lives = 3;
            m_player.PlayerPositionX = m_playerStartposition.X;
            m_player.PlayerPositionY = m_playerStartposition.Y;
            m_gameIsRunning = true;
        }

        //Laddar aktiv map
        internal void LoadActiveMap()
        {
            m_mapPaths.ToArray();
            m_tileGenerator.GenerateMapTiles(m_mapPaths[m_activeMapNr]);
            m_enemySystem = new EnemySystem(m_activeMapNr, m_tileGenerator); 
        }

        //Laddar in samtliga sökvägar för mapsen
        internal void LoadMapPaths(string a_rootDirectory)
        {
            m_mapPaths = new List<string>();

            foreach (string mapName in m_maps)
            {
                m_mapPaths.Add(a_rootDirectory + @"\" + mapName + ".map");
            }


        }
    }
}




























//public bool PlayerCanMoveRight()
//{
//    foreach (Vector2 tilePosition in m_tileGenerator.GetOccupiedCoords)
//    {
//        if ((m_player.PlayerPositionX < tilePosition.X - 2) && (m_player.PlayerPositionX > tilePosition.X + 2) &&
//            (m_player.PlayerPositionY < tilePosition.Y - 0.5) && (m_player.PlayerPositionY > tilePosition.Y + 0.5))
//        {

//            return true;
//        }
//    }

//    return false;
//}