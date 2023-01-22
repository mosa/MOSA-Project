// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::AllocateStackSpace")]
		private static void AllocateStackSpace(Context context, MethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand size = context.Operand1;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.ESP);

			context.SetInstruction(X86.Sub32, esp, esp, size);
			context.AppendInstruction(X86.Mov32, result, esp);
		}
	}
}
