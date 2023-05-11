// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.x86.CompilerStages;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::GetMultibootEAX")]
	private static void GetMultibootEAX(Context context, MethodCompiler methodCompiler)
	{
		var MultibootEAX = Operand.CreateLabel(MultibootV1Stage.MultibootEAX, methodCompiler.Is32BitPlatform);

		context.SetInstruction(IRInstruction.Load32, context.Result, MultibootEAX, Operand.Constant32_0);
	}
}
