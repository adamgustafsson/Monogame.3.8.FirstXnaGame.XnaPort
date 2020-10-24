using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace View
{
    //Klass för texturobject
    public class Texture
    {
        //Privata variabler
        private int m_framecountX;
        private int m_framecountY;
        private Texture2D m_myTexture;
        private float m_timePerFrame;
        private int m_previousFrameX;
        private int m_frameX;
        private int m_frameY;
        private float m_totalElapsed;

        public float m_rotation, m_scale, m_depth;
        public Vector2 m_origin;

        //Konstruktor
        public Texture(Vector2 a_origin, float a_rotation, float a_scale, float a_depth)
        {
            this.m_origin = a_origin;
            this.m_rotation = a_rotation;
            this.m_scale = a_scale;
            this.m_depth = a_depth;
        }
        
        //Laddar textur, sätter framecount
        public void Load(ContentManager a_content, string a_asset, int a_frameX, int a_frameY, int a_framesPerSec)
        {
            m_framecountX = a_frameX;
            m_framecountY = a_frameY;
            m_myTexture = a_content.Load<Texture2D>(a_asset);
            m_timePerFrame = (float)0.5 / a_framesPerSec;
            m_previousFrameX = 1;
            m_frameX = 1;
            m_frameY = 0;
            m_totalElapsed = 0;
        }

        //Simulerar gång åt höger
        public void WalkRight(float a_elapsed)
        {
            m_frameY = 2;

            m_frameX = m_previousFrameX;

            m_totalElapsed += a_elapsed;
              
            if (m_totalElapsed > m_timePerFrame)
            {
                m_frameX++;                 //Ökar position i x-led

                if (m_frameX == 3)          //Om x har flyttat 4 gånger..
                {
                    m_frameX = 0;           //nollställ x..
                }

                m_previousFrameX = m_frameX;
                m_totalElapsed -= m_timePerFrame;
            }
        }

        //Simulerar gång åt vänster
        public void WalkLeft(float a_elapsed)
        {
            m_frameY = 1;

            m_frameX = m_previousFrameX;

            m_totalElapsed += a_elapsed;

            if (m_totalElapsed > m_timePerFrame)
            {
                m_frameX--;                 //Ökar position i x-led

                if (m_frameX == -1)          //Om x har flyttat 4 gånger..
                {
                    m_frameX = 2;           //nollställ x..
                }

                m_previousFrameX = m_frameX;
                m_totalElapsed -= m_timePerFrame;
            }
        }

        //Framsida mot kamera
        public void FaceCamera(float a_elapsed)
        {
            m_frameY = 0;
            m_frameX = 1;
        }

        //Simulerar gång uppåt(klättra)
        public void Climb(float a_elapsed)
        {
            m_frameY = 3;

            m_totalElapsed += a_elapsed;

            if (m_totalElapsed > m_timePerFrame)
            {
                m_frameX++;                 //Ökar position i x-led

                if (m_frameX == 3)          //Om x har flyttat 4 gånger..
                {
                    m_frameX = 0;           //nollställ x..
                }

                m_totalElapsed -= m_timePerFrame;
            }
        }

        //Frame för död fiende
        public void DeadEnemy(float a_elapsed)
        {
            m_frameY = 3;
            m_frameX = 1;
        }

        public void DrawFrame(SpriteBatch a_batch, Vector2 a_screenPos, Color a_color)
        {
            DrawFrame(a_batch, m_frameX, m_frameY, a_screenPos, a_color);
        }

        //Ritar ut animationen
        public void DrawFrame(SpriteBatch a_batch, int a_frameX, int a_frameY, Vector2 a_screenPos, Color a_color)
        {
            int FrameWidth = m_myTexture.Width / m_framecountX;
            int FrameHeight = m_myTexture.Height / m_framecountY;
            Vector2 screenPosition;

            //Subtraherar positionen med 16px i x,y-led för att dess mittpunkt skall motsvara samma mittpunkt som för en 32x32 textur/rektangel
            if (m_scale > 1)
            {
                screenPosition = new Vector2(a_screenPos.X - FrameWidth / 2, a_screenPos.Y - FrameHeight / 2);
            }
            else
            {
                screenPosition = new Vector2(a_screenPos.X, a_screenPos.Y);
            }

            Rectangle sourcerect = new Rectangle(FrameWidth * a_frameX, FrameHeight * a_frameY, FrameWidth, FrameHeight);

            a_batch.Draw(m_myTexture, screenPosition, sourcerect, a_color, m_rotation, m_origin, m_scale, SpriteEffects.None, m_depth);
        }

        public void Reset()
        {
            m_frameX = 0;
            m_totalElapsed = 0f;
        }


    }
}
