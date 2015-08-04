// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Platform.ARMv6
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
		/// Support to ARM instruction set.
		/// </summary>
		ARM = 1,

		/// <summary>
		/// Support to Thumb instruction set.
		/// </summary>
		Thumb = 2,

		/// <summary>
		/// Support to Thumb-2 instruction set.
		/// </summary>
		Thumb2 = 4,

		/// <summary>
		/// Support to ARM DSP instruction set extensions.
		/// Full support in the ARMv5TE, ARMv6 and ARMv7 architectures.
		/// </summary>
		DSP = 8,

		/// <summary>
		/// Support to the ARM SIMD media extensions.
		/// Full support in the ARM1176, ARM11 MPCore, Cortex-A5, Cortex-A8 and Cortex-A9 architectures.
		/// </summary>
		SIMD = 16,

		/// <summary>
		/// Support to Trusted Execution Environment (TEE).
		/// ARM1176, Cortex-A5, Cortex-A7, Cortex-A8, Cortex-A9, Cortex-A12, Cortex-A15, Cortex-A53, Cortex-A57
		/// </summary>
		TrustZone = 32,

		/// <summary>
		/// Support to ARM NEON general-purpose SIMD engine for multimedia and signal processing.
		/// </summary>
		Neon = 64,

		/// <summary>
		/// Support to Jazelle technology hardware extensions.
		/// </summary>
		Jazelle = 128,

		/// <summary>
		/// Support to ARM Floating Point architecture.
		/// VFP1 is obsolete.
		/// </summary>
		VFPv1 = 256,

		/// <summary>
		/// Support to ARM Floating Point architecture.
		/// VFPv2 is an optional extension to the ARM instruction set in the ARMv5TE, ARMv5TEJ and ARMv6 architectures.
		/// </summary>
		VFPv2 = 512,

		/// <summary>
		/// Support to ARM Floating Point architecture.
		/// VFPv3 is an optional extension to the ARM, Thumb and ThumbEE instruction sets in the ARMv7-A and ARMv7-R profiles.
		/// </summary>
		VFPv3 = 1024,
	}
}
