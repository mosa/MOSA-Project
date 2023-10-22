// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::WrMSR")]
	private static void WrMSR(Context context, Transform transform)
	{
		context.SetInstruction(X86.WrMSR, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
