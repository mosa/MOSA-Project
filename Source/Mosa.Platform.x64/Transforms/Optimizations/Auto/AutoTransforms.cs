// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Auto
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class AutoTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform> {
			 new Standard.Mov32Coalescing(),
			 new Standard.Mov64Coalescing(),
			 new StrengthReduction.Add32ByZero(),
			 new StrengthReduction.Add64ByZero(),
			 new StrengthReduction.Sub32ByZero(),
			 new StrengthReduction.Sub64ByZero(),
			 new Shrink.And32ByFF(),
			 new Shrink.And32ByFF_v1(),
			 new Shrink.And64ByFF(),
			 new Shrink.And64ByFF_v1(),
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
		};
	}
}