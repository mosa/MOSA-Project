// -----------------------------------------------------------------------------------------------------------
// <copyright file="BasicArchitecture.cs" company="(C) 2008-2009 MOSA - The Managed Operating System Alliance">
//  
// (c) 2008-2009 MOSA - The Managed Operating System Alliance
// 
// Licensed under the terms of the New BSD License.
//  
// Authors:
//   Michael Ruck (mailto:sharpos@michaelruck.de)
//   
// </copyright>
// -----------------------------------------------------------------------------------------------------------
namespace Mosa.Runtime.CompilerFramework
{
    using System;
    using System.Diagnostics;
    using Metadata;
    using Metadata.Signatures;

    /// <summary>
    /// Implements a basic framework for architectures.
    /// </summary>
    public abstract class BasicArchitecture : IArchitecture
    {
        /// <summary>
        /// Holds the native type of the architecture.
        /// </summary>
        private SigType _nativeType;

        /// <summary>
        /// Gets the width of a native integer in bits.
        /// </summary>
        public abstract int NativeIntegerSize { get; }

        /// <summary>
        /// Gets the register set of the architecture.
        /// </summary>
        public abstract Register[] RegisterSet { get; }

        /// <summary>
        /// Gets the stack frame register of the architecture.
        /// </summary>
        public abstract Register StackFrameRegister { get; }

        /// <summary>
        /// Gets the signature type of the native integer.
        /// </summary>
        public SigType NativeType
        {
            get
            {
                if (null == _nativeType)
                {
                    int bits = NativeIntegerSize;
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
        public abstract IRegisterConstraint GetRegisterConstraint(LegacyInstruction instruction);

        /// <summary>
        /// Gets the type memory requirements.
        /// </summary>
        /// <param name="type">The signature type.</param>
        /// <param name="size">Receives the memory size of the type.</param>
        /// <param name="alignment">Receives alignment requirements of the type.</param>
        public abstract void GetTypeRequirements(SigType type, out int size, out int alignment);

        /// <summary>
        /// Factory method for instruction object instances.
        /// </summary>
        /// <param name="instructionType">The type of the instruction to create.</param>
        /// <param name="args">Array of arguments to pass to the instruction type.</param>
        /// <returns>
        /// An instance of Instruction or a derived class to represent the requested opcode.
        /// </returns>
        public virtual LegacyInstruction CreateInstruction(Type instructionType, params object[] args)
        {
            Debug.Assert(typeof(LegacyInstruction).IsAssignableFrom(instructionType), @"Requested InstructionType is not an instruction.");
            return (LegacyInstruction) Activator.CreateInstance(instructionType, args, new object[0]);
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
            return new TemporaryOperand(label, type, StackFrameRegister, index);
        }
    }
}