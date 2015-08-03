// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal class GetControlRegisterBase : IIntrinsicPlatformMethod
	{
		private ControlRegister control;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected GetControlRegisterBase(ControlRegister control)
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
			Operand result = context.Result;
			Operand imm = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			context.SetInstruction(X86.MovCR, imm, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, control));
			context.AppendInstruction(X86.Mov, result, imm);
		}

		#endregion Methods
	}
}