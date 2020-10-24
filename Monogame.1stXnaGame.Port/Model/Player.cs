using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Model
{
    class Player
    {
        //Privata variablar
        private Vector2 m_playerPosition;
        private Vector2 m_lastPosition;
        private Vector2 m_speed = new Vector2(0.15f, 0.2f);
        private Stopwatch m_hitWatch = new Stopwatch();
        private float m_visability = 1;

        //Publika variablar
        public int m_lives = 13;
        public bool m_isAlive = true;
        public bool m_isJumping = false;
        public bool m_isFalling = false;
        public bool m_gotHit;

        //Konstruktor
        public Player(Vector2 a_startPosition)
        {
            m_playerPosition = a_startPosition;
        }

        //Uppdaterar intern simulering
        internal void UpdateSimulation(float a_elapsedTime)
        {           
            if(m_hitWatch.ElapsedMilliseconds >= 1700 && m_gotHit)
            {
                m_gotHit = false;
                m_visability = 1f;
                m_hitWatch.Stop();
                m_hitWatch.Reset();
            }
            
        }

        //Publika get/set metoder
        public float PlayerPositionX
        {
            get { return m_playerPosition.X; }
            set { m_playerPosition.X = value; }
        }
        
        public float PlayerPositionY
        {
            get { return m_playerPosition.Y; }
            set { m_playerPosition.Y = value; }
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

        public float Visability
        {
            get { return m_visability; }
            set { m_visability = value; }
        }

        public Vector2 LastPosition
        {
            get { return m_lastPosition; }
            set { m_lastPosition = value; }
        }

        //Rörelse/Liv hantering
        internal void DoJump()
        {
            m_isJumping = true;
        }

        internal void MoveLeft()
        {
            m_playerPosition.X -= m_speed.X;
        }

        internal void MoveRight()
        {
            m_playerPosition.X += m_speed.X;
        }

        internal void MoveDown()
        {
            m_playerPosition.Y += m_speed.Y;
        }

        internal void MoveUp()
        {
            m_playerPosition.Y -= m_speed.Y;
        }

        internal void FallFree()
        {
            m_playerPosition.Y += m_speed.Y;
        }

        internal void LostLife()
        {
            if(!m_gotHit)
            {
                m_hitWatch.Start();
                m_gotHit = true;
                m_lives--;
                m_visability = 0.6f;
            }
        }

   }
}
