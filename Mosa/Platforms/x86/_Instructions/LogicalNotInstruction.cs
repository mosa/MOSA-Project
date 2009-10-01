/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the X86 NOT instruction.
    /// </summary>
    sealed class LogicalNotInstruction : IR.LogicalNotInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalNotInstruction"/> class.
        /// </summary>
        public LogicalNotInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalNotInstruction"/> class.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        public LogicalNotInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
        }

        #endregion // Construction
    }
}
