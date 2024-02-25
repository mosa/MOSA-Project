// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Invlpg")]
	private static void Invlpg(Context context, Transform transform)
	{
		//Debug.Assert(context.Operand1.IsConstant);
		context.SetInstruction(X64.Invlpg, null, context.Operand1);
	}
}
