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
	public class BinaryBranchInstruction : BinaryInstruction, IBranchInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryBranchInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The opcode.</param>
		public BinaryBranchInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl
		{
			get { return FlowControl.ConditionalBranch; }
		}

		#endregion // Properties

		#region CILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			instruction.Branch = new Branch(2);
			instruction.Branch.Targets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
			// FIXME: Remove unary branch instructions From this list.
			if (_opcode == OpCode.Beq_s || _opcode == OpCode.Bge_s || _opcode == OpCode.Bge_un_s || _opcode == OpCode.Bgt_s ||
				_opcode == OpCode.Bgt_un_s || _opcode == OpCode.Ble_s || _opcode == OpCode.Ble_un_s || _opcode == OpCode.Blt_s ||
				_opcode == OpCode.Blt_un_s || _opcode == OpCode.Bne_un_s) {
				sbyte target;
				decoder.Decode(out target);
				instruction.Branch.Targets[0] = target;
			}
			else if (_opcode == OpCode.Beq || _opcode == OpCode.Bge || _opcode == OpCode.Bge_un || _opcode == OpCode.Bgt ||
				_opcode == OpCode.Bgt_un || _opcode == OpCode.Ble || _opcode == OpCode.Ble_un || _opcode == OpCode.Blt ||
				_opcode == OpCode.Blt_un || _opcode == OpCode.Bne_un) {
				int target;
				decoder.Decode(out target);
				instruction.Branch.Targets[0] = target;
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
			switch (_opcode) {
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
				format = @" {3} ; if ({0} {1} {2}) goto L_{4:X4}";
			else
				format = @" {3} ; if (unordered({0} {1} {2})) goto L_{4:X4} else goto L_{5:X4}";

			return base.ToString() + String.Format(format, instruction.Operand1, op, instruction.Operand2, base.ToString(), instruction.Branch.Targets[0], instruction.Branch.Targets[1]);
		}

		#endregion // CILInstruction Overrides

		/// <summary>
		/// Determines if the branch is conditional.
		/// </summary>
		/// <value></value>
		public bool IsConditional { get { return true; } }
	}
}
