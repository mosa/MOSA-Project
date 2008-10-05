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
    /// Specifies the possible prefixes of IL instructions.
    /// </summary>
    public enum Prefix
    {
        /// <summary>
        /// Indicates a potentially unaligned, but valid memory access for the instruction.
        /// </summary>
        Unaligned   = 0x01,

        /// <summary>
        /// Indicates a volatile memory access, e.g. this memory access should not be optimized away and always 
        /// needs to go to memory.
        /// </summary>
        Volatile    = 0x02,

        /// <summary>
        /// Subsequent call terminates the method. Can be optimized to remove the current method call frame.
        /// </summary>
        Tail        = 0x04,

        /// <summary>
        /// Invoke a member on a value of a variable type.
        /// </summary>
        Constrained = 0x08,

        /// <summary>
        /// Do not perform type, range or null checks on the following instruction.
        /// </summary>
        No          = 0x10,

        /// <summary>
        /// Subsequent array address operation performs no type check at runtime and returns a controlled mutability managed pointer.
        /// </summary>
        ReadOnly    = 0x20
    }

    /// <summary>
    /// Base class for IL prefix instructions.
    /// </summary>
    public class PrefixInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        protected PrefixInstruction(OpCode code) 
            : base(code)
        {
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the prefix flag.
        /// </summary>
        /// <value>A prefix fla</value>
        public Prefix Flags
        {
            get
            {
                Prefix result;
                switch (_code)
                {
                    case OpCode.PreConstrained: result = IL.Prefix.Constrained; break;
                    case OpCode.PreNo: result = IL.Prefix.No; break;
                    case OpCode.PreReadOnly: result = IL.Prefix.ReadOnly; break;
                    case OpCode.PreTail: result = IL.Prefix.Tail; break;
                    case OpCode.PreUnaligned: result = IL.Prefix.Unaligned; break;
                    case OpCode.PreVolatile: result = IL.Prefix.Volatile; break;
                    default:
                        throw new InvalidOperationException(@"Unknown prefix instruction code.");
                }
                return result;
            }
        }

        #endregion // Properties

        #region Data members

        /// <summary>
        /// Singleton instance of the volatile prefix instruction.
        /// </summary>
        public static readonly PrefixInstruction VolatileInstruction = new PrefixInstruction(OpCode.PreVolatile);

        /// <summary>
        /// Singleton instance of the tail prefix instruction.
        /// </summary>
        public static readonly PrefixInstruction TailInstruction = new PrefixInstruction(OpCode.PreTail);

        /// <summary>
        /// Singleton instance of the readonly prefix instruction.
        /// </summary>
        public static readonly PrefixInstruction ReadOnlyInstruction = new PrefixInstruction(OpCode.PreReadOnly);

        #endregion // Data members

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Prefix(this, arg);
        }

        #endregion // Methods
    }
}
