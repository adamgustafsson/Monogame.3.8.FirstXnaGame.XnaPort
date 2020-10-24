using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Model
{
    class BossA
    {
        //Privata variablar
        private Vector2 m_bossAPosition;
        private Vector2 m_lastPosition;
        private Vector2 m_startPosition;
        private Vector2 m_speed = new Vector2(0.1f, 0.2f);
        private Stopwatch m_jumpWatch = new Stopwatch();
        private Stopwatch m_hitWatch = new Stopwatch();

        //Publika variablar
        public int m_life = 10;
        public bool isAlive = true;
        public bool isMovingLeft;
        public bool isMovingRight;
        public bool isAtTurningPoint;
        public bool isJumping = false;
        public bool isPulled = false;
        public bool previousState;
        public bool m_gotHit = false;

        //Konstruktor
        public BossA(Vector2 a_startPosition)
        {
            m_bossAPosition = a_startPosition;
            m_startPosition = a_startPosition;
        }

        //Uppdaterar simulering
        internal void UpdateSimulation(float a_elapsedTime, Player a_player, TileGenerator a_tileGenerator)
        {
            if (m_life <= 0)
            {
                isAlive = false;
            }

            if (!isAlive)
            {
                m_speed.X = 0;
                isMovingLeft = false;
                isMovingRight = false;
            }
            else
            {
                m_jumpWatch.Start();

                if (m_life <= 4)
                {
                    m_speed.X = 0.25f;
                }
                else
                {
                    m_speed.X = 0.17f;
                }
                
                Vector2 playerPosition = new Vector2(a_player.PlayerPositionX, a_player.PlayerPositionY);

                BossColissionObserver(a_tileGenerator);
                
                //En träff = 0.4s
                if (m_hitWatch.ElapsedMilliseconds >= 400 && m_gotHit)
                {
                    m_gotHit= false;
                    m_hitWatch.Stop();
                    m_hitWatch.Reset();
                }

                //Hoppar vart 1.5s
                if (m_jumpWatch.ElapsedMilliseconds >= 1500)
                {
                    isJumping = true;
                    m_jumpWatch.Stop();
                    m_jumpWatch.Reset();
                }
                if (isJumping)
                {
                    m_bossAPosition.Y -= SpeedY;
                    SpeedY -= a_elapsedTime / 1.8f;

                    //Om bossen landar efter hopp
                    if (m_bossAPosition.Y > m_startPosition.Y -0.13f)
                    {
                        isJumping = false;
                        SpeedY = 0.2f;
                    }
                }

                //Om spelaren hoppar och är på väg ner
                if (a_player.m_isJumping && a_player.SpeedY < 0 && !isJumping)
                {
                    //Träffar bossen
                    if (BossAndPlayerCollide(playerPosition) && !m_gotHit)
                    {
                        LostLife();
                    }
                }
                //Annars träffas spelaren
                else if (BossAndPlayerCollide(playerPosition) && isAlive && !m_gotHit)
                {
                    a_player.LostLife();
                }

                if (isAtTurningPoint)
                {
                    isMovingLeft = !isMovingLeft;
                    isMovingRight = !isMovingRight;
                    isAtTurningPoint = false;
                }

                if(!isMovingLeft && !isMovingRight)
                {
                    MoveLeft();
                    isMovingLeft = true;
                    isMovingRight = false;
                }

                if (isMovingLeft)
                {
                    MoveLeft();
                    isMovingLeft = true;
                    isMovingRight = false;
                }

                else
                {
                    MoveRight();
                    isMovingLeft = false;
                    isMovingRight = true;
                }
            }

        }

        //Kollisionssystem för boss
        public void BossColissionObserver(TileGenerator a_tileGenerator)
        {
            foreach (Vector2 tilePosition in a_tileGenerator.GetOccupiedCoords)
            {
                //Om boss kolliderar med tiles i x-led
                if ((BossAPositionX < tilePosition.X + 1) && (BossAPositionX > tilePosition.X - 1) &&
                    (BossAPositionY > tilePosition.Y - 1.4) && (BossAPositionY < tilePosition.Y + 1.4))
                {
                    isAtTurningPoint = true;
                }
            }
        }

        //Kollisionsmetod spelare/boss
        public bool BossAndPlayerCollide(Vector2 a_currentPlayerPos)
        {
            if ((a_currentPlayerPos.X < BossAPositionX + 1.4) && (a_currentPlayerPos.X > BossAPositionX - 1.4) &&
                (a_currentPlayerPos.Y < BossAPositionY + 1.3) && (a_currentPlayerPos.Y > BossAPositionY - 1.3))
            {
                return true;
            }
            return false;
        }

        //Publika get/set metoder
        public float BossAPositionX
        {
            get { return m_bossAPosition.X; }
            set { m_bossAPosition.X = value; }
        }
        
        public float BossAPositionY
        {
            get { return m_bossAPosition.Y; }
            set { m_bossAPosition.Y = value; }
        }

        public Vector2 GetStartPosition
        {
            get { return m_startPosition; }
        }
        public Vector2 BossAPosition
        {
            get { return m_bossAPosition; }
            set { m_bossAPosition = value; }
        }
       
        public float SpeedY
        {
            get { return m_speed.Y; }
            set { m_speed.Y = value; }
        }

        public float SpeedX
        {
            get { return m_speed.X; }
            set { m_speed.X = value; }
        } 

        public Vector2 LastPosition
        {
            get { return m_lastPosition; }
            set { m_lastPosition = value; }
        }

        internal void MoveLeft()
        {
            m_bossAPosition.X -= m_speed.X;
        }

        internal void MoveRight()
        {
            m_bossAPosition.X += m_speed.X;
        }
        
        internal void LostLife()
        {
            if (!m_gotHit)
            {
                m_hitWatch.Start();
                m_gotHit = true;
                m_life--;
            }
        }

        internal void Reset()
        {
            m_life = 10;
            isMovingLeft = false;
            isMovingRight = false;
            isPulled = false;
            isJumping = false;
            SpeedY = 0.2f;
            m_bossAPosition = m_startPosition;
        }



    }
}
