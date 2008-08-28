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

        public BasicArchitecture()
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

        public abstract Register[] RegisterSet { get; }

        public abstract Register StackFrameRegister { get; }

        public abstract void ExtendAssemblyCompilerPipeline(CompilerPipeline<IAssemblyCompilerStage> assemblyPipeline);
        public abstract void ExtendMethodCompilerPipeline(CompilerPipeline<IMethodCompilerStage> methodPipeline);

        public virtual Instruction CreateInstruction(Type instructionType, params object[] args)
        {
            Debug.Assert(typeof(Instruction).IsAssignableFrom(instructionType));
            return (Instruction)Activator.CreateInstance(instructionType, args, new object[0]);
        }

        public abstract Operand CreateResultOperand(Instruction instruction, SigType type);

        public abstract ICallingConvention GetCallingConvention(CallingConvention cc);

        #endregion // IArchitecture Members
    }
}
