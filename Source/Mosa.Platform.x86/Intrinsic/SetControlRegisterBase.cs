/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using System.Collections.Generic;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public class SetControlRegisterBase : IIntrinsicMethod
	{

		private ControlRegister _control;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetControlRegisterBase"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		protected SetControlRegisterBase(ControlRegister control)
		{
			_control = control;
		}

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand operand1 = context.Operand1;

			RegisterOperand imm = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);

			context.SetInstruction(IR.Instruction.MoveInstruction, imm, operand1);
			context.AppendInstruction(IR.Instruction.MoveInstruction, new RegisterOperand(new SigType(CilElementType.U4), _control), imm);
		}

		#endregion // Methods

	}
}
