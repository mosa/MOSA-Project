// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual;

/// <summary>
/// Manual Optimizations Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new Standard.Add32ToInc32(),
		new Standard.Add32ToLea32(), //
		new Standard.Cmp32ToTest32(),
		new Standard.Cmp32ToZero(),
		new Standard.Lea32ToInc32(),
		new Standard.Sub32ToDec32(),
		new Standard.Lea32ToDec32(),
		new Standard.Sub32ToLea32(),
		new Standard.Test32ToZero(),

		new Special.Deadcode(),
		new Special.Mov32Coalescing(),
		new Special.Mov32ConstantReuse(),
		new Special.Mov32Unless(),
		new Special.Mul32Ditto(),
		new Special.Bt32Movzx8To32Setcc(),

		new StrengthReduction.Mul32ByZero(),
		new StrengthReduction.Mul32WithMov32ByZero(),

		new Stack.Add32(),
	};
}
