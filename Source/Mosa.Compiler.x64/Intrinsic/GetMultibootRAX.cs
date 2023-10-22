// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::GetMultibootRAX")]
	private static void GetMultibootRAX(Context context, Transform transform)
	{
		var MultibootEAX = Operand.CreateLabel(BaseMultibootStage.MultibootEAX, transform.Is32BitPlatform);

		context.SetInstruction(IRInstruction.Load64, context.Result, MultibootEAX, Operand.Constant32_0);
	}
}
