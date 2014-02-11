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
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	public class SetControlRegisterBase : IIntrinsicPlatformMethod
	{
		private ControlRegister control;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected SetControlRegisterBase(ControlRegister control)
		{
			this.control = control;
		}

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand operand1 = context.Operand1;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.Resolver.BuiltIn.UInt32, GeneralPurposeRegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.Resolver.BuiltIn.UInt32, control);

			context.SetInstruction(X86.Mov, eax, operand1);
			context.AppendInstruction(X86.MovCR, cr, eax);
		}

		#endregion Methods
	}
}