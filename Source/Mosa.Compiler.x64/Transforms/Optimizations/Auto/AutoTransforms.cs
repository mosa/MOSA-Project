// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto;

/// <summary>
/// AutoTransforms
/// </summary>
public static class AutoTransforms
{
	public static readonly List<BaseTransform> List = new() {
		new Simplification.Mov32Coalescing(),
		new Simplification.Mov64Coalescing(),
		new StrengthReduction.Add32ByZero(),
		new StrengthReduction.Add64ByZero(),
		new StrengthReduction.Sub32ByZero(),
		new StrengthReduction.Sub64ByZero(),
		new StrengthReduction.Add32By1Not32(),
		new StrengthReduction.Add32By1Not32_v1(),
		new StrengthReduction.Add64By1Not64(),
		new StrengthReduction.Add64By1Not64_v1(),
		new StrengthReduction.Inc32Not32(),
		new StrengthReduction.Inc64Not64(),
		new Specific.And32Add32ToBlsr32(),
		new Specific.And32Add32ToBlsr32_v1(),
		new Specific.And32Add32ToBlsr32_v2(),
		new Specific.And32Add32ToBlsr32_v3(),
		new Specific.And64Add64ToBlsr64(),
		new Specific.And64Add64ToBlsr64_v1(),
		new Specific.And64Add64ToBlsr64_v2(),
		new Specific.And64Add64ToBlsr64_v3(),
		new Simplication.Sub32FromZero(),
		new Simplication.Sub64FromZero(),
		new Ordering.Adc32(),
		new Ordering.Adc64(),
		new Ordering.Add32(),
		new Ordering.Add64(),
		new Ordering.Addss(),
		new Ordering.Addsd(),
		new Ordering.And32(),
		new Ordering.And64(),
		new Ordering.IMul32(),
		new Ordering.IMul64(),
		new Ordering.Mulss(),
		new Ordering.Mulsd(),
		new Ordering.Or32(),
		new Ordering.Or64(),
		new Ordering.Xor32(),
		new Ordering.Xor64(),
	};
}
