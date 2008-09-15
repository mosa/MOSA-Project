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
    public interface IIRVisitor<ArgType> : IInstructionVisitor<ArgType>
    {
        /// <summary>
        /// Visitation function for <see cref="AddressOfInstruction"/>.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The visitation context argument.</param>
        void Visit(AddressOfInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="ArithmeticShiftRightInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        void Visit(ArithmeticShiftRightInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="EpilogueInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(EpilogueInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="FloatingPointToIntegerConversion"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(FloatingPointToIntegerConversion instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LiteralInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LiteralInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LoadInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LoadInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LogicalAndInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LogicalAndInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LogicalOrInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LogicalOrInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LogicalXorInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LogicalXorInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="LogicalNotInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(LogicalNotInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="MoveInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(MoveInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="PhiInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(PhiInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="PopInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(PopInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="PrologueInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(PrologueInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="PushInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(PushInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="ReturnInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(ReturnInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="ShiftLeftInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        void Visit(ShiftLeftInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="ShiftRightInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        void Visit(ShiftRightInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="SignExtendedMoveInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(SignExtendedMoveInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="StoreInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(StoreInstruction instruction, ArgType arg);

        /// <summary>
        /// Visitation function for <see cref="ZeroExtendedMoveInstruction"/> instructions.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">The arg.</param>
        void Visit(ZeroExtendedMoveInstruction instruction, ArgType arg);
    }
}
