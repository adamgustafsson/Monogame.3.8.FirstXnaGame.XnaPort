using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace View
{
    //Klass för utritning av spelinformation
    class OverlayView
    {
        private SpriteFont[] m_fonts;
        private Texture2D[] m_textures;
        private SpriteBatch m_spriteBatch;

        public const int BLUE_HEART = 0;
        public const int BLACK_HEART = 1;
        
        //Konstruktor
        public OverlayView(SpriteBatch a_spriteBatch)
        {
            m_spriteBatch = a_spriteBatch; 
        }

        //Laddar gränssnitt & texturer
        internal void LoadContent(ContentManager a_content)
        {
            m_fonts = new SpriteFont[2] { a_content.Load<SpriteFont>("TimesNewRomanBold"),
                                          a_content.Load<SpriteFont>("TimesNewRomanWhite")};
            m_textures = new Texture2D[2] { a_content.Load<Texture2D>("Textures/Heart"),
                                               a_content.Load<Texture2D>("Textures/blackHeart")};
        }

        //Ritar ut antal liv samt ikon
        internal void DrawLife(SpriteBatch a_spriteBatch, string a_text, Vector2 a_position, int imgId)
        {
            Vector2 textPosition = a_position - new Vector2(590, 340);
            Vector2 heartPosition = textPosition - new Vector2(45, 0);
            Vector2 textPositionB = textPosition - new Vector2(2, 2);
            m_spriteBatch.Draw(m_textures[imgId],heartPosition, null,Color.White,0f,Vector2.Zero,0.5f,SpriteEffects.None,1f);
            m_spriteBatch.DrawString(m_fonts[1], a_text, textPosition, Color.White, 0f,Vector2.Zero,1f,SpriteEffects.None, 1f);
            m_spriteBatch.DrawString(m_fonts[0], a_text, textPositionB, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }
    }
    
}
