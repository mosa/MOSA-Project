// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::In16")]
		private static void In16(Context context, MethodCompiler methodCompiler)
		{
			Operand v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			var result = context.Result;

			context.SetInstruction(X86.In16, v1, context.Operand1);
			context.AppendInstruction(X86.Movzx16To32, result, v1);

			//context.SetInstruction(X86.In16, result, context.Operand1);
		}
	}
}
