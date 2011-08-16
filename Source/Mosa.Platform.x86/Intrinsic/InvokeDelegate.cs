/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 cli instruction.
	/// </summary>
	public sealed class InvokeDelegate : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			var eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
			context.SetInstruction(CPUx86.Instruction.MovInstruction, eax, context.Operand2);
			context.AppendInstruction(CPUx86.Instruction.AddInstruction, eax, new ConstantOperand(new SigType(CilElementType.I), 0x35));
			context.AppendInstruction(CPUx86.Instruction.JmpInstruction, null, new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX));
		}

		#endregion // Methods

	}
}
