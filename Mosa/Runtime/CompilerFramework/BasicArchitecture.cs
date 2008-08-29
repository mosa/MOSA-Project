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

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Implements a basic framework for architectures.
    /// </summary>
    public abstract class BasicArchitecture : IArchitecture
    {
        #region Data members

        /// <summary>
        /// Holds the native type of the architecture.
        /// </summary>
        private SigType _nativeType;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="BasicArchitecture"/>.
        /// </summary>
        protected BasicArchitecture()
        {
        }

        #endregion // Construction

        #region IArchitecture Members

        /// <summary>
        /// Returns the width of a native integer in bits.
        /// </summary>
        public abstract int NativeIntegerSize
        {
            get;
        }

        /// <summary>
        /// Retrieves the signature type of the native integer.
        /// </summary>
        public SigType NativeType 
        {
            get
            {
                if (null == _nativeType)
                {
                    int bits = this.NativeIntegerSize;
                    switch (bits)
                    {
                        case 32:
                            _nativeType = new SigType(CilElementType.I4);
                            break;

                        case 64:
                            _nativeType = new SigType(CilElementType.I8);
                            break;

                        default:
                            throw new NotSupportedException(@"The native bit width is not supported.");
                    }
                }

                return _nativeType;
            }
        }

        /// <summary>
        /// Retrieves the register set of the architecture.
        /// </summary>
        public abstract Register[] RegisterSet { get; }

        /// <summary>
        /// Retrieves the stack frame register of the architecture.
        /// </summary>
        public abstract Register StackFrameRegister { get; }

        /// <summary>
        /// Extends the assembly compiler pipeline with architecture specific assembly compiler stages.
        /// </summary>
        /// <param name="assemblyPipeline">The pipeline to extend.</param>
        public abstract void ExtendAssemblyCompilerPipeline(CompilerPipeline<IAssemblyCompilerStage> assemblyPipeline);
        public abstract void ExtendMethodCompilerPipeline(CompilerPipeline<IMethodCompilerStage> methodPipeline);

        public virtual Instruction CreateInstruction(Type instructionType, params object[] args)
        {
            Debug.Assert(typeof(Instruction).IsAssignableFrom(instructionType));
            return (Instruction)Activator.CreateInstance(instructionType, args, new object[0]);
        }

        public virtual Operand CreateResultOperand(SigType type, int label, int index)
        {
            return new TemporaryOperand(label, type, this.StackFrameRegister, index);
        }

        public abstract ICallingConvention GetCallingConvention(CallingConvention cc);

        public abstract IRegisterConstraint GetRegisterConstraint(Instruction instruction);

        #endregion // IArchitecture Members
    }
}
