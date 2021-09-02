// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::CpuId")]
		private static void CpuId(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X64.CpuId, context.Result, context.Operand1);
		}
	}
}
