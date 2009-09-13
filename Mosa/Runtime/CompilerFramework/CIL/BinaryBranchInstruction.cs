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
	/// <summary>
	/// 
	/// </summary>
	public class BinaryBranchInstruction : BinaryInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the CIL opcode
		/// </summary>
		private OpCode opCode;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryBranchInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public BinaryBranchInstruction(OpCode opCode)
		{
			this.opCode = opCode;
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
			Debug.Assert(opCode == opcode, @"Wrong opcode for BinaryBranch.");
			if (opCode != opcode)
				throw new ArgumentException(@"Wrong opcode.", @"code");

			// Decode base classes first
			// Decode base classes first
			base.Decode(ref instruction, opcode, decoder);

			instruction.Branch = new Branch(2);
			instruction.Branch.BranchTargets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
			// FIXME: Remove unary branch instructions From this list.
			if (opcode == OpCode.Beq_s || opcode == OpCode.Bge_s || opcode == OpCode.Bge_un_s || opcode == OpCode.Bgt_s ||
				opcode == OpCode.Bgt_un_s || opcode == OpCode.Ble_s || opcode == OpCode.Ble_un_s || opcode == OpCode.Blt_s ||
				opcode == OpCode.Blt_un_s || opcode == OpCode.Bne_un_s) {
				sbyte target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else if (opcode == OpCode.Beq || opcode == OpCode.Bge || opcode == OpCode.Bge_un || opcode == OpCode.Bgt ||
				opcode == OpCode.Bgt_un || opcode == OpCode.Ble || opcode == OpCode.Ble_un || opcode == OpCode.Blt ||
				opcode == OpCode.Blt_un || opcode == OpCode.Bne_un) {
				int target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else {
				throw new NotSupportedException(@"Invalid branch opcode specified for BinaryBranchInstruction");
			}
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
			string op, format;
			bool unordered = false;
			switch (opCode) {
				case OpCode.Beq_s: op = @"=="; break;
				case OpCode.Beq: op = @"=="; break;
				case OpCode.Bge_s: op = @">="; break;
				case OpCode.Bge: op = @">="; break;
				case OpCode.Bge_un_s: op = @">="; unordered = true; break;
				case OpCode.Bge_un: op = @">="; unordered = true; break;
				case OpCode.Bgt_s: op = @">"; break;
				case OpCode.Bgt: op = @">"; break;
				case OpCode.Bgt_un_s: op = @">"; unordered = true; break;
				case OpCode.Bgt_un: op = @">"; unordered = true; break;
				case OpCode.Ble_s: op = @"<="; break;
				case OpCode.Ble: op = @"<="; break;
				case OpCode.Ble_un_s: op = @"<="; unordered = true; break;
				case OpCode.Ble_un: op = @"<="; unordered = true; break;
				case OpCode.Blt_s: op = @"<"; break;
				case OpCode.Blt: op = @"<"; break;
				case OpCode.Blt_un_s: op = @"<"; unordered = true; break;
				case OpCode.Blt_un: op = @"<"; unordered = true; break;
				case OpCode.Bne_un_s: op = @"!="; unordered = true; break;
				case OpCode.Bne_un: op = @"!="; unordered = true; break;
				default:
					throw new InvalidOperationException(@"Opcode not set.");
			}

			if (!unordered)
				format = @"{3} ; if ({0} {1} {2}) goto L_{4:X4}";
			else
				format = @"{3} ; if (unordered({0} {1} {2})) goto L_{4:X4} else goto L_{5:X4}";

			return String.Format(format, instruction.Operand1, op, instruction.Operand2, base.ToString(), instruction.Branch.BranchTargets[0], instruction.Branch.BranchTargets[1]);
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL "+opCode.ToString();
		}

		#endregion // Operand Overrides
	}
}
