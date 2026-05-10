// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode;

/// <summary>
/// Address Mode Transformation List
/// </summary>
public static class AddressModeTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Adc32.Instance,
		Add32.Instance,
		Addsd.Instance,
		Addss.Instance,
		And32.Instance,
		Btr32.Instance,
		Bts32.Instance,
		Dec32.Instance,
		Divsd.Instance,
		Divss.Instance,
		IMul32.Instance,
		Mulsd.Instance,
		Mulss.Instance,
		Neg32.Instance,
		Not32.Instance,
		Or32.Instance,
		Rcr32.Instance,
		Roundsd.Instance,
		Roundss.Instance,
		Sar32.Instance,
		Sbb32.Instance,
		Shl32.Instance,
		Shld32.Instance,
		Shr32.Instance,
		Shrd32.Instance,
		Sub32.Instance,
		Subsd.Instance,
		Subss.Instance,
		Xor32.Instance,
		Xorps.Instance,
	};
}
