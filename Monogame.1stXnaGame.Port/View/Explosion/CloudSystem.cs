using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{
    class CloudSystem
    {
        private const int PARTICLES_PER_ARRAY = 100;
        private Model.ObjectArray m_particleArray;
        private CloudParticle m_particle;
        private Texture2D m_cloudTexture;

        //Konstruktor - skapar partikelarray via Arrayklass  
        public CloudSystem(Vector2 a_position, float a_scale)
        {
            m_particleArray = new Model.ObjectArray(PARTICLES_PER_ARRAY);
            int i = 0;

            while (i < PARTICLES_PER_ARRAY)
            {
                m_particle = new CloudParticle(a_position, a_scale, i);
                m_particleArray.Add(m_particle);
                i++;
            }
        }

        //Laddar textur
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_cloudTexture = a_content.Load<Texture2D>("Textures/particlesmoke");
        }

        //Uppdaterar och ritar
        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera)
        {
            foreach(CloudParticle cloudParticle in m_particleArray.Get())
            {
                cloudParticle.Update(a_elapsedTime);

                if (cloudParticle.IsAlive())
                {
                    //Visualiserar logiska kordinater för smoken
                    Vector2 cloudPosition = a_camera.VisualizeCord(cloudParticle.GetPositionX, cloudParticle.GetPositionY);
                    int particleSize = a_camera.VisualizeObject(cloudParticle.GetSize / 2);

                    int vx = (int)cloudPosition.X;
                    int vy = (int)cloudPosition.Y;
                    int vw = particleSize;
                    int vh = particleSize;

                    Rectangle particleRectangle = new Rectangle(vx, vy, vw, vh);

                    //Blenda ut färgen
                    float a = cloudParticle.GetVisibility();
                    Color particleColor = new Color(a, a, a, a);


                    float rotation = cloudParticle.GetSize * 4;
                    float scale = cloudParticle.GetSize;
                    Vector2 origin = new Vector2(m_cloudTexture.Width / 2, m_cloudTexture.Height / 2);

                    //rita ut partikeln
                    a_spriteBatch.Draw(m_cloudTexture, particleRectangle, null, particleColor, rotation, origin, SpriteEffects.None, 0.0f); 
                }
            }

        }
        


    }
}
