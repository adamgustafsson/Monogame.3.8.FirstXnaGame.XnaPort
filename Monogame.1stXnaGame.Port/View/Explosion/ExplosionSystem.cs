using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Explosion.View.Explosion;
using Microsoft.Xna.Framework.Audio;

namespace View
{
    class ExplosionSystem
    {
        //Privata variabler
        private CloudSystem m_cloudSystem;
        private FireSystem m_fireSystem;
        private SplitterSystem m_splitterSystem;
        private SmokeSystem m_smokeSystem;
        private Vector2 m_explosionPosition;
        private float m_scale;

        //Konstruktor
        public ExplosionSystem(Vector2 a_position, float a_scale)
        {

            m_explosionPosition = a_position;
            m_scale = a_scale;
            
            //Skapar de partikelsystem som skall ingå i explosionen
            m_cloudSystem = new CloudSystem(m_explosionPosition, m_scale);
            m_fireSystem = new FireSystem(m_explosionPosition, m_scale);
            m_splitterSystem = new SplitterSystem(m_explosionPosition, m_scale);
            m_smokeSystem = new SmokeSystem(m_explosionPosition, m_scale, false);
        }

        //Laddar innehåll
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_cloudSystem.LoadContent(a_content);
            m_fireSystem.LoadContent(a_content);
            m_splitterSystem.LoadContent(a_content, "Textures/explosion");
            m_smokeSystem.LoadContent(a_content);

        }

        //Uppdaterar och ritar
        internal void UpdateAndDraw(float a_elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch a_spriteBatch, View.Camera a_camera)
        {
            m_splitterSystem.UpdateAndDrawExplosionSplitter(a_elapsedTime, a_spriteBatch, a_camera);
            m_splitterSystem.UpdateAndDrawDefaultSplitter(a_elapsedTime, a_spriteBatch, a_camera);
            m_cloudSystem.UpdateAndDraw(a_elapsedTime, a_spriteBatch, a_camera);
            m_fireSystem.UpdateAndDraw(a_elapsedTime, a_spriteBatch, a_camera);
            //m_smokeSystem.UpdateAndDraw(a_elapsedTime, a_spriteBatch, a_camera);
        }

    }
}
