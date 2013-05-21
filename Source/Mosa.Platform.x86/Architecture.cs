/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Platform.x86.Stages;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public class Architecture : BasicArchitecture
	{
		/// <summary>
		/// Gets a value indicating whether this architecture is little-endian.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is architecture is little-endian; otherwise, <c>false</c>.
		/// </value>
		public override bool IsLittleEndian { get { return true; } }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		/// <value>
		/// The type of the elf machine.
		/// </value>
		public override ushort ElfMachineType { get { return 3; } }

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly Register[] registers = new Register[]
		{
			////////////////////////////////////////////////////////
			// 32-bit general purpose registers
			////////////////////////////////////////////////////////
			GeneralPurposeRegister.EAX,
			GeneralPurposeRegister.ECX,
			GeneralPurposeRegister.EDX,
			GeneralPurposeRegister.EBX,
			GeneralPurposeRegister.ESP,
			GeneralPurposeRegister.EBP,
			GeneralPurposeRegister.ESI,
			GeneralPurposeRegister.EDI,

			////////////////////////////////////////////////////////
			// MMX floating point registers
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
			// SSE 128-bit floating point registers
			////////////////////////////////////////////////////////
			SSE2Register.XMM0,
			SSE2Register.XMM1,
			SSE2Register.XMM2,
			SSE2Register.XMM3,
			SSE2Register.XMM4,
			SSE2Register.XMM5,
			SSE2Register.XMM6,
			SSE2Register.XMM7,

			////////////////////////////////////////////////////////
			// Segmentation Registers
			////////////////////////////////////////////////////////
			SegmentRegister.CS,
			SegmentRegister.DS,
			SegmentRegister.ES,
			SegmentRegister.FS,
			SegmentRegister.GS,
			SegmentRegister.SS
		};

		/// <summary>
		/// Specifies the architecture features to use in generated code.
		/// </summary>
		private ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		private Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
			this.CallingConvention = new DefaultCallingConvention(this);
		}

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
			get { return registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override Register StackFrameRegister
		{
			get { return GeneralPurposeRegister.EBP; }
		}

		/// <summary>
		/// Retrieves the stack pointer register of the x86.
		/// </summary>
		public override Register StackPointerRegister
		{
			get { return GeneralPurposeRegister.ESP; }
		}

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		/// <value>
		/// The name of the platform.
		/// </value>
		public override string PlatformName { get { return "x86"; } }

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		/// <remarks>
		/// This method creates an instance of an appropriate architecture class, which supports the specific
		/// architecture features.
		/// </remarks>
		public static IArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			if (architectureFeatures == ArchitectureFeatureFlags.AutoDetect)
				architectureFeatures = ArchitectureFeatureFlags.MMX | ArchitectureFeatureFlags.SSE | ArchitectureFeatureFlags.SSE2;

			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Extends the compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(CompilerPipeline compilerPipeline)
		{
			compilerPipeline.InsertAfterFirst<ICompilerStage>(
				new InterruptVectorStage()
			);

			compilerPipeline.InsertAfterFirst<InterruptVectorStage>(
				new ExceptionVectorStage()
			);

			compilerPipeline.InsertAfterLast<TypeLayoutStage>(
				new MethodTableBuilderStage()
			);
		}

		/// <summary>
		/// Extends the method compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="methodCompilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline methodCompilerPipeline)
		{
			// FIXME: Create a specific code generator instance using requested feature flags.
			// FIXME: Add some more optimization passes, which take advantage of advanced x86 instructions
			// and packed operations available with MMX/SSE extensions
			methodCompilerPipeline.InsertAfterLast<PlatformStubStage>(
				new IMethodCompilerStage[]
				{
					//new IntrinsicTransformationStage(),
					new LongOperandTransformationStage(),
					new AddressModeConversionStage(),
					new IRTransformationStage(),
					new TweakTransformationStage(),
					new MemToMemConversionStage(),
				});

			methodCompilerPipeline.InsertAfterLast<IBlockOrderStage>(
				new SimplePeepholeOptimizationStage()
			);

			methodCompilerPipeline.InsertAfterLast<CodeGenerationStage>(
				new ExceptionLayoutStage()
			);
		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="signatureType">The signature type.</param>
		/// <param name="memorySize">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public override void GetTypeRequirements(SigType signatureType, out int memorySize, out int alignment)
		{
			if (signatureType == null)
				throw new ArgumentNullException("signatureType");

			switch (signatureType.Type)
			{
				case CilElementType.U1: memorySize = alignment = 4; break;
				case CilElementType.U2: memorySize = alignment = 4; break;
				case CilElementType.U4: memorySize = alignment = 4; break;
				case CilElementType.U8: memorySize = 8; alignment = 4; break;
				case CilElementType.I1: memorySize = alignment = 4; break;
				case CilElementType.I2: memorySize = alignment = 4; break;
				case CilElementType.I4: memorySize = alignment = 4; break;
				case CilElementType.I8: memorySize = 8; alignment = 4; break;
				case CilElementType.R4: memorySize = alignment = 4; break;
				case CilElementType.R8: memorySize = alignment = 8; break;
				case CilElementType.Boolean: memorySize = alignment = 4; break;
				case CilElementType.Char: memorySize = alignment = 4; break;

				// Platform specific
				case CilElementType.Ptr: memorySize = alignment = 4; break;
				case CilElementType.I: memorySize = alignment = 4; break;
				case CilElementType.U: memorySize = alignment = 4; break;
				case CilElementType.Object: memorySize = alignment = 4; break;
				case CilElementType.Class: memorySize = alignment = 4; break;
				case CilElementType.String: memorySize = alignment = 4; break;

				default: memorySize = alignment = 4; break;
			}
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override ICodeEmitter GetCodeEmitter()
		{
			return new MachineCodeEmitter();
		}
	}
}