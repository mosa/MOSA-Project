/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// X86 specific representation of the NOT instruction.
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
        /// <param name="result">The result.</param>
        /// <param name="op1">The op1.</param>
        public LogicalNotInstruction(Operand result, Operand op1) :
            base(result, op1)
        {
        }

        #endregion // Construction
    }
}
