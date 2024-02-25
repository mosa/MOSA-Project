// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::GetCR0")]
	private static void GetCR0(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovCRLoad64, context.Result, transform.PhysicalRegisters.Allocate64(CPURegister.CR0));
	}
}
