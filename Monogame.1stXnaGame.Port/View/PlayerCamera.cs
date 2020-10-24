using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{
    //Kamera klass dedikerad för att följa spelaren
    class PlayerCamera
    {
        protected float m_zoom;      // Camera Zoom
        public Matrix   m_transform; // Matrix Transform
        public Vector2  m_pos;       // Camera Position
        protected float m_rotation;  // Camera Rotation
 
        public PlayerCamera()
        {
            m_zoom = 1.0f;
            m_rotation = 0.0f;
            m_pos = Vector2.Zero;
        }

        //Sätter och hämtar zoom
        public float Zoom
        {
            get { return m_zoom; }
            set { m_zoom = value; if (m_zoom < 0.1f) m_zoom = 0.1f; } // Negativ zoom flippar kameran
        }
 
        //Sätter och hämtar rotation
        public float Rotation
        {
            get {return m_rotation; }
            set { m_rotation = value; }
        }

        //Hämtar och sätter centerposition
        public Vector2 Pos
        {
             get{ return  m_pos; }
             set{ m_pos = value; }
        }

        //Gör kamerauträkning enligt tutorial http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
        public Matrix get_transformation(Viewport a_viewport)
        {
            m_transform =                Matrix.CreateTranslation(new Vector3(-m_pos.X, -m_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(a_viewport.Width * 0.5f, a_viewport.Height * 0.5f, 0));
            return m_transform;
        }
    }
}
