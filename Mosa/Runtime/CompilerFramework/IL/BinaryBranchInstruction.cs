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

        public int[] BranchTargets
        {
            get { return _targets; }
            set { _targets = value; }
        }

        public override FlowControl FlowControl
        {
            get { return FlowControl.ConditionalBranch; }
        }

        public bool IsConditional { get { return true; } }

		#endregion // Properties

		#region BinaryInstruction Overrides

        public override void Decode(IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(decoder);

            _targets = new int[2];
            _targets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
            // FIXME: Remove unary branch instructions from this list.
			if (_code == OpCode.Beq_s || _code == OpCode.Bge_s || _code == OpCode.Bge_un_s || _code == OpCode.Bgt_s ||
				_code == OpCode.Bgt_un_s || _code == OpCode.Ble_s || _code == OpCode.Ble_un_s || _code == OpCode.Blt_s ||
				_code == OpCode.Blt_un_s || _code == OpCode.Bne_un_s)
			{
				_targets[0] = decoder.DecodeSByte();
			}
			else if (_code == OpCode.Beq || _code == OpCode.Bge || _code == OpCode.Bge_un || _code == OpCode.Bgt ||
				_code == OpCode.Bgt_un || _code == OpCode.Ble || _code == OpCode.Ble_un || _code == OpCode.Blt ||
				_code == OpCode.Blt_un || _code == OpCode.Bne_un)
			{
                _targets[0] = decoder.DecodeInt32();
			}
            else
            {
                throw new NotSupportedException(@"Invalid branch opcode specified for BinaryBranchInstruction");
            }
		}

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
