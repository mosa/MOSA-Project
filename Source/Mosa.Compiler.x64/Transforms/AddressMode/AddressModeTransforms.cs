// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.AddressMode;

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

		Adc64.Instance,
		Add64.Instance,
		And64.Instance,
		Btr64.Instance,
		Bts64.Instance,
		Dec64.Instance,
		IMul64.Instance,
		Mulsd.Instance,
		Mulss.Instance,
		Neg64.Instance,
		Not64.Instance,
		Or64.Instance,
		Rcr64.Instance,
		Sar64.Instance,
		Sbb64.Instance,
		Shl64.Instance,
		Shld64.Instance,
		Shr64.Instance,
		Shrd64.Instance,
		Sub64.Instance,
		Xor64.Instance,
	};
}
