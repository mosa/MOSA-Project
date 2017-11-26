// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetControlRegisterBase
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class GetControlRegisterBase : IIntrinsicPlatformMethod
	{
		private readonly ControlRegister control;

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
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand result = context.Result;

			context.SetInstruction(X86.MovCRLoad, result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, control));
		}

		#endregion Methods
	}
}
