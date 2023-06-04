// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetAssemblyListTable")]
	private static void GetAssemblyListTable(Context context, TransformContext transformContext)
	{
		context.SetInstruction(transformContext.MoveInstruction, context.Result, Operand.CreateLabel(Metadata.AssembliesTable, transformContext.Is32BitPlatform));
	}
}
