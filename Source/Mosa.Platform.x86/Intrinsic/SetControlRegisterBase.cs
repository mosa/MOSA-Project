// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class SetControlRegisterBase : IIntrinsicPlatformMethod
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

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, control);

			context.SetInstruction(X86.Mov, eax, operand1);
			context.AppendInstruction(X86.MovCR, cr, eax);
		}

		#endregion Methods
	}
}
