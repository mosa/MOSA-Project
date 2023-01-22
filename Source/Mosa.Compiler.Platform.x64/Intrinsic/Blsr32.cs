// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::Blsr32")]
		private static void Blsr32(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.Blsr32, context.Result, context.Operand1);
		}
	}
}
