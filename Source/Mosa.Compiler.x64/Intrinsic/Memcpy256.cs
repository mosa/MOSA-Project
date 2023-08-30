// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Memcpy256")]
	private static void Memcpy256(Context context, TransformContext transformContext)
	{
		var dest = context.Operand1;
		var src = context.Operand2;

		var v0 = transformContext.VirtualRegisters.Allocate64();
		var v1 = transformContext.VirtualRegisters.Allocate64();
		var offset16 = Operand.Constant64_16;

		context.SetInstruction(X64.MovupsLoad, v0, dest, Operand.Constant64_0);
		context.AppendInstruction(X64.MovupsLoad, v1, dest, offset16);
		context.AppendInstruction(X64.MovupsStore, null, dest, Operand.Constant64_0, v0);
		context.AppendInstruction(X64.MovupsStore, null, dest, offset16, v1);
	}
}
