// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::LoadPointer")]
	private static void LoadPointer(Context context, MethodCompiler methodCompiler)
	{
		var instruction = methodCompiler.Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.LoadZeroExtend32x64;

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 2 ? context.Operand2 : methodCompiler.ConstantZero;

		LoadStore.Set(context, methodCompiler, instruction, result, operand1, operand2);
	}
}
