// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Get8")]
		private static void Get8(Context context, MethodCompiler methodCompiler)
		{
			Debug.Assert(context.Result.IsI4 || context.Result.IsU4);
			context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, methodCompiler.ConstantZero32);
		}
	}
}
