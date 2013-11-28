/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Div instruction.
	/// </summary>
	public sealed class Div : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand n = context.Operand1;
			Operand d = context.Operand2;
			Operand result = context.Result;
			Operand result2 = methodCompiler.CreateVirtualRegister(BuiltInSigType.UInt32);
			
			Operand op0L, op0H;

			LongOperandTransformationStage.SplitLongOperand(methodCompiler, n, out op0L, out op0H, null);

			context.SetInstruction2(X86.Div, result2, result, op0H, op0L, d);
		}

		#endregion Methods
	}
}