using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace View
{
    //Klass för uppspelning av samtliga ljud
    class SoundSystem
    {
        private SoundEffect[] m_soundEffects;
        private Song[] m_soundTracks;
        private int m_activeSong;

        //Publika konstanter för ljudeffekter
        public const int ENEMY_ONE_DIES = 0;
        public const int ENEMY_TWO_DIES = 1;
        public const int ENEMY_THREE_DIES = 2;
        public const int PLAYER_JUMP = 3;
        public const int PLAYER_GOT_HIT = 4;
        public const int BOSS_GOT_HIT = 5;
        public const int EXPLOSION = 6;
        public const int METAL_CRASH = 7;

        //Publika konstanter för soundtracks
        public const int BOSS = 3;
        public const int GAME_OVER = 4;
        public const int THEME = 5;
        public const int VICTORY = 6;
        public const int END = 7;

        //Laddar in samtliga ljudeffekter & soundtracks
        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager a_content)
        {
            m_soundEffects = new SoundEffect[8] {a_content.Load<SoundEffect>("Sounds/Effects/beep1"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/crow"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/beep1"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/jump"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/looseLife"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/bossHit2"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/bossExplosion"),
                                                 a_content.Load<SoundEffect>("Sounds/Effects/metalCrash")};
            m_soundTracks = new Song[8] {a_content.Load<Song>("Sounds/Tracks/map1song"), 
                                         a_content.Load<Song>("Sounds/Tracks/map2song"),
                                         a_content.Load<Song>("Sounds/Tracks/map3song"),
                                         a_content.Load<Song>("Sounds/Tracks/Boss"),
                                         a_content.Load<Song>("Sounds/Tracks/Defeat"),
                                         a_content.Load<Song>("Sounds/Tracks/Theme"),
                                         a_content.Load<Song>("Sounds/Tracks/Victory"),
                                         a_content.Load<Song>("Sounds/Tracks/End")};
        }

        //Metod för uppspelning av ljudeffekter
        internal void PlaySound(int a_sound)
        {   
            switch (a_sound)
            {
                case ENEMY_ONE_DIES:
                    m_soundEffects[ENEMY_ONE_DIES].Play(0.3f, 0.0f, 0.0f);
                    break;
                case ENEMY_TWO_DIES:
                    m_soundEffects[ENEMY_TWO_DIES].Play(0.4f, 0.0f, 0.0f);
                    break;
                case ENEMY_THREE_DIES:
                    m_soundEffects[ENEMY_THREE_DIES].Play(0.3f, 0.0f, 0.0f);
                    break;
                case PLAYER_JUMP:
                    m_soundEffects[PLAYER_JUMP].Play(0.3f, 0.0f, 0.0f);
                    break;
                case PLAYER_GOT_HIT:
                    m_soundEffects[PLAYER_GOT_HIT].Play(0.5f, 0.0f, 0.0f);
                    break;
                case BOSS_GOT_HIT:
                    m_soundEffects[BOSS_GOT_HIT].Play(0.2f, 0.0f, 0.0f);
                    break;
                case EXPLOSION:
                    m_soundEffects[EXPLOSION].Play(0.25f,0.0f,0.0f);
                    break;
                case METAL_CRASH:
                    m_soundEffects[METAL_CRASH].Play(0.075f, 0.0f, 0.0f);
                    break;
            }

        }

        //Retunerar true om angiven song spelas
        public bool IsPlayingSong(int a_songId)
        {
            return MediaPlayer.State == MediaState.Playing && m_activeSong == a_songId;
        }

        internal void StopSong()
        {
            MediaPlayer.Stop();
        }

        //Metod för uppspelning av angiven song
        internal void PlaySoundTrack(int a_songId)
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.Play(m_soundTracks[a_songId]);
            m_activeSong = a_songId;
        }
    }
}
