using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace View
{
    //Klass för utritning av map
    class MapView
    {
        private Texture2D[] m_textures;
        private Texture2D[] m_backgrounds;

        //Konstruktor; Laddar testurer
        public MapView(ContentManager a_content)
        {
            m_textures = new Texture2D[3] { a_content.Load<Texture2D>("MapTextures/map1texture"),
                                            a_content.Load<Texture2D>("MapTextures/map2Texture"),
                                            a_content.Load<Texture2D>("MapTextures/map3Texture")};

            m_backgrounds = new Texture2D[3] { a_content.Load<Texture2D>("Backgrounds/bg1"),
                                               a_content.Load<Texture2D>("Backgrounds/bg2"),
                                               a_content.Load<Texture2D>("Backgrounds/bg5")};
        }

        //Ritar ut aktiv map
        public void DrawMap(SpriteBatch a_spriteBatch, Model.TileGenerator a_tileGenerator, int a_mapNumber)
        {
            for (int x = 0; x < a_tileGenerator.GetWidth; x++)
            {
                for (int y = 0; y < a_tileGenerator.GetHeight; y++)
                {
                    //Om en tile skall ritas (datan är inte -1)
                    if (a_tileGenerator.GetMapData[x, y] != -1)
                    {
                        //Resterande data är mellan 1-4 
                        int sourceY = 32 * a_tileGenerator.GetMapData[x, y]; //(y-led på textur = 0-128)
                        Rectangle src = new Rectangle(0, sourceY, 32, 32); //Källa på textur = 0,0-128 Storlek = 32x32
                        Rectangle dest = new Rectangle(x*32, y*32,32,32); //Destination modell-koordinat * 32, storlek 32x32
                        a_spriteBatch.Draw(m_textures[a_mapNumber], dest, src, Color.White);
                    }

                }
            }
        }

        //Ritar bakgrund beroende på aktiv map
        public void DrawBackground(SpriteBatch a_spriteBatch, int a_mapNumber)
        {
            a_spriteBatch.Begin();

            a_spriteBatch.Draw(m_backgrounds[a_mapNumber], Vector2.Zero, Color.White);

            a_spriteBatch.End();
        }
    } 
}
