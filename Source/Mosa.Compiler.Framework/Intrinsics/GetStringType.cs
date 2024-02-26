// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetStringType")]
	private static void GetStringType(Context context, Transform transform)
	{
		var result = context.Result;

		var typeDef = Operand.CreateLabel(Metadata.TypeDefinition + "System.String", transform.Is32BitPlatform);

		context.SetInstruction(transform.MoveInstruction, result, typeDef);
	}
}
