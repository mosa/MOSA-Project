// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public interface IAudioDevice
{
	/// <summary>
	/// The device name.
	/// </summary>
	string Name { get; }

	// Format is WAV (unsigned PCM)
	byte[] TestSound { get; }

	/// <summary>
	/// Plays a sound from a constrained pointer.
	/// </summary>
	/// <param name="data"></param>
	void Play(ConstrainedPointer data);

	/// <summary>
	/// Sets the output sound volume.
	/// </summary>
	/// <param name="v"></param>
	void SetVolume(byte v);
}
