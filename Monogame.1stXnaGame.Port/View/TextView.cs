using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace View
{
    //Klass för utritning av klickbara/icke klickbara skärmtexter samt skärmbilder
    class TextView
    {
        private static int HALF_WIDTH = 100;
        private static int HALF_HEIGHT = 15;

        private SpriteFont m_segoeKeycaps;
        private Texture2D[] m_textures;
        private SpriteBatch m_spriteBatch;
        private MouseState m_oldState;
        private bool m_didClick;

        //Publika konstanter för bild-id
        public const int LOGO = 0;
        public const int GAME_OVER = 1;
        public const int VICTORY = 2;
        public const int CONTROLLS = 3;
        public const int PAUSE = 4;

        //Konstruktor; laddar gränssnitt & bilder
        public TextView(SpriteBatch a_spriteBatch, ContentManager a_content)
        {
            m_spriteBatch = a_spriteBatch;
            m_segoeKeycaps = a_content.Load<SpriteFont>("SegoeKeycaps");
            m_textures = new Texture2D[5] {a_content.Load<Texture2D>("Textures/Logo"),
                                            a_content.Load<Texture2D>("Textures/gameover"),
                                            a_content.Load<Texture2D>("Textures/victoryImg"),
                                            a_content.Load<Texture2D>("Textures/Controlls"),
                                            a_content.Load<Texture2D>("Textures/pause2")};
        }

        //Ritar ut klickbar text
        internal bool DrawClickableText(MouseState a_mouseState, string a_text, int a_centerPosX, int a_centerPosY)
        {
            bool mouseOver = false;
            bool wasClicked = false;

            //Om musen är över texten
            if ((a_centerPosX - a_mouseState.X + 80) * (a_centerPosX - a_mouseState.X + 80) < HALF_WIDTH * HALF_WIDTH &&
                (a_centerPosY - a_mouseState.Y + 7) * (a_centerPosY - a_mouseState.Y + 7) < HALF_HEIGHT * HALF_HEIGHT)
            {
                mouseOver = true;

                //Vid klick
                if (a_mouseState.LeftButton == ButtonState.Released && m_oldState.LeftButton == ButtonState.Pressed)
                {
                    wasClicked = true;
                }
            }

            //Textposition
            Vector2 position = new Vector2(a_centerPosX, a_centerPosY);

            //Ritar
            m_spriteBatch.Begin();

            if (mouseOver)
            {
                //Vid klick
                if (a_mouseState.LeftButton == ButtonState.Pressed)
                {
                    m_spriteBatch.DrawString(m_segoeKeycaps, a_text, position, Color.CornflowerBlue);
                }
                //Vid hover
                else
                {
                    m_spriteBatch.DrawString(m_segoeKeycaps, a_text, position, Color.CornflowerBlue);
                }
            }
            //Default
            else
            {
                m_spriteBatch.DrawString(m_segoeKeycaps, a_text, position, Color.LightGray);
            }

            m_spriteBatch.End();

            return wasClicked;
        }

        //Ritar icke klickbar text
        internal void DrawText(string a_text, int a_centerPosX, int a_centerPosY)
        {
            m_spriteBatch.Begin();

            Vector2 position = new Vector2(a_centerPosX - 200, a_centerPosY);

            m_spriteBatch.DrawString(m_segoeKeycaps, a_text, position, Color.White);

            m_spriteBatch.End();
        }

        //Ritar angiven bild
        internal void DrawImage(int a_imgId, int a_centerPosX, int a_centerPosY)
        {
            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_textures[a_imgId], new Vector2(a_centerPosX, a_centerPosY), Color.White);

            m_spriteBatch.End();
        }

        internal void SetOldState(MouseState a_mouseState)
        {
            m_oldState = a_mouseState;
        }

        internal bool DidPressEsc()
        {
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                m_didClick = true;
                return false;
            }

            if (m_didClick == true && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                m_didClick = false;
                return true;
            }

            return false;
        }
    }
}
