// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::Jmp")]
		private static void Jmp(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.JmpExternal, null, context.Operand1);
		}
	}
}
