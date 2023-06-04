// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::StorePointer")]
	private static void StorePointer(Context context, MethodCompiler methodCompiler)
	{
		var instruction = methodCompiler.Is32BitPlatform ? IRInstruction.Store32 : IRInstruction.Store64;

		var operand1 = context.Operand1;
		var operand2 = context.OperandCount == 3 ? context.Operand2 : methodCompiler.ConstantZero;
		var operand3 = context.OperandCount == 3 ? context.Operand3 : context.Operand2;

		LoadStore.Set(context, methodCompiler, instruction, null, operand1, operand2, operand3);
	}
}
