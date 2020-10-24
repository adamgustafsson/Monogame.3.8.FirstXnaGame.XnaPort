using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Model
{
    //Klass för skapande av tile-data, hämtat från .map fil
    class TileGenerator
    {
        private int[,] m_mapData;
        private int m_width;
        private int m_height;
        private List<Vector2> m_occupiedCoords;
        
        //Läser ut modellkordinater från .map fil
        public void GenerateMapTiles(string a_mapPath)
        {
            using (StreamReader sr = new StreamReader(a_mapPath))
            {
                //Skapar lista för ockuperade koordinater
                m_occupiedCoords = new List<Vector2>();

                int currentY = 0;

                //Så länge det finns en ny rad att läsa
                while (sr.Peek() > 0) 
                {
                    string line = sr.ReadLine(); //Läser rad

                    if (currentY == 0)//Första raden = dimensioner (antal x/y rader)
                    {
                        string[] dimensions = line.Split(','); //Skapar array enligt angivna dimensioner (ex 155,23)
                        m_width = int.Parse(dimensions[0]);
                        m_height = int.Parse(dimensions[1]);
                        m_mapData = new int[m_width, m_height];

                        //Ger alla koordinater värdet -1 (ingen tile)
                        for (int x = 0; x < m_width; x++)
                        {
                            for (int y = 0; y < m_height; y++)
                            {
                                m_mapData[x, y] = -1;
                            }
                        }
                    }
                    //om vi inte är på första raden    
                    else
                    {
                        int currentX = 0;

                        //Läser varje rad och byter ut -1 mot angivet nummer om sådant finns
                        foreach (char c in line.ToArray())
                        {
                            try
                            {
                                double numericCheck;
                                if (double.TryParse(c.ToString(), out numericCheck))
                                {
                                    //Minus ett pga av första raden är dimensioner
                                    m_mapData[currentX, currentY - 1] = int.Parse(c.ToString());


                                    if (currentX < m_mapData.GetLength(0) && currentY < m_mapData.GetLength(1))
                                    {
                                        //Om siffran fortfarande är -1 sparas den som ockuperad koordinat
                                        if (m_mapData[currentX, currentY] == -1)
                                        {
                                            m_occupiedCoords.Add(new Vector2(currentX, currentY - 1));
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var error = ex;
                            }

                            currentX++;
                        }
                    }

                    currentY++;
                }

            }
        }
        
        //Publika get metoder
        public int GetWidth
        {
            get { return m_width; }
        }

        public int GetHeight
        {
            get { return m_height; }
        }

        public int[,] GetMapData
        {
            get { return m_mapData; }
        }

        public List<Vector2> GetOccupiedCoords
        {
            get { return m_occupiedCoords; }
        }
    }
}
