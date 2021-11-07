// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public interface IAudioDevice
	{
		/// <summary>
		/// Plays a sound from a constrained pointer.
		/// </summary>
		/// <param name="Data"></param>
		void Play(ConstrainedPointer Data);

		/// <summary>
		/// Sets the output sound volume.
		/// </summary>
		/// <param name="v"></param>
		void SetVolume(byte v);
	}
}
