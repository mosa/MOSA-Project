// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.Transform;
using Mosa.Platform.x86.Transform.Manual;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Transform.Manual
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class ManualTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			new Special.Deadcode(),
			new Standard.Mov32ToXor32(),
			new Standard.Add32ToInc32(),
			new Standard.Sub32ToDec32(),
			new Standard.Lea32ToInc32(),
			new Standard.Lea32ToDec32(),
			new Standard.Cmp32ToZero(),
			new Standard.Test32ToZero(),
			new Standard.Cmp32ToTest32(),
		};

		public static readonly List<BaseTransformation> PostList = new List<BaseTransformation>
		{
			new Special.Mov32ConstantReuse(),
		};

		public static readonly List<BaseTransformation> EarlyList = new List<BaseTransformation>
		{
			new Stack.Add32(),

			//new Special.Mov32ReuseConstant(), /// this can wait
			//new Special.Mov32Propagate(),
		};
	}
}
