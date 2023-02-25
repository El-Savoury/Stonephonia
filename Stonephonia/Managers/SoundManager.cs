using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace Stonephonia
{
    public static class SoundManager
    {
        public enum MusicType
        {
            AmbientTrack
        }

        public enum SFXType
        {
            MainTheme,
            bell,
            flute,
            pad,
            rhodes
        }

        private static Dictionary<MusicType, Song> mSongs;
        private static Dictionary<SFXType, SoundEffect> mSFX;

        public static void LoadContent(ContentManager content)
        {
            mSongs = new Dictionary<MusicType, Song> { };
            mSongs.Add(MusicType.AmbientTrack, content.Load<Song>("Sounds/theme_space"));

            MediaPlayer.IsRepeating = true;

            mSFX = new Dictionary<SFXType, SoundEffect> { };
            mSFX.Add(SFXType.bell, content.Load<SoundEffect>("Sounds/SFX/bell0"));
            mSFX.Add(SFXType.flute, content.Load<SoundEffect>("Sounds/SFX/marimba0"));
            mSFX.Add(SFXType.pad, content.Load<SoundEffect>("Sounds/SFX/wood0"));
            mSFX.Add(SFXType.rhodes, content.Load<SoundEffect>("Sounds/SFX/wobble0"));
        }

        public static void PlayMusic(MusicType musicType, float volume)
        {
            if (mSongs.ContainsKey(musicType))
            {
                MediaPlayer.Play(mSongs[musicType]);
                MediaPlayer.Volume = volume;
            }
        }

        public static void PlaySFX(SFXType sfx, float volume)
        {
            if (mSFX.ContainsKey(sfx))
            {
                mSFX[sfx].Play(volume, 0.0f, 0.0f);
            }
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
