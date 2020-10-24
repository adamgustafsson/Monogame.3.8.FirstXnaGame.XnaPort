using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Controller
{
    class ScreenController
    {
        private Model.GameModel m_gameModel;
        private View.TextView m_textView;
        private Stopwatch m_watch = new Stopwatch();

        private bool m_doExit;
        private bool m_restartGame;
        private bool m_backToStart = false;

        //Variabler för bild/texthantering
        public bool m_showStartScreen = true;
        private bool m_showPauseMenu = false;
        private bool m_showMapFinish = false;
        private bool m_showControlls = false;
        private bool m_showGameOverScreen = false;
        private bool m_showVictoryScreen = false;

        public ScreenController(Model.GameModel a_gameModel, SpriteBatch a_spriteBatch, ContentManager a_content)
        {
            m_gameModel = a_gameModel;
            m_textView = new View.TextView(a_spriteBatch, a_content);
        }

        internal void UpdateScreenSimulation(float a_elapsedTime)
        {
            //Användaren trycker escape
            if (m_textView.DidPressEsc())
            {
                m_showPauseMenu = true;
            }
            //Bana avklarad
            else if (m_gameModel.MapCompleted())
            {
                m_showMapFinish = true;
                m_watch.Start();
            }
            //Game over
            else if (m_gameModel.GameOver())
            {
                m_showGameOverScreen = true;
            }
            //Spelet avklarat    
            else if (m_gameModel.GameCompleted())
            {
                m_showVictoryScreen = true;
                m_watch.Start();
            }
        }

        internal void DrawScreens(float a_elapsedTime)
        {
            int x = 640;
            int y = 320;
            int separation = 40;

            var back = "BaCk";
            var controls = "cOntRoLs";
            var newGame = "nEwGaMe";
            var continueT = "cOnTiNue";
            var exit = "eXit";
            var level = "leVeL";
            var loading = "lOaDiNg";
            var playAgain = "pLaY aGain";

            if (true)
            {
                back = "BaCk".ToUpper();
                controls = "cOntRoLs".ToUpper();
                newGame = "nEwGaMe".ToUpper();
                continueT = "cOnTiNue".ToUpper();
                exit = "eXit".ToUpper();
                level = "leVeL".ToUpper();
                loading = "lOaDiNg".ToUpper();
                playAgain = "pLaY aGain".ToUpper();
            }


            //Skärm för kontroll info
            if (m_showControlls)
            {             
                m_textView.DrawImage(View.TextView.CONTROLLS, 1, 85);

                if (m_textView.DrawClickableText(Mouse.GetState(), back, x - 74, y += separation + 20))
                {
                    m_showControlls = false;
                }

                m_textView.SetOldState(Mouse.GetState());

            }
            //Ritar startskärm
            else if (m_showStartScreen)
            {
                //Ritar bild för startskärm
                m_textView.DrawImage(View.TextView.LOGO, 0, 135);

                //Ritar klickbar text
                if (m_textView.DrawClickableText(Mouse.GetState(), controls, x-148, y)) // 296px
                {
                    m_showControlls = true;
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), newGame, x-129, y += separation)) // 259px
                {
                    m_showStartScreen = false;
                    m_gameModel.StartNewGame();
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), exit, x-74, y += separation)) // 148px
                {
                    m_doExit = true;
                }

                m_textView.SetOldState(Mouse.GetState());

            }
            //Om en bana är avklarad
            else if (m_showMapFinish)
            {
                //Presenterar kommande bana, medan den laddas in
                if (m_watch.ElapsedMilliseconds <= 4000)
                {
                    m_textView.DrawText(level + " " + (m_gameModel.m_activeMapNr + 2).ToString(), x + 100, y);
                    m_textView.DrawText(loading, x + 100, y + separation);
                }
                //Startar nästa bana
                else
                {
                    m_watch.Stop();
                    m_watch.Reset();
                    m_gameModel.StartNextLevel();
                    m_showMapFinish = false;
                }
            }
            //Om användare tryckt escape
            else if (m_showPauseMenu)
            {
                m_textView.DrawImage(View.TextView.PAUSE, 338, 180);

                //Rita ut klickbara menyval
                if (m_textView.DrawClickableText(Mouse.GetState(), continueT, x - 148, y))
                {
                    m_showPauseMenu = false;
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), controls, x - 148, y += separation))
                {
                    m_showControlls = true;
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), newGame, x - 129, y += separation))
                {
                    m_showPauseMenu = false;
                    m_gameModel.StartNewGame();
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), exit, x - 74, y += separation))
                {
                    m_doExit = true;
                }

                m_textView.SetOldState(Mouse.GetState());

            }
            //Om spelaren förbrukat sina liv (GameOver)        
            else if (m_showGameOverScreen)
            {
                m_textView.DrawImage(View.TextView.GAME_OVER, 395, 220);

                //Ritar ut klickbara alternativ
                if (m_textView.DrawClickableText(Mouse.GetState(), continueT, x - 100, y))
                {
                    m_showGameOverScreen = false;
                    m_gameModel.Continue();
                }
                if (m_textView.DrawClickableText(Mouse.GetState(), exit, x - 55, y += separation))
                {
                    m_doExit = true;
                }

                m_textView.SetOldState(Mouse.GetState());
            }
            //Om spelet är avklarat    
            else if (m_showVictoryScreen)
            {
                m_textView.DrawImage(View.TextView.VICTORY, 345, 200);

                if (m_watch.ElapsedMilliseconds >= 7500 && m_watch.ElapsedMilliseconds <= 17000)
                {
                    m_textView.DrawText("mAde bY", x + 120, y);
                }
                if (m_watch.ElapsedMilliseconds >= 9500 && m_watch.ElapsedMilliseconds <= 17000)
                {
                    m_textView.DrawText("aDam gUstaFsSon", x + 25, y + separation);
                }
                if (m_watch.ElapsedMilliseconds >= 18000)
                {
                    if (m_textView.DrawClickableText(Mouse.GetState(), playAgain, x - 110, y))
                    {
                        m_showVictoryScreen = false;
                        m_gameModel.StartNewGame();
                    }
                    if (m_textView.DrawClickableText(Mouse.GetState(), exit, x - 52, y += separation))
                    {
                        m_doExit = true;
                    }
                }

                //Startar om spelet efter 55 sekunder
                if (m_watch.ElapsedMilliseconds >= 55000)
                {
                    m_showVictoryScreen = false;
                    m_showStartScreen = true;
                    m_restartGame = true;
                    m_watch.Stop();
                    m_watch.Reset();
                }

                m_textView.SetOldState(Mouse.GetState());
            }
            else
            {
                m_doExit = false;
                m_restartGame = false;
            }

        }

        //Retunerar true om anv väljer Exit 
        public bool DoExit()
        {
            return m_doExit;
        }

        //Retunerar true om en text/bild skärm skall visas
        public bool IsShowingScreen()
        {
            return m_showStartScreen ||
                   m_showControlls ||
                   m_showMapFinish ||
                   m_showPauseMenu ||
                   m_showGameOverScreen ||
                   m_showVictoryScreen;
        }

        //Retunerar true om anv vill starta ett nytt spel
        public bool RestartGame()
        {
            return m_restartGame;
        }
    }


}
