// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.ARMv6;

namespace Mosa.Platform.ARMv6.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.ARMv6.Intrinsic:Nop")]
		private static void Nop(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(ARMv6.Nop32);
		}
	}
}
