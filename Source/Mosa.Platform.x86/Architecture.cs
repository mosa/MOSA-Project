// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.Intel;
using Mosa.Platform.x86.CompilerStages;
using Mosa.Platform.x86.Stages;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public sealed class Architecture : BaseArchitecture
	{
		/// <summary>
		/// Gets the endianness of the target architecture.
		/// </summary>
		public override Endianness Endianness { get { return Endianness.Little; } }

		/// <summary>
		/// Gets the type of the elf machine.
		/// </summary>
		public override MachineType ElfMachineType { get { return MachineType.Intel386; } }

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly PhysicalRegister[] registers = new PhysicalRegister[]
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
			// SSE 128-bit floating point registers
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
		private readonly ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		private Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
		}

		/// <summary>
		/// Gets the native size of architecture in bytes.
		/// </summary>
		/// <value>This property always returns 4.</value>
		public override int NativePointerSize { get { return 4; } }

		/// <summary>
		/// Retrieves the register set of the x86 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet { get { return registers; } }

		/// <summary>
		/// Retrieves the stack frame register of the x86.
		/// </summary>
		public override PhysicalRegister StackFrameRegister { get { return GeneralPurposeRegister.EBP; } }

		/// <summary>
		/// Retrieves the stack pointer register of the x86.
		/// </summary>
		public override PhysicalRegister StackPointerRegister { get { return GeneralPurposeRegister.ESP; } }

		/// <summary>
		/// Retrieves the scratch register of the x86.
		/// </summary>
		public override PhysicalRegister ScratchRegister { get { return GeneralPurposeRegister.EDX; } }

		/// <summary>
		/// Gets the return32 bit register.
		/// </summary>
		public override PhysicalRegister ReturnRegister { get { return GeneralPurposeRegister.EAX; } }

		/// <summary>
		/// Gets the return64 bit register.
		/// </summary>
		public override PhysicalRegister ReturnHighRegister { get { return GeneralPurposeRegister.EDX; } }

		/// <summary>
		/// Gets the return floating point register.
		/// </summary>
		public override PhysicalRegister ReturnFloatingPointRegister { get { return SSE2Register.XMM0; } }

		/// <summary>
		/// Retrieves the exception register of the architecture.
		/// </summary>
		public override PhysicalRegister ExceptionRegister { get { return GeneralPurposeRegister.EDI; } }

		/// <summary>
		/// Gets the finally return block register.
		/// </summary>
		public override PhysicalRegister LeaveTargetRegister { get { return GeneralPurposeRegister.ESI; } }

		/// <summary>
		/// Retrieves the program counter register of the x86.
		/// </summary>
		public override PhysicalRegister ProgramCounter { get { return null; } }

		/// <summary>
		/// Gets the offset of first local.
		/// </summary>
		public override int OffsetOfFirstLocal { get { return 0; } }

		/// <summary>
		/// Gets the offset of first parameter.
		/// </summary>
		public override int OffsetOfFirstParameter { get { return 8; } }

		/// <summary>
		/// Gets the name of the platform.
		/// </summary>
		public override string PlatformName { get { return "x86"; } }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public override List<BaseInstruction> Instructions { get { return X86Instructions.List; } }

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		/// <remarks>
		/// This method creates an instance of an appropriate architecture class, which supports the specific
		/// architecture features.
		/// </remarks>
		public static BaseArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Extends the compiler pipeline with x86 compiler stages.
		/// </summary>
		/// <param name="pipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, CompilerOptions compilerOptions)
		{
			if (compilerOptions.MultibootSpecification == MultibootSpecification.V1)
			{
				pipeline.InsertAfterFirst<TypeInitializerStage>(
					new MultibootV1Stage()
				);
			}

			pipeline.Add(new Intel.CompilerStages.StartUpStage());
		}

		/// <summary>
		/// Extends the method compiler pipeline with x86 specific stages.
		/// </summary>
		/// <param name="pipeline">The method compiler pipeline to extend.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, CompilerOptions compilerOptions)
		{
			pipeline.InsertBefore<CallStage>(
				new IRSubstitutionStage()
			);

			pipeline.InsertAfterLast<PlatformIntrinsicStage>(
				new BaseMethodCompilerStage[]
				{
					new LongOperandStage(),
					new IRTransformationStage(),
					compilerOptions.EnablePlatformOptimizations ? new OptimizationStage() : null,
					new TweakStage(),
					new FixedRegisterAssignmentStage(),
					new SimpleDeadCodeRemovalStage(),
					new AddressModeConversionStage(),
					new FloatingPointStage(),
				});

			pipeline.InsertAfterLast<StackLayoutStage>(
				new BuildStackStage()
			);

			pipeline.InsertBefore<CodeGenerationStage>(
				new BaseMethodCompilerStage[]
				{
					new FinalTweakStage(),
					compilerOptions.EnablePlatformOptimizations ? new PostOptimizationStage() : null,
				});

			pipeline.InsertBefore<CodeGenerationStage>(
				new JumpOptimizationStage()
			);
		}

		/// <summary>
		/// Create platform move.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			BaseInstruction instruction = X86.Mov32;

			if (destination.IsR4)
			{
				instruction = X86.Movss;
			}
			else if (destination.IsR8)
			{
				instruction = X86.Movsd;
			}

			context.AppendInstruction(instruction, destination, source);
		}

		/// <summary>
		/// Inserts the store instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
		{
			BaseInstruction instruction = X86.MovStore32;

			if (value.IsR4)
			{
				instruction = X86.MovssStore;
			}
			else if (value.IsR8)
			{
				instruction = X86.MovsdStore;
			}

			context.AppendInstruction(instruction, null, destination, offset, value);
		}

		/// <summary>
		/// Inserts the load instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="offset">The offset.</param>
		public override void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset)
		{
			BaseInstruction instruction = X86.MovLoad32;

			if (destination.IsR4)
			{
				instruction = X86.MovssLoad;
			}
			else if (destination.IsR8)
			{
				instruction = X86.MovsdLoad;
			}

			context.AppendInstruction(instruction, destination, source, offset);
		}

		/// <summary>
		/// Create platform compound move.
		/// </summary>
		/// <param name="methodCompiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="destinationBase">The destination base.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="sourceBase">The source base.</param>
		/// <param name="source">The source.</param>
		/// <param name="size">The size.</param>
		public override void InsertCompoundCopy(MethodCompiler methodCompiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size)
		{
			const int LargeAlignment = 16;
			int alignedSize = size - (size % NativeAlignment);
			int largeAlignedTypeSize = size - (size % LargeAlignment);

			Debug.Assert(size > 0);

			var srcReg = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
			var dstReg = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);

			context.AppendInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(X86.Lea32, srcReg, sourceBase, source);
			context.AppendInstruction(X86.Lea32, dstReg, destinationBase, destination);

			var tmp = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.R8);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large aligned moves allow 128bits to be copied at a time
				var index = methodCompiler.CreateConstant((int)i);
				context.AppendInstruction(X86.MovupsLoad, tmpLarge, srcReg, index);
				context.AppendInstruction(X86.MovupsStore, null, dstReg, index, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedSize; i += 4)
			{
				var index = methodCompiler.CreateConstant(i);
				context.AppendInstruction(X86.MovLoad32, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore32, null, dstReg, index, tmp);
			}
			for (int i = alignedSize; i < size; i++)
			{
				var index = methodCompiler.CreateConstant(i);
				context.AppendInstruction(X86.MovLoad8, tmp, srcReg, index);
				context.AppendInstruction(X86.MovStore8, null, dstReg, index, tmp);
			}

			context.AppendInstruction(IRInstruction.StableObjectTracking);
		}

		/// <summary>
		/// Creates the swap.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			if (source.IsR8 || source.IsR4)
			{
				context.AppendInstruction(X86.PXor, destination, source);
				context.AppendInstruction(X86.PXor, source, destination);
				context.AppendInstruction(X86.PXor, destination, source);
			}
			else
			{
				context.AppendInstruction2(X86.XChg32, destination, source, source, destination);
			}
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			context.AppendInstruction(X86.Jmp, destination);
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			return instruction == X86.Mov32 || instruction == X86.Movsd || instruction == X86.Movss;
		}
	}
}
