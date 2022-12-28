// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;


namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::GetCS")]
		private static void GetCS(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovLoadSeg32, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.CS));
		}
	}
}
