// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	[System.Flags]
	public enum PlatformArchitecture
	{
		/// <summary>
		///
		/// </summary>
		None = 0,

		/// <summary>
		///
		/// </summary>
		X86 = 1,

		/// <summary>
		///
		/// </summary>
		X64 = 2,

		/// <summary>
		///
		/// </summary>
		X86AndX64 = PlatformArchitecture.X86 | PlatformArchitecture.X64,
	}
}