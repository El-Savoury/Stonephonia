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
            bell,
            flute,
            pad,
            rhodes,
            bass,
            plinks,
            square,
            vamp,
            ageBass,
            agePlinks,
            ageSquare,
            ageVamp
        }

        public static Dictionary<MusicType, Song> mSongs;
        public static Dictionary<SFXType, SoundEffect> mSFX;

        public static void LoadContent(ContentManager content)
        {
            mSongs = new Dictionary<MusicType, Song> { };
            mSongs.Add(MusicType.AmbientTrack, content.Load<Song>("Sounds/Minimalism"));

            MediaPlayer.IsRepeating = true;

            mSFX = new Dictionary<SFXType, SoundEffect> { };
            mSFX.Add(SFXType.bell, content.Load<SoundEffect>("Sounds/Bells_0"));
            mSFX.Add(SFXType.flute, content.Load<SoundEffect>("Sounds/Flute_0"));
            mSFX.Add(SFXType.pad, content.Load<SoundEffect>("Sounds/Pad_0"));
            mSFX.Add(SFXType.rhodes, content.Load<SoundEffect>("Sounds/Rhodes_0"));
            mSFX.Add(SFXType.bass, content.Load<SoundEffect>("Sounds/rocksounds/musicBass"));
            mSFX.Add(SFXType.plinks, content.Load<SoundEffect>("Sounds/rocksounds/musicPlinks"));
            mSFX.Add(SFXType.square, content.Load<SoundEffect>("Sounds/rocksounds/musicSquare"));
            mSFX.Add(SFXType.vamp, content.Load<SoundEffect>("Sounds/rocksounds/musicVamp"));
            mSFX.Add(SFXType.ageBass, content.Load<SoundEffect>("Sounds/agesounds/ageBass"));
            mSFX.Add(SFXType.agePlinks, content.Load<SoundEffect>("Sounds/agesounds/agePlinks"));
            mSFX.Add(SFXType.ageSquare, content.Load<SoundEffect>("Sounds/agesounds/ageSquare"));
            mSFX.Add(SFXType.ageVamp, content.Load<SoundEffect>("Sounds/agesounds/ageVamp"));
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
