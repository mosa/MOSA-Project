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
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the newobj IL instruction.
    /// </summary>
    /// <remarks>
    /// Actually this is a waste. Newobj is a compound of at least three instructions:
    ///   - pop ctor-args
    ///   - push type
    ///   - push type-size
    ///   - call allocator.new
    ///   - dup
    ///   - push ctor-args
    ///   - call ctor
    /// Note that processing this instruction does require extensive call stack rewriting in order
    /// to insert the this reference in front of all other ctor arguments, even though it is pushed
    /// *after* calling allocator new as seen above. Additionally note that after executing the ctor
    /// call another reference to this is on the stack in order to be able to use the constructed object.
    /// Note that this is very similar to arrays (newarr), except there's no ctor to call.
    /// I don't want to have runtime helpers for newarr and newobj, so we unite both by using a common
    /// allocator, which receives the type and memory size as parameters. This also fixes string
    /// issues for us, which vary in size and thus can't be allocated by a plain newobj.
    /// <para/>
    /// These details are automatically processed by the Expand function, which expands this highlevel
    /// opcode into its parts as described above. The exception is that Expand is not stack based anymore
    /// and uses virtual registers to implement two calls:
    /// - this-vreg = allocator-new(type-vreg, type-size-vreg)
    /// - ctor(this-vreg[, args])
    /// <para/>
    /// Those calls are ultimately processed by further expansion and inlining, except that allocator-new
    /// is a kernel call and can't be inlined - even by the jit.
    /// <para/>
    /// The expansion essentially adds a dependency to mosacor, which provides the allocator and gc.
    /// </remarks>
    public class NewobjInstruction : InvokeInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="NewobjInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
        public NewobjInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Newobj == code);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the supported immediate metadata tokens in the instruction.
        /// </summary>
        /// <value></value>
        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get
            {
                return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            /*
             * HACK: We need to remove the this parameter from the operand list, as it
             * is not available yet. It is implicitly created by newobj and appropriately
             * passed. So we do as if it doesn't exist. Upon instruction expansion a call
             * to the allocator is inserted and its result is the this pointer passed. This
             * must be done by expansion though...
             * 
             */
            base.Decode(decoder);

            // Remove the this parameter
            // FIXME: _operands = new Operand[_operands.Length-1];

            // Set the return value, even though constructors return void
            throw new NotImplementedException();
            //SetResult(0, null);
                //CreateResultOperand(_invokeTarget.DeclaringType)
        }

        /// <summary>
        /// Convert the call to a string.
        /// </summary>
        /// <returns>The string of the call expression.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
//            int opIdx = 0;

            // Output the result...
            builder.AppendFormat("{0} = newobj ", this.Results[0]);


            // Output the call name
            builder.Append(_invokeTarget);
/*
            if (opIdx < _operands.Length)
            {
                builder.Append('(');

                while (opIdx < _operands.Length)
                {
                    builder.AppendFormat("{0}, ", _operands[opIdx++]);
                }

                builder.Remove(builder.Length - 2, 2);
                builder.Append(')');
            }
            else
                builder.Append("()");
*/
            return builder.ToString();
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");

            // HACK: Don't validate the base class - it still assumes a method call without the this ptr required
            // for constructors.
            //base.Validate(compiler);

            // Validate the operands...
            Operand[] ops = this.Operands;
            Debug.Assert(ops.Length == this._invokeTarget.Parameters.Count - 1, @"Operand count doesn't match parameter count.");
            for (int i = 0; i < ops.Length; i++)
            {
                if (null != ops[i])
                {
                    /* FIXME: Check implicit conversions
                                        Debug.Assert(_operands[i].Type == _parameterTypes[i]);
                                        if (_operands[i].Type != _parameterTypes[i])
                                        {
                                            // FIXME: Determine if we can do an implicit conversion
                                            throw new ExecutionEngineException(@"Invalid operand types.");
                                        }
                     */
                }
            }            
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Newobj(this, arg);
        }

        #endregion // Methods
    }
}
