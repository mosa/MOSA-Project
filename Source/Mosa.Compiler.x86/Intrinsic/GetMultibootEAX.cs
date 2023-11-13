// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetMultibootEAX")]
	private static void GetMultibootEAX(Context context, Transform transform)
	{
		var MultibootEAX = Operand.CreateLabel(BaseMultibootStage.MultibootEAX, transform.Is32BitPlatform);

		context.SetInstruction(IR.Load32, context.Result, MultibootEAX, Operand.Constant32_0);
	}
}
