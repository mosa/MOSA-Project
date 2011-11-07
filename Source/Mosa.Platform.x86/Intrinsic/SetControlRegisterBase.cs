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
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using IR = Mosa.Compiler.Framework.IR;

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

			RegisterOperand imm = new RegisterOperand(BuiltInSigType.UInt32, GeneralPurposeRegister.EAX);

			context.SetInstruction(IR.Instruction.MoveInstruction, imm, operand1);
			context.AppendInstruction(IR.Instruction.MoveInstruction, new RegisterOperand(BuiltInSigType.UInt32, _control), imm);
		}

		#endregion // Methods

	}
}
