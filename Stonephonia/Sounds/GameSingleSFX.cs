using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Stonephonia.Sounds
{
	/// <summary>
	/// Simple sound effects
	/// </summary>
	class GameSingleSFX : GameSFX
	{
		SoundEffect mSFX;
		float mVolumeModifier;

		public GameSingleSFX(ContentManager content, string sfxPath, float volumeMod = 1.0f)
		{
			mSFX = content.Load<SoundEffect>(sfxPath);
			mVolumeModifier = volumeMod;

			if (mVolumeModifier < 0.0f)
			{
				throw new Exception("Volume modifier must be positive.");
			}
		}

		public override void Play(float volume, float pitch, float pan)
		{
			mSFX.Play(volume * mVolumeModifier, pitch, pan);
		}

		public override void StopAll()
		{
			// Can't stop these yet!
			throw new NotImplementedException();
		}
	}
}
