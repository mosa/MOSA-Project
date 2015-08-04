// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.LinkerFormat.PE
{
	/// <summary>
	/// Holds CLI image flags.
	/// </summary>
	[Flags]
	public enum RuntimeImageFlags : uint
	{
		/// <summary>
		/// Always 1.
		/// </summary>
		ILOnly = 0x0000001,

		/// <summary>
		/// Indicates that the image can only be loaded in 32-bit systems.
		/// </summary>
		F32BitsRequired = 0x0000002,

		/// <summary>
		/// Image is signed with a strong name.
		/// </summary>
		StrongNameSigned = 0x0000008,

		/// <summary>
		/// Must never be set.
		/// </summary>
		TrackDebugData = 0x00010000
	}
}
