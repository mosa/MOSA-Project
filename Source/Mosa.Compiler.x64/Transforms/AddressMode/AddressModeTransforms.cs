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
		new Adc32(),
		new Add32(),
		new Addsd(),
		new Addss(),
		new And32(),
		new Btr32(),
		new Bts32(),
		new Dec32(),
		new Divsd(),
		new Divss(),
		new IMul32(),
		new Mulsd(),
		new Mulss(),
		new Neg32(),
		new Not32(),
		new Or32(),
		new Rcr32(),
		new Roundsd(),
		new Roundss(),
		new Sar32(),
		new Sbb32(),
		new Shl32(),
		new Shld32(),
		new Shr32(),
		new Shrd32(),
		new Sub32(),
		new Subsd(),
		new Subss(),
		new Xor32(),

		new Adc64(),
		new Add64(),
		new And64(),
		new Btr64(),
		new Bts64(),
		new Dec64(),
		new IMul64(),
		new Mulsd(),
		new Mulss(),
		new Neg64(),
		new Not64(),
		new Or64(),
		new Rcr64(),
		new Sar64(),
		new Sbb64(),
		new Shl64(),
		new Shld64(),
		new Shr64(),
		new Shrd64(),
		new Sub64(),
		new Xor64(),
	};
}
