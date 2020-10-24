using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace View
{
    class CloudParticle
    {
        //Statiska värden
        private static float LIFETIME_MAX = 5f;
        private static Random m_random;
        private static float m_visability = 0.6f;

        //State
        private float m_delay;
        private bool m_isAlive = true;
        private Vector2 m_position;
        private float m_timeLived = 0;
        private Vector2 m_speed = new Vector2(0,0);
        private Vector2 m_gravity = new Vector2(0, 0);     
        private float m_size = (Model.Level.LEVEL_WIDTH + Model.Level.LEVEL_HEIGHT) / 40;     

        //Konstruktor
        public CloudParticle(Vector2 a_startPosition, float a_scale, int a_seed)
        {
            m_random = new Random(a_seed);                               
            
            m_position = a_startPosition;
            m_size = m_size * a_scale;

            m_gravity.X = ((float)m_random.NextDouble()) * 5f;
            m_gravity.Y = ((float)m_random.NextDouble()) * 5f; 
                                  
        }

        public bool IsAlive()
        {
            return m_isAlive;
        }


        internal void Update(float a_elapsedTime)
        {
            m_visability = m_visability - 0.0005f;

            //Påbörjar fördröjning
            if (m_delay > 0) 
            {
                m_delay -= a_elapsedTime;
                return;
            }

            m_timeLived += a_elapsedTime;

            //Sätter partikelfart
            m_speed = m_speed + m_gravity * a_elapsedTime/2;

            //Påverkar partikelposition
            m_position = m_position + m_speed * a_elapsedTime; 

            //Ökar partikelstorlek
            float lifePercent = m_timeLived / LIFETIME_MAX;
            m_size = m_size + lifePercent * (m_size * 0.3f);

            if (m_visability < 0)
            {
                m_isAlive = false;
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
            return m_visability;
        }

    }
}






















    //    //Constants
    //    public static Vector2 DEFAULT_POSITION = new Vector2(Model.Level.LEVEL_WIDTH / 2, Model.Level.LEVEL_HEIGHT / 1.1f);
    //    public static float MAX_LIFETIMES = 1f;
    //    public static float MAX_DELAY = 5f;
       
    //    //State
    //    private float m_delay;
    //    private float m_lifeLived;
    //    private float m_size = (Model.Level.LEVEL_WIDTH + Model.Level.LEVEL_HEIGHT) / 20;
        
    //    private Vector2 m_position;
    //    private Vector2 m_particleSpeed = new Vector2(0, 0f);    
    //    private Vector2 m_gravity = new Vector2(0, -1f);    



    //    public SmokeParticle(Vector2 a_randomXgravitation, int a_randomSeed)
    //    {
    //        m_position = DEFAULT_POSITION;
    //        m_lifeLived = 0;
    //        //m_gravity.X = a_randomXgravitation.X;

    //        Random r = new Random(a_randomSeed);
    //        m_delay = (float)r.NextDouble() * MAX_DELAY;
    //    }

    //    private void Respawn(int a_randomSeed)
    //    {       
    //        m_lifeLived = 0;
    //        m_position = DEFAULT_POSITION;
    //        m_particleSpeed = new Vector2(0.0f, -1f);

    //    }

    //    public bool IsAlive()
    //    {
    //        return m_lifeLived > 0;
    //    }

    //    internal void Update(float a_elapsedTime, int a_randomSeed)
    //    {
    //        //if (m_delay > 0)
    //        //{
    //        //    m_delay -= a_elapsedTime;
    //        //    return;
    //        //}
            
    //        //v1 = v0 + a *t
    //        m_particleSpeed = m_particleSpeed + m_gravity * a_elapsedTime;

    //        //s1 = s0 + var * t
    //        m_position = m_position + m_particleSpeed * a_elapsedTime;

    //        m_lifeLived += a_elapsedTime;


    //        //Ökar storleken på röken
    //        float lifePercent = m_lifeLived / MAX_LIFETIMES;
    //        m_size = 0.1f + lifePercent * 10f;

    //        //Respawnar om lifeLived överstiger 1
    //        if (m_lifeLived > MAX_LIFETIMES)
    //        {
    //            if (m_delay > 0)
    //            {
    //                m_delay -= a_elapsedTime;
    //                return;
    //            }
    //            Respawn(a_randomSeed);
    //        }




    //    }

        
    //    public float GetVisibility()
    //    {
    //        return m_lifeLived * MAX_LIFETIMES;
    //    }

    //    public float GetPositionX
    //    {
    //        get { return m_position.X; }
    //        set { m_position.X = value; }
    //    }

    //    public float GetPositionY
    //    {
    //        get { return m_position.Y; }
    //        set { m_position.Y = value; }
    //    }
        


    //    public float GetSize
    //    {
    //        get { return m_size; }
    //        set { m_size = value; }
    //    }

    //}

//Random r = new Random(a_randomSeed);
//m_delay = (float)r.NextDouble() * MAX_DELAY;