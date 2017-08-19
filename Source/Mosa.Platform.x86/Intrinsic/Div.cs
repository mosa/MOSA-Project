// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Div instruction.
	/// </summary>
	internal sealed class Div : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand n = context.Operand1;
			Operand d = context.Operand2;
			Operand result = context.Result;
			Operand result2 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			LongOperandTransformationStage.SplitLongOperand(methodCompiler, n, out Operand op0L, out Operand op0H);

			context.SetInstruction2(X86.Div, result2, result, op0H, op0L, d);
		}

		#endregion Methods
	}
}
