using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Model
{
    class Enemy
    {
        //Privata variablar
        private Vector2 m_enemyPosition;
        private Vector2 m_lastPosition;
        private Vector2 m_startPosition;
        private Vector2 m_speed = new Vector2(0.1f, 0.2f);
        private Stopwatch m_resWatch = new Stopwatch();
        
        //Publika variablar
        public float enemyType;
        public bool isJumping = false;
        public bool isAlive = true;
        public bool isMovingLeft;
        public bool isMovingRight;
        public bool previousState;

        //Konstruktor
        public Enemy(Vector3 a_positionAndType)
        {
            m_startPosition = new Vector2(a_positionAndType.X, a_positionAndType.Y);
            m_enemyPosition = m_startPosition;
            enemyType = a_positionAndType.Z;
        }

        //Uppdaterar interna simuleringar
        internal void UpdateSimulation(float a_elapsedTime)
        {
            //Återupplivar fiende av typ 2 efter 5 sekunder
            if (!isAlive && enemyType == 2)
            {
                if (m_resWatch.ElapsedMilliseconds == 0)
                {
                    m_resWatch.Start();
                }

                m_speed.X = 0;

                if (m_resWatch.ElapsedMilliseconds > 5000)
                {
                    m_resWatch.Stop();
                    m_resWatch.Reset();
                    isAlive = true;
                }
            }
            if (!isAlive)
            {
                m_speed.X = 0;
                isMovingLeft = false;
                isMovingRight = false;
            }
            else
            {
                if(enemyType == 2)
                {
                    m_speed.X = 0.15f;
                }
                else
                {
                    m_speed.X = 0.1f;
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

        //Publika get/set metoder
        public float EnemyPositionX
        {
            get { return m_enemyPosition.X; }
            set { m_enemyPosition.X = value; }
        }
        
        public float EnemyPositionY
        {
            get { return m_enemyPosition.Y; }
            set { m_enemyPosition.Y = value; }
        }

        public Vector2 GetStartPosition
        {
            get { return m_startPosition; }
        }
        public Vector2 EnemyPosition
        {
            get { return m_enemyPosition; }
            set { m_enemyPosition = value; }
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

        //Interna rörelsemetoder
        internal void DoJump()
        {
            isJumping = true;
        }

        internal void MoveLeft()
        {
            m_enemyPosition.X -= m_speed.X;
        }

        internal void MoveRight()
        {
            m_enemyPosition.X += m_speed.X;
        }

        internal void MoveDown()
        {
            m_enemyPosition.Y += m_speed.Y;
        }

        internal void MoveUp()
        {
            m_enemyPosition.Y -= m_speed.Y;
        }

        internal void FallFree()
        {
            m_enemyPosition.Y += m_speed.Y;
        }

   }
}
