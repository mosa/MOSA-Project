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

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Provides a visitor interface for instructions.
    /// </summary>
    /// <typeparam name="ArgType">Specifies the type of the additional argument, which provides context to the visitor.</typeparam>
    public interface IInstructionVisitor<ArgType>
    {
        /// <summary>
        /// Visitation method for instructions not caught by more specific visitation methods.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        void Visit(Instruction instruction, ArgType arg);
    }
}
