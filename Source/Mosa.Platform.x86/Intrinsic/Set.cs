/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	public sealed class Set : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand dest = context.Operand1;
			Operand value = context.Operand2;

			Operand v1 = methodCompiler.CreateVirtualRegister(dest.Type);
			Operand v2 = methodCompiler.CreateVirtualRegister(value.Type);
			Operand memory = Operand.CreateMemoryAddress(context.InvokeMethod.Parameters[1].Type, v1, 0);

			context.SetInstruction(X86.Mov, v1, dest);
			context.AppendInstruction(X86.Mov, v2, value);
			context.AppendInstruction(X86.Mov, memory, v2);
		}

		#endregion Methods
	}
}