using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Stonephonia.Sounds
{
	/// <summary>
	/// Sound effect that plays from a pool randomly.
	/// Avoids repeats if possible.
	/// </summary>
	class GameRandomSFX : GameSFX
	{
		Random mRandom;
		GameSingleSFX[] mSFXPool;
		int mPrevSelection;

		public GameRandomSFX(ContentManager content, params (string, float)[] soundData)
		{
			if(soundData.Length <= 1)
			{
				throw new Exception("Random sound effect with only 1 sound should be a GameSingleSFX.");
			}

			mPrevSelection = -1;
			mRandom = new Random();

			// Load sound effects
			mSFXPool = new GameSingleSFX[soundData.Length];
			for (int i = 0; i < soundData.Length; i++)
			{
				mSFXPool[i] = new GameSingleSFX(content, soundData[i].Item1, soundData[i].Item2);
			}
		}

		public override void Play(float volume, float pitch, float pan)
		{
			int selectedSFX = mRandom.Next(0, mSFXPool.Length);

			if(selectedSFX == mPrevSelection)
			{
				// Move to the next one on the list
				selectedSFX = (selectedSFX + 1) % mSFXPool.Length;
			}

			mSFXPool[selectedSFX].Play(volume, pitch, pan);
			mPrevSelection = selectedSFX; // Save this so we don't repeat
		}

		public override void StopAll()
		{
			foreach(GameSingleSFX gameSingleSFX in mSFXPool)
			{
				gameSingleSFX.StopAll();
			}
		}
	}
}
