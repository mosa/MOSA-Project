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
    /// 
    /// </summary>
    [Flags]
    public enum PrefixFlags
    {
        /// <summary>
        /// 
        /// </summary>
        Unaligned   = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Volatile    = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Tail        = 0x04,
        /// <summary>
        /// 
        /// </summary>
        Constrained = 0x08,
        /// <summary>
        /// 
        /// </summary>
        No          = 0x10,
        /// <summary>
        /// 
        /// </summary>
        ReadOnly    = 0x20
    }

    /// <summary>
    /// 
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
        /// Retrieves the prefix flag.
        /// </summary>
        public PrefixFlags Flags
        {
            get
            {
                PrefixFlags result;
                switch (_code)
                {
                    case OpCode.PreConstrained: result = PrefixFlags.Constrained; break;
                    case OpCode.PreNo: result = PrefixFlags.No; break;
                    case OpCode.PreReadOnly: result = PrefixFlags.ReadOnly; break;
                    case OpCode.PreTail: result = PrefixFlags.Tail; break;
                    case OpCode.PreUnaligned: result = PrefixFlags.Unaligned; break;
                    case OpCode.PreVolatile: result = PrefixFlags.Volatile; break;
                    default:
                        throw new InvalidOperationException(@"Unknown prefix instruction code.");
                }
                return result;
            }
        }

        #endregion // Properties

        #region Data members

        /// <summary>
        /// 
        /// </summary>
        public static readonly PrefixInstruction Volatile = new PrefixInstruction(OpCode.PreVolatile);
        /// <summary>
        /// 
        /// </summary>
        public static readonly PrefixInstruction Tail = new PrefixInstruction(OpCode.PreTail);
        /// <summary>
        /// 
        /// </summary>
        public static readonly PrefixInstruction ReadOnly = new PrefixInstruction(OpCode.PreReadOnly);

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
