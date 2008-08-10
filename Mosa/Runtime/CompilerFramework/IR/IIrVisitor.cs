/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Visitor interface for instructions of the intermediate representation.
    /// </summary>
    public interface IIrVisitor
    {
        /// <summary>
        /// Visitation function for <see cref="EpilogueInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(EpilogueInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="LiteralInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(LiteralInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="MoveInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(MoveInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="PushInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(PopInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="PrologueInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(PrologueInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="PushInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(PushInstruction instruction);

        /// <summary>
        /// Visitation function for <see cref="ReturnInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        void Visit(ReturnInstruction instruction);
    }
}
