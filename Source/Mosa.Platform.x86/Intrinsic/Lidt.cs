/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lidt instruction.
	/// </summary>
	public sealed class Lidt : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			MemoryOperand operand = new MemoryOperand(BuiltInSigType.Ptr, GeneralPurposeRegister.EAX, new System.IntPtr(0));
			context.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(BuiltInSigType.Ptr, GeneralPurposeRegister.EAX), context.Operand1);
			context.AppendInstruction(CPUx86.Instruction.LidtInstruction, null, operand);
		}

		#endregion // Methods

	}
}
