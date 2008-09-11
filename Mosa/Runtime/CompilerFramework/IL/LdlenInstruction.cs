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
using System.Diagnostics;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the ldlen IL instruction.
    /// </summary>
    public class LdlenInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LdlenInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode, which must be OpCode.Ldlen.</param>
        public LdlenInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Ldlen == code);
            if (OpCode.Ldlen != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format("{2} ; {0} = len({1})", this.Results[0], this.Operands[0], base.ToString());
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);

            Mosa.Runtime.Metadata.Signatures.ArraySigType a = this.Operands[0].Type as Mosa.Runtime.Metadata.Signatures.ArraySigType;
            if (null == a || 1 != a.Rank)
                throw new InvalidProgramException(@"Operand to ldlen is not a vector.");
            SetResult(0, compiler.CreateResultOperand(new SigType(CilElementType.I)));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldlen(this, arg);
        }

        #endregion // Methods
    }
}
