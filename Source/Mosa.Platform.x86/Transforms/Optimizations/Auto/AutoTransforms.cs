// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Optimizations.Auto;

/// <summary>
/// Transformations
/// </summary>
public static class AutoTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform> {
		new Standard.Mov32Coalescing(),
		new StrengthReduction.Add32ByZero(),
		new StrengthReduction.Sub32ByZero(),
		new StrengthReduction.IMul32ByZero(),
		new StrengthReduction.IMul32ByOne(),
		new Consolidating.Add32Mov32ByZero(),
		new Consolidating.IMul32Mov32ByZero(),
		new Consolidating.IMul32Mov32ByOne(),
		new Shrink.And32ByFF(),
		new Shrink.And32ByFF_v1(),
		new StrengthReduction.Add32By1Not32(),
		new StrengthReduction.Add32By1Not32_v1(),
		new StrengthReduction.Inc32Not32(),
		new Specific.And32Add32ToBlsr32(),
		new Specific.And32Add32ToBlsr32_v1(),
		new Specific.And32Add32ToBlsr32_v2(),
		new Specific.And32Add32ToBlsr32_v3(),
		new Simplication.SubFromZero(),
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
