// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::SetFS")]
	private static void SetFS(Context context, TransformContext transformContext)
	{
		context.SetInstruction(X64.MovStoreSeg64, Operand.CreateCPURegister64(CPURegister.FS), context.Operand1);
	}
}
