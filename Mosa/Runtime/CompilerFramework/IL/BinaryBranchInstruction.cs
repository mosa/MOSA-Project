/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Intermediate representation for binary IL branch instructions.
	/// </summary>
	public class BinaryBranchInstruction : BinaryInstruction, IBranchInstruction {
		#region Data members

        /// <summary>
        /// Branch target.
        /// </summary>
		private int[] _targets;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryBranchInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the binary branch instruction.</param>
		public BinaryBranchInstruction(OpCode code)
			: base(code)
		{
		}

		#endregion // Construction

		#region Properties

        /// <summary>
        /// Returns the branch targets instruction index.
        /// </summary>
        /// <value></value>
        public int[] BranchTargets
        {
            get { return _targets; }
            set { _targets = value; }
        }

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

        /// <summary>
        /// Determines if the branch is conditional.
        /// </summary>
        /// <value></value>
        public bool IsConditional { get { return true; } }

		#endregion // Properties

		#region BinaryInstruction Overrides

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(decoder);

            _targets = new int[2];
            _targets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
            // FIXME: Remove unary branch instructions From this list.
			if (_code == OpCode.Beq_s || _code == OpCode.Bge_s || _code == OpCode.Bge_un_s || _code == OpCode.Bgt_s ||
				_code == OpCode.Bgt_un_s || _code == OpCode.Ble_s || _code == OpCode.Ble_un_s || _code == OpCode.Blt_s ||
				_code == OpCode.Blt_un_s || _code == OpCode.Bne_un_s)
			{
                sbyte target;
                decoder.Decode(out target);
				_targets[0] = target;
			}
			else if (_code == OpCode.Beq || _code == OpCode.Bge || _code == OpCode.Bge_un || _code == OpCode.Bgt ||
				_code == OpCode.Bgt_un || _code == OpCode.Ble || _code == OpCode.Ble_un || _code == OpCode.Blt ||
				_code == OpCode.Blt_un || _code == OpCode.Bne_un)
			{
                decoder.Decode(out _targets[0]);
			}
            else
            {
                throw new NotSupportedException(@"Invalid branch opcode specified for BinaryBranchInstruction");
            }
		}

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            string op, format;
            bool unordered = false;
            switch (_code)
            {
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

            if (false == unordered)
                format = @"{3} ; if ({0} {1} {2}) goto L_{4:X4}";
            else
                format = @"{3} ; if (unordered({0} {1} {2})) goto L_{4:X4} else goto L_{5:X4}";

            Operand[] ops = this.Operands;
            return String.Format(format, ops[0], op, ops[1], base.ToString(), _targets[0], _targets[1]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.BinaryBranch(this, arg);
        }

        #endregion // BinaryInstruction Overrides
	}
}
