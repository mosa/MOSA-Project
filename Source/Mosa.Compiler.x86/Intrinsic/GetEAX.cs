// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetEAX")]
	private static void GetEAX(Context context, Transform transform)
	{
		var result = context.Result;
		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);

		context.SetInstruction(IR.Gen, eax);
		context.AppendInstruction(X86.Mov32, result, eax);
	}
}
