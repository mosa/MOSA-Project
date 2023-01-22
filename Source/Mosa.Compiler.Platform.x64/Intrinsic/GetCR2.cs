// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::GetCR2")]
		private static void GetCR2(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.MovCRLoad64, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U8, CPURegister.CR2));
		}
	}
}
