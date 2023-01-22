// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.ARMv8A32.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.ARMv8A32.Intrinsic::Nop")]
		private static void Nop(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(ARMv8A32.Nop);
		}
	}
}
