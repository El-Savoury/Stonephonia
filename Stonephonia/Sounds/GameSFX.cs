namespace Stonephonia.Sounds
{
	abstract class GameSFX
	{
		/// <summary>
		/// Play the sound effect
		/// </summary>
		public abstract void Play(float volume, float pitch, float pan);

		/// <summary>
		/// Stop all instances of this sound effect.
		/// </summary>
		public abstract void StopAll();
	}
}
