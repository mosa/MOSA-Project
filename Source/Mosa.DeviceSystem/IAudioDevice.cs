// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public interface IAudioDevice
{
	/// <summary>
	/// Plays a sound.
	/// </summary>
	/// <param name="data"></param>
	void Play(byte[] data);

	/// <summary>
	/// Sets the output sound volume.
	/// </summary>
	/// <param name="v"></param>
	void SetVolume(byte v);
}
