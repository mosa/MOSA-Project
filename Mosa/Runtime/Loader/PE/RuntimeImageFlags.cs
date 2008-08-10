/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.Loader.PE {
	[Flags]
	public enum RuntimeImageFlags : uint {
		ILOnly = 0x0000001,
		F32BitsRequired = 0x0000002,
		StrongNameSigned = 0x0000008,
		TrackDebugData = 0x00010000
	}
}