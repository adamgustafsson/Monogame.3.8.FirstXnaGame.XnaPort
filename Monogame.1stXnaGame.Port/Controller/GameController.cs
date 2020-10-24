using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Controller
{
    class GameController
    {
        //Privata variabler
        private View.GameView m_gameView;
        private View.Camera m_camera;
        private View.MapView m_mapView;
        private Model.GameModel m_gameModel;
        private ContentManager m_content;
        private int m_playerAnimation;

        public GameController(GraphicsDevice a_graphicDevice, ContentManager a_content, Model.GameModel a_gameModel)
        {
            //Instansierar 
            m_gameView = new View.GameView(a_graphicDevice);
            m_camera = new View.Camera(a_graphicDevice.Viewport);
            m_mapView = new View.MapView(a_content);
            m_gameModel = a_gameModel;
            m_content = a_content;

            //Laddar samtliga mappvägar samt läser in mapp 1(tiles)
            m_gameModel.LoadMapPaths(m_content.RootDirectory);
            m_gameModel.LoadActiveMap();
        }

        internal void LoadContent()
        {
            m_gameView.LoadContent(m_content);
        }

        //Hanterar användarinteraktion
        internal void UpdateSimulation(float a_elapsedTime)
        {
            //Bestämmer default spelaranimation
            m_playerAnimation = View.TextureAnimationSystem.FACE_CAMERA;

            //Hanterar interaktion "gå vänster"
            if (m_gameView.DidPressKey("A"))
            {
                if (m_gameModel.CanMoveLeft())
                {
                    m_gameModel.m_player.MoveLeft();
                }
                m_playerAnimation = View.TextureAnimationSystem.MOVE_LEFT;
            }

            //Hanterar interaktion "gå höger"
            if (m_gameView.DidPressKey("D"))
            {
                if (m_gameModel.CanMoveRight())
                {
                    m_gameModel.m_player.MoveRight();
                }

                m_playerAnimation = View.TextureAnimationSystem.MOVE_RIGHT;
            }

            ////Hanterar interaktion "gå ner"
            if (m_gameView.DidPressKey("S"))
            {
                if(m_gameModel.CanMoveDown())
                {
                    m_gameModel.m_player.MoveDown();
                    m_playerAnimation = View.TextureAnimationSystem.FACE_CAMERA;
                }
                
            }

            ////Hanterar interaktion "gå upp"
            if (m_gameView.DidPressKey("W"))
            {
                if (m_gameModel.CanMoveUp())
                {
                    m_gameModel.m_player.MoveUp();
                }
            }

            //Hanterar interaktion "hoppa"
            if (m_gameView.DidPressJump())
            {
                if (m_gameModel.CanJump())
                {
                    m_gameModel.m_player.DoJump();
                }
            }

            //Uppdaterar spelmotor
            m_gameModel.UpdateSimulation(a_elapsedTime);
        }
        
        //Ritar spelet via GameView
        internal void Draw(float a_elapsedTime, SpriteBatch a_spriteBatch)
        {
            m_gameView.Draw(m_gameModel, a_elapsedTime, m_mapView, m_playerAnimation);
        }

        //Extern metod, anropas av MasterController för uppspelning av ljud då spelmotorn inte körs
        internal void PlayStartTheme()
        {
            m_gameView.SoundHandler(m_gameModel);
        }

    }
}
