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
        /// <summary>
        /// Requests the architecture to add architecture specific compilation stages to the pipeline. These
        /// may depend upon the current state of the pipeline.
        /// </summary>
        /// <param name="methodPipeline">The pipeline of the method compiler to add architecture specific compilation stages to.</param>
        public abstract void ExtendMethodCompilerPipeline(CompilerPipeline<IMethodCompilerStage> methodPipeline);

        /// <summary>
        /// Factory method for instruction object instances.
        /// </summary>
        /// <param name="instructionType">The type of the instruction to create.</param>
        /// <param name="args">Array of arguments to pass to the instruction type.</param>
        /// <returns>
        /// An instance of Instruction or a derived class to represent the requested opcode.
        /// </returns>
        public virtual Instruction CreateInstruction(Type instructionType, params object[] args)
        {
            Debug.Assert(typeof(Instruction).IsAssignableFrom(instructionType));
            return (Instruction)Activator.CreateInstance(instructionType, args, new object[0]);
        }

        /// <summary>
        /// Factory method for result operands of instructions.
        /// </summary>
        /// <param name="type">The datatype held in the result operand.</param>
        /// <param name="label">The label.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The operand, which holds the instruction result.
        /// </returns>
        public virtual Operand CreateResultOperand(SigType type, int label, int index)
        {
            return new TemporaryOperand(label, type, this.StackFrameRegister, index);
        }

        /// <summary>
        /// Retrieves an object, that is able to translate the CIL calling convention into appropriate native code.
        /// </summary>
        /// <param name="cc">The CIL calling convention to translate.</param>
        /// <returns>A calling convention implementation.</returns>
        public abstract ICallingConvention GetCallingConvention(CallingConvention cc);

        /// <summary>
        /// Retrieves a register constraint description object.
        /// </summary>
        /// <param name="instruction">The instruction to retrieve the register constraint description for.</param>
        /// <returns>A register constraint descriptor.</returns>
        public abstract IRegisterConstraint GetRegisterConstraint(Instruction instruction);

        /// <summary>
        /// Gets object file builders for this architecture
        /// </summary>
        /// <returns>A list of ObjectFileBuilders</returns>
        public abstract ObjectFileBuilderBase[] GetObjectFileBuilders();

        /// <summary>
        /// Gets the type memory requirements.
        /// </summary>
        /// <param name="type">The signature type.</param>
        /// <param name="size">Receives the memory size of the type.</param>
        /// <param name="alignment">Receives alignment requirements of the type.</param>
        public abstract void GetTypeRequirements(SigType type, out int size, out int alignment);

        #endregion // IArchitecture Members
    }
}
