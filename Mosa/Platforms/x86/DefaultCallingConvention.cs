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

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Implements the CIL default calling convention for x86.
    /// </summary>
    sealed class DefaultCallingConvention : ICallingConvention
    {
        #region Static data members

        /// <summary>
        /// Holds the single instance of the default calling convention.
        /// </summary>
        public static readonly DefaultCallingConvention Instance = new DefaultCallingConvention();

        #endregion // Static data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
        /// </summary>
        private DefaultCallingConvention()
        { 
        }

        #endregion // Construction

        #region ICallingConvention Members

        object ICallingConvention.Expand(IArchitecture architecture, IL.InvokeInstruction instruction)
        {
            throw new NotImplementedException();
        }

        Instruction[] ICallingConvention.MoveReturnValue(IArchitecture architecture, Operand operand)
        {
            int size, alignment;
            GetStackRequirements(operand.Type, out size, out alignment);

            if (4 == size)
            {
                return new Instruction[] { architecture.CreateInstruction(typeof(MoveInstruction), new RegisterOperand(operand.Type, GeneralPurposeRegister.EAX), operand) };
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        void ICallingConvention.GetStackRequirements(StackOperand stackOperand, out int size, out int alignment)
        {
            // Special treatment for some stack types
            // FIXME: Handle the size and alignment requirements of value types
            GetStackRequirements(stackOperand.Type, out size, out alignment);
        }

        private void GetStackRequirements(SigType sigType, out int size, out int alignment)
        {
            switch (sigType.Type)
            {
                case CilElementType.R4: goto case CilElementType.R8;
                case CilElementType.R8:
                    // Default alignment and size are 4
                    size = alignment = 8;
                    break;

                case CilElementType.I8: goto case CilElementType.U8;
                case CilElementType.U8:
                    size = alignment = 8;
                    break;

                case CilElementType.ValueType:
                    throw new NotSupportedException();

                default:
                    size = alignment = 4;
                    break;
            }
        }

        int ICallingConvention.OffsetOfFirstLocal
        {
            get 
            { 
                /*
                 * The first local variable is offset by 8 bytes from the start of
                 * the stack frame. [EBP-08h] (The first stack slot available for
                 * locals is [EBP], so we're reserving two 32-bit ints for
                 * system/compiler use as described below.
                 * 
                 * The first 4 bytes hold the method token, so that the GC can
                 * retrieve the method GC map and that we can do smart stack traces.
                 * 
                 * The second 4 bytes are used to hold the start of the method,
                 * so that we can embed floating point constants in our PIC.
                 * 
                 */
                return -8; 
            }
        }

        #endregion // ICallingConvention Members
    }
}
