// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::GetCR4")]
		private static void GetCR4(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovCRLoad32, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, ControlRegister.CR4));
		}
	}
}
