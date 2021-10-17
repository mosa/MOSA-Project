// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Platform Architecture
	/// </summary>
	[System.Flags]
	public enum PlatformArchitecture
	{
		/// <summary>
		/// The none
		/// </summary>
		None = 0,

		/// <summary>
		/// The X86
		/// </summary>
		X86 = 1,

		/// <summary>
		/// The X64
		/// </summary>
		X64 = 2,

		/// <summary>
		/// The ARMv8 32bit
		/// </summary>
		ARMv8A32 = 3,

		/// <summary>
		/// The X86 and X64
		/// </summary>
		X86AndX64 = PlatformArchitecture.X86 | PlatformArchitecture.X64,
	}
}
