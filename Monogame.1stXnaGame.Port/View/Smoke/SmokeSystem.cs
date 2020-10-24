using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{
    class SmokeSystem
    {
        private const int PARTICLES_PER_ARRAY = 10;
        private Model.ObjectArray m_particleArray;
        private SmokeParticle m_particle;
        private  Texture2D m_smokeTexture;

        //Konstruktor - skapar partikelarray via Arrayklass  
        public SmokeSystem(Vector2 a_position, float a_scale, bool a_isRespawning)
        {
            m_particleArray = new Model.ObjectArray(PARTICLES_PER_ARRAY);
            int i = 0;

            while (i < PARTICLES_PER_ARRAY)
            {
                m_particle = new SmokeParticle(a_position, a_scale, i, a_isRespawning);
                m_particleArray.Add(m_particle);
                i++;
            }
        }

        //Laddar textur
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_smokeTexture = a_content.Load<Texture2D>("Textures/particlesmoke");
        }

        //Uppdaterar och ritar
        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            foreach(SmokeParticle smokeParticle in m_particleArray.Get())
            {

                smokeParticle.Update(a_elapsedTime);


                if (smokeParticle.IsAlive())
                {
                    //Visualiserar logiska kordinater för smoken
                    Vector2 smokePosition = a_camera.VisualizeCord(smokeParticle.GetPositionX, smokeParticle.GetPositionY);
                    int particleSize = a_camera.VisualizeObject(smokeParticle.GetSize / 2);

                    int vx = (int)smokePosition.X;
                    int vy = (int)smokePosition.Y;
                    int vw = particleSize;
                    int vh = particleSize;

                    Rectangle particleRectangle = new Rectangle(vx, vy, vw, vh);

                    //Blenda ut färgen
                    float a = smokeParticle.GetVisibility() / 2;
                    Color particleColor = new Color(a, a, a, a);

                    float rotation = smokeParticle.GetSize * 4;                 
                    float scale = smokeParticle.GetSize;
                    Vector2 origin = new Vector2(m_smokeTexture.Width / 2, m_smokeTexture.Height / 2);

                    //rita ut partikeln
                    a_spriteBatch.Draw(m_smokeTexture, particleRectangle, null, particleColor, rotation, origin, SpriteEffects.None, 0.0f);
                }

            }

        }


    }
}
