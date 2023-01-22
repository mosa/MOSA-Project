// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::Invlpg")]
		private static void Invlpg(Context context, MethodCompiler methodCompiler)
		{
			//Debug.Assert(context.Operand1.IsConstant);
			context.SetInstruction(X86.Invlpg, null, context.Operand1);
		}
	}
}
