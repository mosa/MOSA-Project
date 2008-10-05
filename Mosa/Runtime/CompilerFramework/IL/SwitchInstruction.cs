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
using System.IO;

namespace Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Intermediate representation for the IL switch instruction.
	/// </summary>
	public class SwitchInstruction : UnaryBranchInstruction {
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SwitchInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the switch instruction.</param>
		public SwitchInstruction(OpCode code)
			: base(code)
		{
			if (OpCode.Switch != code)
				throw new ArgumentException(@"Not a switch statement opcode.", @"code");
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
            get { return FlowControl.Switch; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Branches can not be folded.
        /// </summary>
        /// <param name="decoder"></param>
        public override void Decode(IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(decoder);

			// Retrieve the number of branch targets
            uint count;
            decoder.Decode(out count);

			// Create an array for the branch targets
			_branchTargets = new int[count];

			// Populate the array
			for (uint i = 0; i < count; i++)
			{
                decoder.Decode(out _branchTargets[i]);
			}
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            // FIXME:
            return String.Format(@"switch (...)");
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Switch(this, arg);
        }

        #endregion // Methods
    }
}
