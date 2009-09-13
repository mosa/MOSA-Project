/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/* FIXME:
	 * - Schedule compilation of invocation target
	 * - Scheduling puts the target method on the jit or aot compilers work list
	 * - This allows the jit to run async ahead of time in the same process
	 * - This may not turn the jit into a full aot, but is used to prepare MethodCompilers and jit stubs
	 *   for invoked methods.
	 */

	/// <summary>
	/// Base class for instructions, which invoke other functions.
	/// </summary>
	public class InvokeInstruction : CILInstruction
	{      
		
		#region Types

        /// <summary>
        /// Specifies a set of flags used to control invocation target metadata decoding.
        /// </summary>
        [Flags]
        protected enum InvokeSupportFlags
        {
            /// <summary>
            /// Specifies that the invoke instruction supports member references.
            /// </summary>
            MemberRef = 1,
            /// <summary>
            /// Specifies that the invoke instruction supports member definitions.
            /// </summary>
            MethodDef = 2,
            /// <summary>
            /// Specifies that the invoke instruction supports member specifications.
            /// </summary>
            MethodSpec = 4,
            /// <summary>
            /// Specifies that the invoke instruction supports call site invocations.
            /// </summary>
            CallSite = 8,
            /// <summary>
            /// Specifies support for all method invocation targets.
            /// </summary>
            All = MemberRef|MethodDef|MethodSpec|CallSite
        }

        #endregion // Types

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InvokeInstruction"/> class.
		/// </summary>
		public InvokeInstruction()
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			Debug.Assert(OpCode.Nop == opcode, @"Wrong opcode for NopInstruction.");
			if (OpCode.Nop != opcode)
				throw new ArgumentException(@"Wrong opcode.", @"code");

			//instruction.Instruction = this;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return ToString();
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL nop";
		}

		#endregion // Operand Overrides
	}
}
