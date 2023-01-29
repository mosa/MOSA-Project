// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public interface ITimer
{
	/// <summary>
	/// Waits for a specific time, in milliseconds.
	/// </summary>
	void Wait(uint ms);
}
