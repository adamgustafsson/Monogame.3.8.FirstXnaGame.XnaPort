using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace View
{    
    class Camera 
    {
        //Privata variablar
        private int m_windowWidth;
        private int m_windowHeight;
        private float m_scaleX;
        private float m_scaleY;

        private float displacementX = 0;
        private float displacementY = 0;


        //Konstruktor
        public Camera(Viewport a_viewport)
        {
            m_windowWidth = a_viewport.Width;
            m_windowHeight = a_viewport.Height;

            m_scaleX = a_viewport.Width / (Model.Level.LEVEL_WIDTH);
            m_scaleY = a_viewport.Height / (Model.Level.LEVEL_HEIGHT);
        }

        //Visualiserar bollens logiska kordinater
        public Vector2 VisualizeCord(float a_xCoord, float a_yCoord)
        {
            int visualX = (int)(displacementX + a_xCoord * (m_scaleX - displacementX * 2));
            int visualY = (int)(displacementY + a_yCoord * (m_scaleY - displacementY * 2));

            Vector2 a_coord = new Vector2(visualX, visualY);

            return a_coord;
        }

        //Gör om visuella kordinater till logiska
        public Vector2 LogicizeCord(float a_xCoord, float a_yCoord)
        {
            float logicalX = (a_xCoord - displacementX) / (m_scaleX);
            float logicalY = (a_yCoord - displacementY) / (m_scaleY);

            Vector2 a_coord = new Vector2(logicalX, logicalY);

            return a_coord;
        }

        //Visualiserar bollens logiska storlek till px
        public int VisualizeObject(float a_objectRadius)
        {
            float scale = (m_scaleX + m_scaleY) - displacementX * 2;

            int objSize = (int)((a_objectRadius) * scale);

            return objSize;
        }
    }
}
