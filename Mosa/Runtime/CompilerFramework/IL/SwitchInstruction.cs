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

		#region Methods

        public override void Decode(IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(decoder);

			// Retrieve the number of branch targets
			uint count = decoder.DecodeUInt32();

			// Create an array for the branch targets
			_branchTargets = new int[count];

			// Populate the array
			for (uint i = 0; i < count; i++)
			{
				_branchTargets[i] = decoder.DecodeInt32();
			}
        }

        public override string ToString()
        {
            // FIXME:
            return String.Format(@"switch (...)");
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Switch(this);
        }

        #endregion // Methods
    }
}
