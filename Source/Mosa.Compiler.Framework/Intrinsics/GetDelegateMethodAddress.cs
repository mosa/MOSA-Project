// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// GetObjectAddress
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetDelegateMethodAddress")]
	public sealed class GetDelegateMethodAddress : IIntrinsicInternalMethod
	{
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			var load = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadInt32 : IRInstruction.LoadInt64;

			context.SetInstruction(load, context.Result, context.Operand1, methodCompiler.CreateConstant(2 * methodCompiler.Architecture.NativePointerSize));
		}
	}
}
