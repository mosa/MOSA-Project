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
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Represents the call indirect CIL instruction in IR.
    /// </summary>
    public class CalliInstruction : InvokeInstruction
    {
        #region Data members

        /// <summary>
        /// The target of the calli instruction.
        /// </summary>
        private Operand _callTarget;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="CalliInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the calli instruction.</param>
        public CalliInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Calli == code);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets/Sets the target of the calli instruction.
        /// </summary>
        public Operand CallTarget
        {
            get { return _callTarget; }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (value.Type != _callTarget.Type)
                    throw new ArgumentException(@"Operand type is not mutable.", @"value");

                _callTarget = value;
            }
        }

        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.CallSite; }
        }
/* FIXME
        public override Operand[] Operands
        {
            get
            {
                List<Operand> l = new List<Operand>();
                l.Add(_callTarget);
                return l.ToArray();
            }
            set
            {
                // Set the call target explicitly
                this.CallTarget = value[0];

                _operands = new Operand[value.Length - 1];
                Array.Copy(value, 1, _operands, 0, value.Length - 1);
            }
        }
*/
        #endregion // Properties

        #region Methods

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Calli(this);
        }

        #endregion // Methods
    }
}
