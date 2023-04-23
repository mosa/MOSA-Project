// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStackPointer")]
	private static void GetStackPointer(Context context, MethodCompiler methodCompiler)
	{
		var instruction = methodCompiler.Is32BitPlatform ? IRInstruction.Move32 : IRInstruction.Move64;

		context.SetInstruction(instruction, context.Result, methodCompiler.Compiler.StackPointer);
	}
}
