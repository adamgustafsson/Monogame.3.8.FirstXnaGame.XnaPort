using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Explosion.View;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace View
{
    //Huvudklass för utritning av spelets komponenter
    class GameView
    {
        //Privata variabler      
        private Camera m_camera;
        private PlayerCamera m_cam; 
        private Viewport m_viewPort;
        private OverlayView m_overlay;
        private ContentManager m_content;
        private SpriteBatch m_spriteBatch;
        private SoundSystem m_soundsSystem;
        private ExplosionSystem m_explosion;
        private Stopwatch m_watch = new Stopwatch();
        private Stopwatch m_explosionWatch = new Stopwatch();
        
        private bool m_didClick;
        private bool m_previousBossHitState;
        private bool m_previousPlayerHitState;

        //Instanser av animationssystem
        private TextureAnimationSystem m_playerTextureSystem = new TextureAnimationSystem("Sprites/playerTexture", 2.0f, 6);
        private TextureAnimationSystem m_enemyOneTextureSystem = new TextureAnimationSystem("Sprites/enemy1texture", 0.5f, 1);
        private TextureAnimationSystem m_enemyTwoTextureSystem = new TextureAnimationSystem("Sprites/enemy2texture", 1.5f, 3);
        private TextureAnimationSystem m_enemyThreeTextureSystem = new TextureAnimationSystem("Sprites/enemy3texture", 1.7f, 1);
        private TextureAnimationSystem m_bossATextureSystem = new TextureAnimationSystem("Sprites/bossA", 2.0f, 6);

        //Konstruktor
        public GameView(GraphicsDevice a_graphicDevice)
        {
            m_viewPort = a_graphicDevice.Viewport;
            m_camera = new Camera(a_graphicDevice.Viewport);
            m_spriteBatch = new SpriteBatch(a_graphicDevice);
            m_soundsSystem = new SoundSystem();
            m_cam = new PlayerCamera();
            m_overlay = new OverlayView(m_spriteBatch);          
        }
        //Laddar samtligt innehåll
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_playerTextureSystem.LoadContent(a_content, m_viewPort);
            m_enemyOneTextureSystem.LoadContent(a_content, m_viewPort);
            m_enemyTwoTextureSystem.LoadContent(a_content, m_viewPort);
            m_enemyThreeTextureSystem.LoadContent(a_content, m_viewPort);
            m_bossATextureSystem.LoadContent(a_content, m_viewPort);
            m_soundsSystem.LoadContent(a_content);
            m_overlay.LoadContent(a_content);
            m_content = a_content;
        }

        //Huvudmetod för utritning av spelet
        internal void Draw(Model.GameModel a_gameModel, float a_elapsedTime, MapView a_mapView, int a_playerAnimation)
        {
            //Visualiserar spelarens koordinater
            Vector2 playerCoords = m_camera.VisualizeCord(a_gameModel.m_player.PlayerPositionX, a_gameModel.m_player.PlayerPositionY);

            //Sätter visuella kameraregler
            SetCameraRules(playerCoords, a_gameModel);

            //Hanterar ljud
            SoundHandler(a_gameModel);

            //Ritar bakgrund
            a_mapView.DrawBackground(m_spriteBatch, a_gameModel.m_activeMapNr);

            //Ritar resterande komponenter
            m_spriteBatch.Begin(SpriteSortMode.FrontToBack,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        m_cam.get_transformation(m_viewPort));

            //Ritar map
            a_mapView.DrawMap(m_spriteBatch, a_gameModel.m_tileGenerator, a_gameModel.m_activeMapNr);

            //Ritar spelare
            float a = a_gameModel.m_player.Visability;
            Color playerColor = new Color(a,a,a,a);
            m_playerTextureSystem.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera, playerColor, playerCoords, a_playerAnimation);

            //Ritar fiender
            DrawEnemies(a_gameModel, a_elapsedTime);

            //Ritar overlay
            m_overlay.DrawLife(m_spriteBatch, a_gameModel.m_player.m_lives.ToString(), m_cam.Pos, OverlayView.BLUE_HEART);

            m_spriteBatch.End();
        }

        //Hanterar och fördelar uppspelning av ljud
        internal void SoundHandler(Model.GameModel a_gameModel)
        {
            int activeMap = a_gameModel.m_activeMapNr;

            //Spel avklarat
            if (a_gameModel.GameCompleted())
            {
                if (!m_soundsSystem.IsPlayingSong(SoundSystem.END))
                     m_soundsSystem.PlaySoundTrack(SoundSystem.END);
            }
            //GameOver
            else if (a_gameModel.GameOver())
            {
                if (!m_soundsSystem.IsPlayingSong(SoundSystem.GAME_OVER))
                     m_soundsSystem.PlaySoundTrack(SoundSystem.GAME_OVER);
            }
            //Bossencounter
            else if (a_gameModel.BossEncountered() && a_gameModel.m_enemySystem.m_bossA.isAlive)
            {
                if (!m_soundsSystem.IsPlayingSong(SoundSystem.BOSS))   
                     m_soundsSystem.PlaySoundTrack(SoundSystem.BOSS);
            }
            //Boss avklarad
            else if (a_gameModel.BossEncountered() && !a_gameModel.m_enemySystem.m_bossA.isAlive)
            {
                m_watch.Start();

                //Spelar seger sång efter 2s
                if (m_watch.ElapsedMilliseconds > 2000 && !m_soundsSystem.IsPlayingSong(SoundSystem.VICTORY))
                {
                    m_soundsSystem.PlaySoundTrack(SoundSystem.VICTORY);
                    m_watch.Stop();
                    m_watch.Reset();
                }
                //Stoppar föregående sång
                else if (!m_soundsSystem.IsPlayingSong(SoundSystem.VICTORY))
                    m_soundsSystem.StopSong();               
            }
            //Spelar sång tillhörandes aktiv bana
            else if (a_gameModel.GameIsRunning())
            {
                if(!m_soundsSystem.IsPlayingSong(a_gameModel.m_activeMapNr))
                    m_soundsSystem.PlaySoundTrack(a_gameModel.m_activeMapNr);
            }
            //Intro theme
            else if (!m_soundsSystem.IsPlayingSong(SoundSystem.THEME))
            {
                m_soundsSystem.PlaySoundTrack(SoundSystem.THEME);
            }

            //Spelare blir träffad
            if(a_gameModel.m_player.m_gotHit && !m_previousPlayerHitState)
            {
                m_soundsSystem.PlaySound(SoundSystem.PLAYER_GOT_HIT);
            }

            m_previousPlayerHitState = a_gameModel.m_player.m_gotHit;

            //Boss blir träffad
            if (a_gameModel.m_enemySystem.m_bossA !=null)
            {
                if (a_gameModel.m_enemySystem.m_bossA.m_gotHit && !m_previousBossHitState)
                {
                    m_soundsSystem.PlaySound(SoundSystem.BOSS_GOT_HIT);
                }

                m_previousBossHitState = a_gameModel.m_enemySystem.m_bossA.m_gotHit;
            }
        }

        //Metod för hantering av kameraregler
        internal void SetCameraRules(Vector2 a_playerCoords, Model.GameModel a_gameModel)
        {
            //Om mappen tar slut åt vänster..
            if (a_playerCoords.X < m_viewPort.Width / 2)
            {
                //Stoppa kameran
                m_cam.Pos = new Vector2(m_viewPort.Width / 2, m_viewPort.Height / 2);
            }
            //Annars om mappen tar slut åt höger    
            else if (a_playerCoords.X > (a_gameModel.m_tileGenerator.GetWidth * 32) - (m_viewPort.Width / 2))
            {   
                //Stoppa kameran
                m_cam.Pos = new Vector2((a_gameModel.m_tileGenerator.GetWidth * 32) - (m_viewPort.Width / 2), 360);
            }
            else
            //Annars följ spelarens x-position
            {
                m_cam.Pos = new Vector2(a_playerCoords.X, 360);
            }
        }

        //Metod för utritning av fiender
        //TODO: Egen klass
        internal void DrawEnemies(Model.GameModel a_gameModel, float a_elapsedTime)
        {
            int enemyAanimation;
            int bossAanimation;

            //Bestämmer animation & ritar ut samtliga fiender på aktiv bana
            foreach (Model.Enemy enemy in a_gameModel.m_enemySystem.m_objectArray.Get())
            {
                Vector2 enemyACoords = m_camera.VisualizeCord(enemy.EnemyPositionX, enemy.EnemyPositionY);

                if (enemy.isMovingLeft)
                {
                    enemyAanimation = TextureAnimationSystem.MOVE_LEFT;
                }
                else if (enemy.isMovingRight)
                {
                    enemyAanimation = TextureAnimationSystem.MOVE_RIGHT;
                }
                else
                {
                    enemyAanimation = TextureAnimationSystem.DEAD_ENEMY;
                    
                    //Spelar upp ljud när fienden dör
                    if(enemy.previousState && !enemy.isAlive)
                    {
                        if (enemy.enemyType == 1)
                        {
                            m_soundsSystem.PlaySound(SoundSystem.ENEMY_ONE_DIES);
                        }
                        else if (enemy.enemyType == 2)
                        {
                            m_soundsSystem.PlaySound(SoundSystem.ENEMY_TWO_DIES);
                        }
                        else
                        {
                            m_soundsSystem.PlaySound(SoundSystem.ENEMY_THREE_DIES);
                        }
                    }
                }

                //Uppdaterar och ritar fiender beroende på typ (1,2 & 3)
                if (enemy.enemyType == 1)
                {
                    m_enemyOneTextureSystem.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera, Color.White, enemyACoords, enemyAanimation);    
                }
                else if (enemy.enemyType == 2)
                {
                    m_enemyTwoTextureSystem.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera, Color.White, enemyACoords, enemyAanimation);
                }
                else
                {
                    m_enemyThreeTextureSystem.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera, Color.White, enemyACoords, enemyAanimation);
                }
                    
                enemy.previousState = enemy.isAlive;
            }

            //Om bosssen har instansierats
            if (a_gameModel.m_enemySystem.m_bossA != null)
            {
                //Visualiserar bossens koordinater
                Vector2 bossACoords = m_camera.VisualizeCord(a_gameModel.m_enemySystem.m_bossA.BossAPositionX, a_gameModel.m_enemySystem.m_bossA.BossAPositionY);

                //Ritar bossens liv
                if (a_gameModel.BossEncountered() && a_gameModel.m_enemySystem.m_bossA.isAlive)
                {
                    m_overlay.DrawLife(m_spriteBatch, a_gameModel.m_enemySystem.m_bossA.m_life.ToString(), m_cam.Pos + new Vector2(1190, 0), OverlayView.BLACK_HEART);
                }
                //Bossen blir träffad
                if (a_gameModel.m_enemySystem.m_bossA.m_gotHit && a_gameModel.m_enemySystem.m_bossA.isAlive)
                {
                    bossAanimation = TextureAnimationSystem.FACE_CAMERA;
                }
                //Bossen går åt vänster
                else if (a_gameModel.m_enemySystem.m_bossA.isMovingLeft)
                {
                    bossAanimation = +TextureAnimationSystem.MOVE_LEFT;
                }
                //Bossen går åt höger
                else if (a_gameModel.m_enemySystem.m_bossA.isMovingRight)
                {
                    bossAanimation = TextureAnimationSystem.MOVE_RIGHT;
                }
                //Bossen är död
                else
                {
                    bossAanimation = TextureAnimationSystem.FACE_CAMERA;

                    //Instansierar och laddar partikelsystem för explosion
                    if (m_explosion == null && !a_gameModel.m_enemySystem.m_bossA.isAlive)
                    {
                        m_explosionWatch.Start();

                        //Väntar 1.5s innan explosion
                        if (m_explosionWatch.ElapsedMilliseconds > 1500)
                        {
                            m_explosion = new ExplosionSystem(a_gameModel.m_enemySystem.m_bossA.BossAPosition, 1.3f);
                            m_explosion.LoadContent(m_content);
                            m_soundsSystem.PlaySound(SoundSystem.EXPLOSION);
                            m_soundsSystem.PlaySound(SoundSystem.METAL_CRASH);

                            m_explosionWatch.Stop();
                            m_explosionWatch.Reset();
                        }
                    }
                    //Uppdaterar explosion
                    if (m_explosion != null)
                    {
                        m_explosion.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera);

                        //(Buggfix)Nollställer explosion samt klocka för uppspelning av segersång efter 15 sekunder
                        m_explosionWatch.Start();
                        if (m_explosionWatch.ElapsedMilliseconds > 15000)
                        {
                            m_explosion = null;
                            m_explosionWatch.Stop();
                            m_explosionWatch.Reset();
                            m_watch.Stop();
                            m_watch.Reset();
                        }

                    }
                }
                //Uppdaterar & ritar bossanimation
                if (m_explosion == null)
                {
                    m_bossATextureSystem.UpdateAndDraw(a_elapsedTime, m_spriteBatch, m_camera, Color.White, bossACoords, bossAanimation);
                }

                a_gameModel.m_enemySystem.m_bossA.previousState = a_gameModel.m_enemySystem.m_bossA.isAlive;
            }

        }

        //Hantering av anv-interaktion via tangentbordet
        internal bool DidPressKey(string a_key)
        {
            KeyboardState kbs = Keyboard.GetState();

            if (kbs.IsKeyDown(Keys.A) && a_key == "A")
            {
                return true;
            }
            if (kbs.IsKeyDown(Keys.S) && a_key == "S")
            {
                return true;
            }
            if (kbs.IsKeyDown(Keys.D) && a_key == "D")
            {
                return true;
            }
            if (kbs.IsKeyDown(Keys.W) && a_key == "W")
            {
                return true;
            }

            return false;

        }

        internal bool DidPressJump()
        {
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                m_didClick = true;
                return false;
            }

            if (m_didClick == true && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                m_didClick = false;
                m_soundsSystem.PlaySound(SoundSystem.PLAYER_JUMP);
                return true;
            }

            return false;
        }
    }
}
