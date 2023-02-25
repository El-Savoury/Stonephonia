using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Stonephonia.Sounds;


namespace Stonephonia
{
    public static class SoundManager
    {
        public enum MusicType
        {
            jungleLoop
        }

        public enum SFXType
        {
            mainTheme,
            bell,
            marimba,
            wobble,
            wood
        }

        private static Dictionary<MusicType, Song> mSongs;
        private static Dictionary<SFXType, GameSFX> mSFX;

        public static void LoadContent(ContentManager content)
        {
            mSongs = new Dictionary<MusicType, Song> { };
            mSongs.Add(MusicType.jungleLoop, content.Load<Song>("Sounds/jungle_loop"));

            MediaPlayer.IsRepeating = true;

            mSFX = new Dictionary<SFXType, GameSFX>();

            mSFX.Add(SFXType.mainTheme, new GameSongSFX(content, "Sounds/theme_space"));

            mSFX.Add(SFXType.bell, new GameRandomSFX(content, ("Sounds/SFX/bell0", 0.6f)
                                                            , ("Sounds/SFX/bell1", 0.6f)));

            mSFX.Add(SFXType.marimba, new GameRandomSFX(content, ("Sounds/SFX/marimba0", 1.0f)
                                                               , ("Sounds/SFX/marimba1", 1.0f)
                                                               , ("Sounds/SFX/marimba2", 1.0f)
                                                               , ("Sounds/SFX/marimba3", 1.0f)
                                                               , ("Sounds/SFX/marimba4", 1.0f)));

            mSFX.Add(SFXType.wobble, new GameRandomSFX(content, ("Sounds/SFX/wobble0", 1.0f)
                                                              , ("Sounds/SFX/wobble1", 1.0f)));

            mSFX.Add(SFXType.wood, new GameRandomSFX(content, ("Sounds/SFX/wood0", 1.0f)
                                                            , ("Sounds/SFX/wood1", 1.0f)
                                                            , ("Sounds/SFX/wood2", 1.0f)));
        }

        public static void PlayMusic(MusicType musicType, float volume)
        {
            if (mSongs.ContainsKey(musicType))
            {
                MediaPlayer.Play(mSongs[musicType]);
                MediaPlayer.Volume = volume;
            }
        }
        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        public static void PlaySFX(SFXType sfx, float volume)
        {
            if (mSFX.ContainsKey(sfx))
            {
                mSFX[sfx].Play(volume, 0.0f, 0.0f);
            }
        }

        public static void StopSFX(SFXType sfx)
        {
            if (mSFX.ContainsKey(sfx))
            {
                mSFX[sfx].StopAll();
            }
        }
    }
}
