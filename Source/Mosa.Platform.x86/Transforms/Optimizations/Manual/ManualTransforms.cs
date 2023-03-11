// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual;

/// <summary>
/// Manual Optimizations Transformations
/// </summary>
public static class ManualTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new Special.Deadcode(),

		new Standard.Add32ToInc32(),
		new Standard.Sub32ToDec32(),
		new Standard.Lea32ToInc32(),
		new Standard.Lea32ToDec32(),
		new Standard.Cmp32ToZero(),
		new Standard.Test32ToZero(),
		new Standard.Cmp32ToTest32(),

		new Special.Mov32ConstantReuse(),
		new Special.Bt32Movzx8To32Setcc(),

		new Stack.Add32(),

		//new Special.Mov32Propagate(),
		//Add32ToLea32
		//Sub32ToLea32
	};
}
