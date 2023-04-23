// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStringType")]
	private static void GetStringType(Context context, MethodCompiler methodCompiler)
	{
		var result = context.Result;

		var typeDef = Operand.CreateLabel(Metadata.TypeDefinition + "System.String", methodCompiler.Is32BitPlatform);

		var move = methodCompiler.Is32BitPlatform ? IRInstruction.Move32 : IRInstruction.Move64;

		context.SetInstruction(move, result, typeDef);
	}
}
