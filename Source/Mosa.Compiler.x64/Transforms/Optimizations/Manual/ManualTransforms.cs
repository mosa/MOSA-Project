// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual;

/// <summary>
/// Manual Optimizations Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new Special.Deadcode(),

		new Rewrite.Add32ToInc32(),
		new Rewrite.Add32ToLea32(),
		new Rewrite.Add64ToLea64(),
		new Rewrite.Mov32ToXor32(),
		new Rewrite.Mov64ToXor64(),
		new Rewrite.Sub32ToDec32(),
		new Rewrite.Sub64ToLea64(),
		new Rewrite.Sub64ToLea64(),
		new Rewrite.Lea32ToInc32(),
		new Rewrite.Lea32ToDec32(),
		new Rewrite.Lea32ToMov32(),
		new Rewrite.Lea64ToMov64(),
		new Rewrite.Cmp32ToZero(),
		new Rewrite.Cmp32ToTest32(),
		new Rewrite.Test32ToZero(),

		new Special.Mov32ConstantReuse(),

		new Stack.Add32(),
		new Stack.Add64(),

		//new Special.Mov32ReuseConstant(), /// this can wait
		//new Special.Mov32Propagate(),
	};
}
