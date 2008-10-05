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
    /// Base class for the intermediate representation for CIL load operations.
    /// </summary>
    public abstract class LoadInstruction : ILInstruction, ILoadInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        protected LoadInstruction(OpCode code)
            : base(code, 0, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="operandCount">The number of operands of the load.</param>
        protected LoadInstruction(OpCode code, int operandCount)
            : base(code, operandCount, 1)
        {
        }

        #endregion // Construction

        #region ILoadInstruction Members

        Operand ILoadInstruction.Destination
        {
            get { return this.Results[0]; }
        }

        Operand ILoadInstruction.Source
        {
            get { return this.Operands[0]; }
        }

        #endregion // ILoadInstruction Members
    }
}
