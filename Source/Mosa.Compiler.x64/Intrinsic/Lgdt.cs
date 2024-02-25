// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Lgdt")]
	private static void Lgdt(Context context, Transform transform)
	{
		context.SetInstruction(X64.Lgdt, null, context.Operand1);

		//context.AppendInstruction(X64.JmpFar);s
	}
}
