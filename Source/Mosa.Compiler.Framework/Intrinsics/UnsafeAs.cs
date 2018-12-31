// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("System.Runtime.CompilerServices.Unsafe::As")]
		private static void UnsafeAs(Context context, MethodCompiler methodCompiler)
		{
			var instruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.MoveInt32 : IRInstruction.MoveInt64;

			context.SetInstruction(instruction, context.Result, context.Operand1);
		}
	}
}
