// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Get32
	/// </summary>
	internal sealed class Get32 : IIntrinsicPlatformMethod
	{
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			Debug.Assert(context.Result.IsI4 | context.Result.IsU4);
			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, methodCompiler.ConstantZero);
		}
	}
}
