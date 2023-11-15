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
		new Rewrite.Add32ToInc32(),
		new Rewrite.Add32ToLea32(),
		new Rewrite.Cmp32ToTest32(),
		new Rewrite.Cmp32ToZero(),
		new Rewrite.Lea32ToInc32(),
		new Rewrite.Lea32ToDec32(),
		new Rewrite.Sub32ToDec32(),
		new Rewrite.Sub32ToLea32(),
		new Rewrite.Test32ToZero(),

		new Special.Deadcode(),
		new Special.Mov32Coalescing(),
		new Special.Mov32ConstantReuse(),
		new Special.Mov32Unless(),
		new Special.Mul32Ditto(),
		new Special.Bt32Movzx8To32Setcc(),

		new StrengthReduction.Mul32ByZero(),
		new StrengthReduction.Mul32WithMov32ByZero(),

		new Stack.Add32(),

		new Size.Add32By2ToInc32(),
		new Size.Lea32By2(),
	};
}
