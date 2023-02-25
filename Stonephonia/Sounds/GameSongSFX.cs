using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Stonephonia.Sounds
{
	/// <summary>
	/// A class for sound effects that are actually songs.
	/// We want to make sure only one instance is playing.
	/// </summary>
	class GameSongSFX : GameSFX
	{
		SoundEffectInstance mSoundEffectInstance; // Only one instance!
		SoundEffect mSFX;

		public GameSongSFX(ContentManager content, string sfxPath)
		{
			mSFX = content.Load<SoundEffect>(sfxPath);
			mSoundEffectInstance = null;
		}

		public override void Play(float volume, float pitch, float pan)
		{
			if(mSoundEffectInstance != null)
			{
				StopAll();
			}

			mSoundEffectInstance = mSFX.CreateInstance();
			mSoundEffectInstance.Volume = volume;
			mSoundEffectInstance.Pitch = pitch;
			mSoundEffectInstance.Pan = pan;
			mSoundEffectInstance.Play();
		}

		public override void StopAll()
		{
			mSoundEffectInstance.Stop(true);
			mSoundEffectInstance.Dispose();
		}
	}
}
