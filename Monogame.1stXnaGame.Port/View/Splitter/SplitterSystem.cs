using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{
    class SplitterSystem
    {
        private const int PARTICLES_PER_ARRAY = 150;

        private float m_visability = 1f;
        private Model.ObjectArray m_particleArray;
        private View.Particle m_particle;
        private Texture2D m_particleTexture;    
        private static Random m_rand = new Random();

        //Konstruktor; skapar ny partikelarray via ArrayKlass
        public SplitterSystem(Vector2 a_explosionPosition, float a_scale)
        {
            m_particleArray = new Model.ObjectArray(PARTICLES_PER_ARRAY);
            int i = 0;

            while (i < PARTICLES_PER_ARRAY)
            {
                Vector2 m_randomDirection = new Vector2((float)m_rand.NextDouble() - 0.5f, (float)m_rand.NextDouble() - 0.5f);
                m_randomDirection.Normalize();
                m_randomDirection = m_randomDirection * ((float)m_rand.NextDouble() * 0.2f);

                this.m_particle = new View.Particle(m_randomDirection, a_explosionPosition, a_scale);
                m_particleArray.Add(m_particle);
                i++;
            }

        }

        //Uppdatering/Ritfunktion för Explosionssplitter
        internal void UpdateAndDrawExplosionSplitter(float a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, Camera a_camera)
        {
            m_visability = m_visability - 0.04f;

            foreach (Particle particle in m_particleArray.Get())
            {
                particle.ParticlePositionX += particle.DirectionX;
                particle.ParticlePositionY += particle.DirectionY;

                float particleRadius = particle.GetWidth() / 2;

                Vector2 particleCoords = a_camera.VisualizeCord(particle.ParticlePositionX - particleRadius, particle.ParticlePositionY - particleRadius);

                int particleSize = a_camera.VisualizeObject(particleRadius);

                int vx = (int)particleCoords.X;
                int vy = (int)particleCoords.Y;
                int vw = particleSize;
                int vh = particleSize;

                Rectangle particleRectangel = new Rectangle(vx, vy, vw, vh);


                float a = m_visability;
                Color particleColor = new Color(a, a, a, a);

   
                a_spriteBatch.Draw(m_particleTexture, particleRectangel, particleColor); 
            }

        }

        //Uppdatering/Ritfunktion för default splitter
        internal void UpdateAndDrawDefaultSplitter(float a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, Camera a_camera)
        {
            foreach (Particle particle in m_particleArray.Get())
            {
                particle.ParticlePositionX += particle.DirectionX;
                particle.ParticlePositionY += particle.DirectionY;
                particle.ParticlePositionY += particle.Gravitation += 0.0005f;

                float particleRadius = particle.GetWidth() / 2;

                Vector2 particleCoords = a_camera.VisualizeCord(particle.ParticlePositionX - particleRadius, particle.ParticlePositionY - particleRadius);

                int particleSize = a_camera.VisualizeObject(particleRadius);

                int vx = (int)particleCoords.X;
                int vy = (int)particleCoords.Y;
                int vw = particleSize;
                int vh = particleSize;

                Rectangle particleRectangel = new Rectangle(vx, vy, vw, vh);

                //Ingen fade
                float a = 1f;
                Color particleColor = new Color(a, a, a, a);

                a_spriteBatch.Draw(m_particleTexture, particleRectangel, particleColor);
            }

        }

        //Laddar textur
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, string a_textureSrc)
        {
            m_particleTexture = a_content.Load<Texture2D>(a_textureSrc);
        }
    }
}
