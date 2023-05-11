// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetMethodLookupTable")]
	private static void GetMethodLookupTable(Context context, MethodCompiler methodCompiler)
	{
		var move = methodCompiler.Is32BitPlatform ? IRInstruction.Move32 : IRInstruction.Move64;

		context.SetInstruction(move, context.Result, Operand.CreateLabel(Metadata.MethodLookupTable, methodCompiler.Is32BitPlatform));
	}
}
