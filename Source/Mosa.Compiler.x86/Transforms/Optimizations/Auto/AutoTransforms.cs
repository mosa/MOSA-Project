// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto;

/// <summary>
/// AutoTransforms
/// </summary>
public static class AutoTransforms
{
	public static readonly List<BaseTransform> List = new() {
		new Simplification.Mov32Coalescing(),
		new StrengthReduction.Add32ByZero(),
		new StrengthReduction.Sub32ByZero(),
		new StrengthReduction.IMul32ByZero(),
		new StrengthReduction.IMul32ByOne(),
		new StrengthReduction.Add32By1Not32(),
		new StrengthReduction.Add32By1Not32_v1(),
		new StrengthReduction.Inc32Not32(),
		new Specific.And32Add32ToBlsr32(),
		new Specific.And32Add32ToBlsr32_v1(),
		new Specific.And32Add32ToBlsr32_v2(),
		new Specific.And32Add32ToBlsr32_v3(),
		new Simplication.SubFromZero(),
		new StrengthReduction.And32ByZero(),
		new StrengthReduction.And32ByZero_v1(),
		new StrengthReduction.And32ByMax(),
		new StrengthReduction.And32ByMax_v1(),
		new StrengthReduction.Sar32ZeroValue(),
		new StrengthReduction.Sar32ByZero(),
		new StrengthReduction.Shl32ZeroValue(),
		new StrengthReduction.Shl32ByZero(),
		new StrengthReduction.Shr32ZeroValue(),
		new StrengthReduction.Shr32ByZero(),
		new Consolidation.IMul32Mov32ByZero(),
		new Consolidation.IMul32Mov32ByOne(),
		new ConstantMove.Add32(),
		new ConstantMove.Sub32(),
		new ConstantMove.And32(),
		new ConstantMove.Or32(),
		new ConstantMove.Xor32(),
		new ConstantMove.Sar32(),
		new ConstantMove.Shl32(),
		new ConstantMove.Shr32(),
		new Ordering.Adc32(),
		new Ordering.Add32(),
		new Ordering.Addss(),
		new Ordering.Addsd(),
		new Ordering.And32(),
		new Ordering.IMul32(),
		new Ordering.Mulss(),
		new Ordering.Mulsd(),
		new Ordering.Or32(),
		new Ordering.Xor32(),
	};
}
