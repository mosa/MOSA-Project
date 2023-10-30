// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetCR2")]
	private static void GetCR2(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovCRLoad32, context.Result, transform.PhysicalRegisters.Allocate32(CPURegister.CR2));
	}
}
