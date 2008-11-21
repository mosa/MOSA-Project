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

using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using Mosa.ObjectFiles.Elf32;
using Mosa.Platforms.x86.Constraints;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// This class provides a common base class for architecture
    /// specific operations.
    /// </summary>
    public class Architecture : BasicArchitecture
    {
        #region Data members

        private static Dictionary<Type, Type> s_constraints = new Dictionary<Type, Type>()
        {
            //{ typeof(x86.Instructions.IntInstruction), typeof(IntConstraint) },
            { typeof(x86.Instructions.AddInstruction), typeof(GPRConstraint) },
            { typeof(x86.Instructions.AdcInstruction), typeof(GPRConstraint) },
            { typeof(x86.Instructions.DivInstruction), typeof(DivConstraint) },
            { typeof(x86.Instructions.LogicalAndInstruction), typeof(LogicalAndConstraint) },
            { typeof(x86.Instructions.LogicalOrInstruction), typeof(LogicalOrConstraint) },
            { typeof(x86.Instructions.LogicalXorInstruction), typeof(LogicalXorConstraint) },
            { typeof(IR.MoveInstruction), typeof(MoveConstraint) },
            { typeof(x86.Instructions.MulInstruction), typeof(MulConstraint) },
            { typeof(x86.Instructions.SarInstruction), typeof(ShiftConstraint) },
            { typeof(x86.Instructions.ShlInstruction), typeof(ShiftConstraint) },
            { typeof(x86.Instructions.ShrInstruction), typeof(ShiftConstraint) },
            { typeof(x86.Instructions.SubInstruction), typeof(GPRConstraint) },
        };

        /// <summary>
        /// Remaps specific IL intermediate representation types to more x86 specific ones.
        /// </summary>
        private static Dictionary<Type, Type> s_irTypes = new Dictionary<Type, Type>
        {
/*
            { typeof(IL.AddInstruction), typeof(x86.AddInstruction) },
            { typeof(IL.SubInstruction), typeof(x86.SubInstruction) },
            { typeof(IL.MulInstruction), typeof(x86.MulInstruction) },
            { typeof(IL.DivInstruction), typeof(x86.DivInstruction) },
            { typeof(IL.ShiftInstruction), typeof(x86.ShiftInstruction) },

            { typeof(IR.EpilogueInstruction), typeof(x86.EpilogueInstruction) },
            { typeof(IR.LiteralInstruction), typeof(x86.LiteralInstruction) },
            { typeof(IR.LogicalAndInstruction), typeof(x86.LogicalAndInstruction) },
            { typeof(IR.LogicalOrInstruction), typeof(x86.LogicalOrInstruction) },
            { typeof(IR.LogicalXorInstruction), typeof(x86.LogicalXorInstruction) },
            { typeof(IR.LogicalNotInstruction), typeof(x86.LogicalNotInstruction) },
            { typeof(IR.MoveInstruction), typeof(x86.MoveInstruction) },
            { typeof(IR.PrologueInstruction), typeof(x86.PrologueInstruction) },
 */
        };

        /// <summary>
        /// Defines the register set of the target architecture.
        /// </summary>
        private static Register[] _registers = new Register[] {
            ////////////////////////////////////////////////////////
            // 32-bit general purpose registers
            ////////////////////////////////////////////////////////
            GeneralPurposeRegister.EAX,
            GeneralPurposeRegister.EBX,
            GeneralPurposeRegister.ECX,
            GeneralPurposeRegister.EDX,
            GeneralPurposeRegister.ESI,
            GeneralPurposeRegister.EDI,

            ////////////////////////////////////////////////////////
            // 64-bit integral registers
            ////////////////////////////////////////////////////////
            MMXRegister.MM0,
            MMXRegister.MM1,
            MMXRegister.MM2,
            MMXRegister.MM3,
            MMXRegister.MM4,
            MMXRegister.MM5,
            MMXRegister.MM6,
            MMXRegister.MM7,

            ////////////////////////////////////////////////////////
            // 128-bit floating point registers
            ////////////////////////////////////////////////////////
            SSE2Register.XMM0,
            SSE2Register.XMM1,
            SSE2Register.XMM2,
            SSE2Register.XMM3,
            SSE2Register.XMM4,
            SSE2Register.XMM5,
            SSE2Register.XMM6,
            SSE2Register.XMM7
        };

        /// <summary>
        /// Specifies the architecture features to use in generated code.
        /// </summary>
        private ArchitectureFeatureFlags _features;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        protected Architecture(ArchitectureFeatureFlags features)
        {
            _features = features;
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Factory method for the Architecture class.
        /// </summary>
        /// <returns>The created architecture instance.</returns>
        /// <param name="features">The features available in the architecture and code generation.</param>
        /// <remarks>
        /// This method creates an instance of an appropriate architecture class, which supports the specific
        /// architecture features.
        /// </remarks>
        public static IArchitecture CreateArchitecture(ArchitectureFeatureFlags features)
        {
            if (features == ArchitectureFeatureFlags.AutoDetect)
                features = ArchitectureFeatureFlags.MMX | ArchitectureFeatureFlags.SSE;

            return new Architecture(features);
        }

        #endregion // Methods

        #region IArchitecture Members

        /// <summary>
        /// Retrieves the native integer size of the x86 platform.
        /// </summary>
        /// <value>This property always returns 32.</value>
        public override int NativeIntegerSize
        {
            get { return 32; }
        }

        /// <summary>
        /// Retrieves the register set of the x86 platform.
        /// </summary>
        public override Register[] RegisterSet
        {
            get { return _registers; }
        }

        /// <summary>
        /// Retrieves the stack frame register of the x86.
        /// </summary>
        public override Register StackFrameRegister
        {
            get { return GeneralPurposeRegister.EBP; }
        }

        /// <summary>
        /// Creates a new instruction instance.
        /// </summary>
        /// <param name="instructionType">The instruction type to create.</param>
        /// <param name="args">Arguments to pass to the instruction ctor.</param>
        /// <returns>A new instance of the requested instruction or a derived type.</returns>
        public override Instruction CreateInstruction(Type instructionType, params object[] args)
        {
            // Make sure we use x86 specific override classes, if there's one defined.
            Type replType = null;
            if (true == s_irTypes.TryGetValue(instructionType, out replType))
            {
                return (Instruction)Activator.CreateInstance(replType, args, null);
            }
            else
            {
                return base.CreateInstruction(instructionType, args);
            }
        }

        /// <summary>
        /// Extends the assembly compiler pipeline with x86 specific stages.
        /// </summary>
        /// <param name="assemblyPipeline">The assembly compiler pipeline to extend.</param>
        public override void ExtendAssemblyCompilerPipeline(CompilerPipeline<IAssemblyCompilerStage> assemblyPipeline)
        {
        }

        /// <summary>
        /// Extends the method compiler pipeline with x86 specific stages.
        /// </summary>
        /// <param name="methodPipeline">The method compiler pipeline to extend.</param>
        public override void ExtendMethodCompilerPipeline(CompilerPipeline<IMethodCompilerStage> methodPipeline)
        {
            // FIXME: Create a specific code generator instance using requested feature flags.
            // FIXME: Add some more optimization passes, which take advantage of advanced x86 instructions
            // and packed operations available with MMX/SSE extensions
            methodPipeline.AddRange(new IMethodCompilerStage[] {
                new LongOperandTransformationStage(),
                new IRToX86TransformationStage(),
                InstructionLogger.Instance,
                //new SimpleRegisterAllocator(),
                //InstructionLogger.Instance,
                new CodeGenerator()
            });
        }

        /// <summary>
        /// Creates a new result operand of the requested type.
        /// </summary>
        /// <param name="type">The type requested.</param>
        /// <param name="label">The label of the instruction requesting the operand.</param>
        /// <param name="index">The stack index of the operand.</param>
        /// <returns>A new operand usable as a result operand.</returns>
        public override Operand CreateResultOperand(SigType type, int label, int index)
        {
            return new RegisterOperand(type, GeneralPurposeRegister.EAX);
        }

        /// <summary>
        /// Retrieves a calling convention object for the requested calling convention.
        /// </summary>
        /// <param name="cc">One of the defined calling conventions.</param>
        /// <returns>An instance of <see cref="ICallingConvention"/>.</returns>
        /// <exception cref="System.NotSupportedException"><paramref name="cc"/> is not a supported calling convention.</exception>
        public override ICallingConvention GetCallingConvention(CallingConvention cc)
        {
            switch (cc)
            {
                case CallingConvention.Default:
                    return new DefaultCallingConvention(this);

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Requests a <see cref="IRegisterConstraint"/> object for the given instruction.
        /// </summary>
        /// <param name="instruction">The <see cref="Instruction"/> to provide register constraints for.</param>
        /// <returns>An object specifying the register constraints or null, if there are no constraints.</returns>
        public override IRegisterConstraint GetRegisterConstraint(Instruction instruction)
        {
            Type constraintType = null;
            if (s_constraints.TryGetValue(instruction.GetType(), out constraintType) == true)
            {
                return (IRegisterConstraint)Activator.CreateInstance(constraintType);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ObjectFileBuilderBase[] GetObjectFileBuilders()
        {
            return new ObjectFileBuilderBase[] {
                /*new Elf32ObjectFileBuilder(
                    Elf32MachineKind.I386
                )*/
            };
        }

        /// <summary>
        /// Gets the type memory requirements.
        /// </summary>
        /// <param name="type">The signature type.</param>
        /// <param name="size">Receives the memory size of the type.</param>
        /// <param name="alignment">Receives alignment requirements of the type.</param>
        public override void GetTypeRequirements(SigType type, out int size, out int alignment)
        {
            if (null == type)
                throw new ArgumentNullException(@"type");

            switch (type.Type)
            {
                // TODO ROOTNODE
                case CilElementType.R4:
                    size = alignment = 4;
                    break;
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

        #endregion // IArchitecture Members
    }
}
