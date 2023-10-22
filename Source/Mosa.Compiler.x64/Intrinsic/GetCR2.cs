// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::GetCR2")]
	private static void GetCR2(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovCRLoad64, context.Result, Operand.CreateCPURegister64(CPURegister.CR2));
	}
}
