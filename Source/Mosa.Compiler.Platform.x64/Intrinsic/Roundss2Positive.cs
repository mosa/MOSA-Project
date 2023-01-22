// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::Roundss2Positive")]
		private static void Roundss2Positive(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.Roundss, context.Result, context.Operand1, methodCompiler.CreateConstant((byte)2));
		}
	}
}
