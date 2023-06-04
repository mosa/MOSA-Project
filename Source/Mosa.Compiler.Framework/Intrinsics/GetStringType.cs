// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStringType")]
	private static void GetStringType(Context context, TransformContext transformContext)
	{
		var result = context.Result;

		var typeDef = Operand.CreateLabel(Metadata.TypeDefinition + "System.String", transformContext.Is32BitPlatform);

		context.SetInstruction(transformContext.MoveInstruction, result, typeDef);
	}
}
