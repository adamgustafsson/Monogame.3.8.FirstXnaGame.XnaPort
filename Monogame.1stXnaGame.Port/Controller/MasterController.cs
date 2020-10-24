using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace Controller
{
    //Mastercontroller
    public class MasterController : Microsoft.Xna.Framework.Game
    {
        //Privata variabler
        private GraphicsDeviceManager m_graphics;
        private Stopwatch m_watch = new Stopwatch();
        private SpriteBatch m_spriteBatch;

        private GameController m_gameController;
        private ScreenController m_screenController;
        private View.TextView m_textView;
        private Model.GameModel m_gameModel;

        public MasterController()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            m_graphics.PreferredBackBufferHeight = 720;
            m_graphics.PreferredBackBufferWidth = 1280;
            m_graphics.ApplyChanges();
            base.Initialize();
        }

        //Instansierar klasser samt laddar innehåll
        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_gameModel = new Model.GameModel();
            m_gameController = new GameController(GraphicsDevice, Content, m_gameModel);
            m_screenController = new ScreenController(m_gameModel,m_spriteBatch, Content);
            m_textView = new View.TextView(m_spriteBatch, Content);
            m_gameController.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime a_gameTime)
        {
            //Om en text/bildskärm visas
            if (m_screenController.IsShowingScreen())
            {
                //..updateras inte spelmotorn
                this.IsMouseVisible = true;
            }
            else
            {
                //Uppdaterar spelet via GameController
                this.IsMouseVisible = false;
                m_screenController.UpdateScreenSimulation((float)a_gameTime.ElapsedGameTime.TotalSeconds);
                m_gameController.UpdateSimulation((float)a_gameTime.ElapsedGameTime.TotalSeconds);
            }    

            base.Update(a_gameTime);
        }

        protected override void Draw(GameTime a_gameTime)
        {
            //Om en text/bildskärm skall ritas
            if (m_screenController.IsShowingScreen())
            {
                GraphicsDevice.Clear(Color.Black);

                //Ritar skärm
                m_screenController.DrawScreens((float)a_gameTime.ElapsedGameTime.TotalSeconds);

                //Spelar introlåt vid startskärm
                if(m_screenController.m_showStartScreen)
                {
                    m_gameController.PlayStartTheme();
                }
                //Avslutar på användarens begäran
                if (m_screenController.DoExit())
                {
                    this.Exit();
                }
                //Startar om spelet
                else if (m_screenController.RestartGame())
                {
                    LoadContent();
                }
            }
            //Annars ritas spelet via GameController    
            else
            {
                GraphicsDevice.Clear(Color.LightBlue);
                m_gameController.Draw((float)a_gameTime.ElapsedGameTime.TotalSeconds, m_spriteBatch);
            }
            base.Draw(a_gameTime);
        }
    }
}























//Om text/bildvyer skall visas..
//if (m_showStartScreen || m_showMapFinish || m_showPauseMenu || m_showGameOverScreen || m_showVictoryScreen)
//{
//    //..updateras inte spelmotorn
//}
////Användaren trycker escape
//else if (m_textView.DidPressEsc())
//{
//    m_showPauseMenu = true;
//}
////Bana avklarad
//else if (m_gameModel.MapCompleted())
//{
//    m_showMapFinish = true;
//    m_watch.Start();
//}
////Game over
//else if (m_gameModel.GameOver())
//{
//    m_showGameOverScreen = true;
//}
//else if (m_gameModel.GameCompleted())
//{
//    m_showVictoryScreen = true;
//    m_watch.Start();
//}

//int x = 640;
//int y = 320;
//int separation = 35;

//GraphicsDevice.Clear(Color.Black);

////Om startskärm skall visas
//if (m_showStartScreen)
//{
//    //Spela song för startskärm
//    m_gameController.PlayStartTheme();

//    //Ritar bild för startskärm
//    m_textView.DrawImage(View.TextView.LOGO, 0, 120);

//    //Ritar klickbar text
//    if (m_textView.DrawClickableText(Mouse.GetState(), "NEWGAME", x-100, y))
//    {
//        m_showStartScreen = false;
//        m_gameModel.StartNewGame();
//    }
//    if (m_textView.DrawClickableText(Mouse.GetState(), "EXIT", x-65, y += separation))
//    {
//        this.Exit();
//    }

//    m_textView.SetOldState(Mouse.GetState());

//}
////Om en bana är avklarad
//else if (m_showMapFinish)
//{
//    //Presenterar kommande bana, medan den laddas in
//    if (m_watch.ElapsedMilliseconds <= 1000)
//    {
//        m_textView.DrawText("LEVEL " + (m_gameModel.m_activeMapNr + 2).ToString(), x+100, y);
//        m_textView.DrawText("LOADING..", x+100, y + separation);
//    }
//    //Startar nästa bana
//    else
//    {
//        m_watch.Stop();
//        m_watch.Reset();
//        m_gameModel.StartNextLevel();
//        m_showMapFinish = false;
//    }
//}
////Om användare tryckt escape
//else if (m_showPauseMenu)
//{
//    //Rita ut klickbara menyval
//    if (m_textView.DrawClickableText(Mouse.GetState(), "CONTINUE", x-100, y))
//    {
//        m_showPauseMenu = false;
//    }
//    if (m_textView.DrawClickableText(Mouse.GetState(), "NEWGAME", x-87, y += separation))
//    {
//        m_showPauseMenu = false;
//        m_gameModel.StartNewGame();
//    }
//    if (m_textView.DrawClickableText(Mouse.GetState(), "EXIT", x - 52, y += separation))
//    {
//        this.Exit();
//    }

//    m_textView.SetOldState(Mouse.GetState());

//}
////Om spelaren förbrukat sina liv (GameOver)        
//else if (m_showGameOverScreen)
//{
//    m_textView.DrawImage(View.TextView.GAME_OVER, 395, 220);

//    //Ritar ut klickbara alternativ
//    if (m_textView.DrawClickableText(Mouse.GetState(), "CONTINUE", x-100, y))
//    {
//        m_showGameOverScreen = false;
//        m_gameModel.Continue();
//    }
//    if (m_textView.DrawClickableText(Mouse.GetState(), "EXIT", x-55, y += separation))
//    {
//        this.Exit();
//    }

//    m_textView.SetOldState(Mouse.GetState());
//}
////Om spelet är avklarat    
//else if (m_showVictoryScreen)
//{
//    m_textView.DrawImage(View.TextView.VICTORY, 395, 220);

//    if (m_watch.ElapsedMilliseconds >= 5000 && m_watch.ElapsedMilliseconds <= 10000)
//    {
//        m_textView.DrawText("congratulations!", x + 50, y);
//    }
//    if (m_watch.ElapsedMilliseconds >= 15000 && m_watch.ElapsedMilliseconds <= 20000)
//    {
//        m_textView.DrawText("madeby", x + 100, y);
//    }
//    if (m_watch.ElapsedMilliseconds >= 18000 && m_watch.ElapsedMilliseconds <= 25000)
//    {
//        m_textView.DrawText("adamgustafsson", x +50, y + separation);
//    }

//    if (m_watch.ElapsedMilliseconds >= 55000)
//    {
//        m_showVictoryScreen = false;
//        m_showStartScreen = true;
//        LoadContent();
//        m_watch.Stop();
//        m_watch.Reset();
//    }

//}