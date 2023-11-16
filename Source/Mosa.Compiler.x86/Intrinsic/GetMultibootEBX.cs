// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::GetMultibootEBX")]
	private static void GetMultibootEBX(Context context, Transform transform)
	{
		var MultibootEBX = Operand.CreateLabel(BaseMultibootStage.MultibootEBX, transform.Is32BitPlatform);

		context.SetInstruction(IR.Load32, context.Result, MultibootEBX, Operand.Constant32_0);
	}
}
