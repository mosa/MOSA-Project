// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::Memclr256")]
	private static void Memclr256(Context context, MethodCompiler methodCompiler)
	{
		var dest = context.Operand1;

		var v0 = Operand.CreateCPURegisterR8(CPURegister.XMM0);
		var offset16 = methodCompiler.CreateConstant(16);

		context.SetInstruction(X64.PXor, v0, v0, v0);
		context.AppendInstruction(X64.MovupsStore, dest, Operand.Constant64_0, v0);
		context.AppendInstruction(X64.MovupsStore, dest, offset16, v0);
	}
}
