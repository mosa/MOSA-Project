/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.X86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	public class GetControlRegisterBase : IIntrinsicMethod
	{

		private ControlRegister _control;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected GetControlRegisterBase(ControlRegister control)
		{
			_control = control;
		}

		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			Operand result = context.Result;

			RegisterOperand imm = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);

			context.SetInstruction(IR.Instruction.MoveInstruction, imm, new RegisterOperand(new SigType(CilElementType.U4), _control));
			context.AppendInstruction(IR.Instruction.MoveInstruction, result, imm);
		}

		#endregion // Methods

	}
}
