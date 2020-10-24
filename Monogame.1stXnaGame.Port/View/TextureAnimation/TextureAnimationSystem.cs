using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{
    //Klass för hantering av frameanimationer.
    //Struktur enligt http://msdn.microsoft.com/en-us/library/bb203866.aspx)
    class TextureAnimationSystem
    {
        private Texture m_spriteTexture;
        private const float m_rotation = 0;
        private const float m_depth = 0.5f;

        private const int m_framesX = 3;
        private const int m_framesY = 4;
        private int m_framesPerSec;
        private string m_textureName;

        //Publika konstanter för animerings id
        public const int FACE_CAMERA = 1;
        public const int MOVE_LEFT = 2;
        public const int MOVE_RIGHT = 3;
        public const int CLIMB = 4;
        public const int DEAD_ENEMY = 5;

        //Konstruktor - skapar nytt TexturObjekt
        public TextureAnimationSystem(string a_textureName, float a_scale, int a_framesPerSec)  
        {
            m_framesPerSec = a_framesPerSec;
            m_textureName = a_textureName;
            m_spriteTexture = new Texture(Vector2.Zero, m_rotation, a_scale, m_depth);
        }

        //Anropar "Load" i texturklassen (skickar med texturnamn samt frame-egenskaper)
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content, Viewport a_viewPort)
        {
            m_spriteTexture.Load(a_content, m_textureName, m_framesX, m_framesY, m_framesPerSec);
        }

        //Uppdaterar och ritar via UpdateFrame & DrawFrame
        internal void UpdateAndDraw(float a_elapsedTime, SpriteBatch a_spriteBatch, Camera a_camera, Color a_color, Vector2 a_texturePos, int a_animation)
        {
            switch(a_animation)
            {
                case FACE_CAMERA:
                    m_spriteTexture.FaceCamera(a_elapsedTime);
                    break;
                case MOVE_LEFT:
                    m_spriteTexture.WalkLeft(a_elapsedTime);
                    break;
                case MOVE_RIGHT:
                    m_spriteTexture.WalkRight(a_elapsedTime);
                    break;
                case CLIMB:
                    m_spriteTexture.Climb(a_elapsedTime);
                    break;
                case DEAD_ENEMY:
                    m_spriteTexture.DeadEnemy(a_elapsedTime);
                    break;
            
            }

            m_spriteTexture.DrawFrame(a_spriteBatch, a_texturePos, a_color);

        }

    }
}
