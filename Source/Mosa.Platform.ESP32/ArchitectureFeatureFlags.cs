// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Platform.ESP32
{
	/// <summary>
	/// Determines features provided by an architecture.
	/// </summary>
	[Flags]
	public enum ArchitectureFeatureFlags
	{
		/// <summary>
		/// Auto detect architecture features using the current processor. Not available for cross compilation.
		/// </summary>
		AutoDetect = 0,

		/// <summary>
		/// Support to ESP32 instruction set.
		/// </summary>
		ESP32 = 1,
	}
}
