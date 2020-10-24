using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Explosion.View.Explosion
{
    class FireParticle
    {
        //Statiska värden
        private static Random m_random;

        //State
        private Vector2 m_position;
        private Vector2 m_speed = new Vector2(0,0);
        private Vector2 m_gravity = new Vector2(0,0);
        private float m_size = (Model.Level.LEVEL_WIDTH + Model.Level.LEVEL_HEIGHT) / 30;
        private float m_lifetime = 0.4f;

        //Konstruktor
        public FireParticle(Vector2 a_startPosition, float a_scale, int a_seed)
        {
            m_random = new Random(a_seed);                                
            m_position = a_startPosition;
            m_size = m_size * a_scale;          
            m_gravity.X = ((float)m_random.NextDouble() - 0.5f) * 1f;
            m_gravity.Y = ((float)m_random.NextDouble() - 0.5f) * 1f; 

        }

        public bool IsAlive()
        {
            return m_lifetime > 0;
        }

        internal void Update(float a_elapsedTime)
        {

            m_lifetime -= a_elapsedTime;
            //Sätter partikelfart
            m_speed = m_speed + m_gravity * a_elapsedTime;

            
            if(m_lifetime > 1.8f)
            {
                //Påverkar partikelposition
                m_position = m_position + m_speed * a_elapsedTime;
                //Ökar partikelstorlek
                float lifePercent = a_elapsedTime / m_lifetime;
                m_size = m_size + lifePercent * (m_size * 10f);
            
            }
            else
            {
                //Påverkar partikelposition
                m_position = m_position - m_speed * a_elapsedTime;
                //Minskar partikelstorlek
                float lifePercent = a_elapsedTime / m_lifetime;
                m_size = m_size + lifePercent * (m_size * 0.10f);
            
            }



        }

        public float GetPositionX
        {
            get { return m_position.X; }
            set { m_position.X = value; }
        }

        public float GetPositionY
        {
            get { return m_position.Y; }
            set { m_position.Y = value; }
        }

        public float GetSize
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public float GetVisibility()
        {
            return 4f * m_lifetime;
        }
    }
}
