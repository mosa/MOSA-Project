// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Internal.Intrinsic::GetObjectAddress")]
	[ReplacementTarget("Mosa.Internal.Intrinsic::GetValueTypeAddress")]
	public sealed class GetObjectAddress : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			if (operand1.IsValueType)
			{
				operand1 = context.Previous.Operand1;
				context.Previous.Empty();
				context.SetInstruction(IRInstruction.AddressOf, result, operand1);
				return;
			}
			context.SetInstruction(IRInstruction.Move, result, operand1);
		}
	}
}
