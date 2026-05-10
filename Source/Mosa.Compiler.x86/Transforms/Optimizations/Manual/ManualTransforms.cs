// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual;

/// <summary>
/// Manual Optimizations Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Rewrite.Add32ToInc32.Instance,
		Rewrite.Add32ToLea32.Instance,
		Rewrite.Cmp32ToTest32.Instance,
		Rewrite.Cmp32ToZero.Instance,
		Rewrite.Lea32ToInc32.Instance,
		Rewrite.Lea32ToDec32.Instance,
		//Rewrite.Mov32ToXor32.Instance,
		Rewrite.Sub32ToDec32.Instance,
		Rewrite.Sub32ToLea32.Instance,
		Rewrite.Test32ToZero.Instance,

		Special.Deadcode.Instance,
		Special.Mov32Coalescing.Instance,
		Special.Mov32ConstantReuse.Instance,
		Special.Mov32Unless.Instance,
		Special.Mul32Ditto.Instance,
		Special.Bt32Movzx8To32Setcc.Instance,

		StrengthReduction.Mul32ByZero.Instance,
		StrengthReduction.Mul32WithMov32ByZero.Instance,

		Stack.Add32.Instance,

		Size.Add32By2ToInc32.Instance,
		Size.Lea32By2.Instance,
	};
}
