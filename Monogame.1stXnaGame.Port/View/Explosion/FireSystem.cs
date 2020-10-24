using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Explosion.View.Explosion;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace View
{
    class FireSystem
    {
        //Privata variabler
        private const int PARTICLES_PER_ARRAY = 10;
        private Model.ObjectArray m_particleArray;
        private FireParticle m_particle;
        private Texture2D m_fireTexture;

        //Konstruktor: skapar array
        public FireSystem(Microsoft.Xna.Framework.Vector2 a_position, float a_scale)
        {
            m_particleArray = new Model.ObjectArray(PARTICLES_PER_ARRAY);
            int i = 0;

            while (i < PARTICLES_PER_ARRAY)
            {
                m_particle = new FireParticle(a_position, a_scale, i);
                m_particleArray.Add(m_particle);
                i++;
            }
        }

        //Laddar textur
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_fireTexture = a_content.Load<Texture2D>("Textures/spark");
        }


        internal void UpdateAndDraw(float a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, View.Camera a_camera)
        {
            foreach (FireParticle fireParticle in m_particleArray.Get())
            {

                fireParticle.Update(a_elapsedTime);

                if (fireParticle.IsAlive())
                {
                    //Visualiserar logiska kordinater för smoken
                    Vector2 firePosition = a_camera.VisualizeCord(fireParticle.GetPositionX, fireParticle.GetPositionY);
                    int particleSize = a_camera.VisualizeObject(fireParticle.GetSize / 2);

                    int vx = (int)firePosition.X;
                    int vy = (int)firePosition.Y;
                    int vw = particleSize;
                    int vh = particleSize;

                    Rectangle particleRectangle = new Rectangle(vx, vy, vw, vh);

                    //Blenda ut färgen
                    float a =  fireParticle.GetVisibility();
                    Color particleColor = new Color(a, a, a, a);


                    float rotation = fireParticle.GetSize * 4;
                    float scale = fireParticle.GetSize;
                    Vector2 origin = new Vector2(m_fireTexture.Width / 2, m_fireTexture.Height / 2);

                    //rita ut partikeln
                    a_spriteBatch.Draw(m_fireTexture, particleRectangle, null, particleColor, rotation, origin, SpriteEffects.None, 0.0f);  
                }

            }
        }
    }
}
