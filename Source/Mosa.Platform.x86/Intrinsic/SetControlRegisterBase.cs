// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// SetControlRegisterBase
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal abstract class SetControlRegisterBase : IIntrinsicPlatformMethod
	{
		private readonly PhysicalRegister control;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected SetControlRegisterBase(PhysicalRegister control)
		{
			this.control = control;
		}

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			Operand operand1 = context.Operand1;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, control);

			context.SetInstruction(X86.Mov32, eax, operand1);
			context.AppendInstruction(X86.MovCRStore32, null, cr, eax);
		}

		#endregion Methods
	}
}
