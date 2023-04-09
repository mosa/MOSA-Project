// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetDelegateTargetAddress")]
	private static void GetDelegateTargetAddress(Context context, MethodCompiler methodCompiler)
	{
		var load = methodCompiler.Is32BitPlatform ? (BaseInstruction)IRInstruction.Load32 : IRInstruction.Load64;

		context.SetInstruction(load, context.Result, context.Operand1, methodCompiler.CreateConstant(methodCompiler.Architecture.NativePointerSize));
	}
}
