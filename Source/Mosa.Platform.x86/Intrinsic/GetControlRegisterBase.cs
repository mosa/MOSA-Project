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

		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.MovCRLoad, context.Result, Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, control));
		}
	}
}
