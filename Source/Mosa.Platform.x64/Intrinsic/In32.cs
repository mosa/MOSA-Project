// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::In32")]
		private static void In32(Context context, MethodCompiler methodCompiler)
		{
			var v1 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U8);

			var result = context.Result;

			context.SetInstruction(X64.In32, v1, context.Operand1);
			context.AppendInstruction(X64.Movzx16To64, result, v1);
		}
	}
}
