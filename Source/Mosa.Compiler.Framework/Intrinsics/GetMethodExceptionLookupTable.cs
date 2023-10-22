// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::GetMethodExceptionLookupTable")]
	private static void GetMethodExceptionLookupTable(Context context, Transform transform)
	{
		context.SetInstruction(transform.MoveInstruction, context.Result, Operand.CreateLabel(Metadata.MethodExceptionLookupTable, transform.Is32BitPlatform));
	}
}
