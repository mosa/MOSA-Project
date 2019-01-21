// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.Intel;
using Mosa.Platform.x64.CompilerStages;
using Mosa.Platform.x64.Stages;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Platform.x64
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
		private static readonly PhysicalRegister[] Registers = new PhysicalRegister[]
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

			GeneralPurposeRegister.R8,
			GeneralPurposeRegister.R9,
			GeneralPurposeRegister.R10,
			GeneralPurposeRegister.R11,
			GeneralPurposeRegister.R12,
			GeneralPurposeRegister.R13,
			GeneralPurposeRegister.R14,
			GeneralPurposeRegister.R15,

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

			SSE2Register.XMM8,
			SSE2Register.XMM9,
			SSE2Register.XMM10,
			SSE2Register.XMM11,
			SSE2Register.XMM12,
			SSE2Register.XMM13,
			SSE2Register.XMM14,
			SSE2Register.XMM15
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
		/// <value>This property always returns 8.</value>
		public override int NativePointerSize { get { return 8; } }

		/// <summary>
		/// Retrieves the register set of the x64 platform.
		/// </summary>
		public override PhysicalRegister[] RegisterSet { get { return Registers; } }

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
		/// Gets the return register.
		/// </summary>
		public override PhysicalRegister ReturnRegister { get { return GeneralPurposeRegister.EAX; } }

		/// <summary>
		/// Gets the return register for the high portion of the 64bit result.
		/// </summary>
		public override PhysicalRegister ReturnHighRegister { get { return null; } }

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
		/// Gets the name of the platform.
		/// </summary>
		public override string PlatformName { get { return "x64"; } }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		public override List<BaseInstruction> Instructions { get { return X64Instructions.List; } }

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		public static BaseArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Extends the compiler pipeline with x64 specific stages.
		/// </summary>
		/// <param name="compilerPipeline">The pipeline to extend.</param>
		public override void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> compilerPipeline, CompilerOptions compilerOptions)
		{
			if (compilerOptions.MultibootSpecification == MultibootSpecification.V1)
			{
				compilerPipeline.InsertAfterFirst<TypeInitializerSchedulerStage>(
					new MultibootV1Stage()
				);
			}

			compilerPipeline.Add(
				new Intel.CompilerStages.StartUpStage()
			);
		}

		/// <summary>
		/// Extends the method compiler pipeline with x64 specific stages.</summary>
		/// <param name="compilerPipeline">The method compiler pipeline to extend.</param>
		/// <param name="compilerOptions"></param>
		public override void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> compilerPipeline, CompilerOptions compilerOptions)
		{
			compilerPipeline.InsertBefore<LowerIRStage>(
				new IRSubstitutionStage()
			);

			compilerPipeline.InsertAfterLast<PlatformIntrinsicStage>(
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

			compilerPipeline.InsertAfterLast<StackLayoutStage>(
				new BuildStackStage()
			);

			compilerPipeline.InsertBefore<CodeGenerationStage>(
				new BaseMethodCompilerStage[]
				{
					new FinalTweakStage(),
					compilerOptions.EnablePlatformOptimizations ? new PostOptimizationStage() : null,
				});

			compilerPipeline.InsertBefore<CodeGenerationStage>(
				new JumpOptimizationStage()
			);
		}

		/// <summary>
		/// Inserts the move instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertMoveInstruction(Context context, Operand destination, Operand source)
		{
			BaseInstruction instruction = X64.Mov64;

			if (destination.IsR4)
			{
				instruction = X64.Movss;
			}
			else if (destination.IsR8)
			{
				instruction = X64.Movsd;
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
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value)
		{
			BaseInstruction instruction = X64.MovStore32;

			if (value.IsR4)
			{
				instruction = X64.MovssStore;
			}
			else if (value.IsR8)
			{
				instruction = X64.MovsdStore;
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
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset)
		{
			BaseInstruction instruction = X64.MovLoad32;

			if (destination.IsR4)
			{
				instruction = X64.MovssLoad;
			}
			else if (destination.IsR8)
			{
				instruction = X64.MovsdLoad;
			}

			context.AppendInstruction(instruction, destination, source, offset);
		}

		/// <summary>
		/// Create platform compound move.
		/// </summary>
		/// <param name="methodCompiler">The compiler.</param>
		/// <param name="context">The context.</param>
		/// <param name="destinationBase">The destination.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="sourceBase">The source.</param>
		/// <param name="source">The source.</param>
		/// <param name="size">The size.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void InsertCompoundCopy(MethodCompiler methodCompiler, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, int size)
		{
			const int LargeAlignment = 16;
			int alignedSize = size - (size % NativeAlignment);
			int largeAlignedTypeSize = size - (size % LargeAlignment);

			Debug.Assert(size > 0);

			var srcReg = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
			var dstReg = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);

			context.AppendInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(X64.Lea64, srcReg, sourceBase, source);
			context.AppendInstruction(X64.Lea64, dstReg, destinationBase, destination);

			var tmp = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = methodCompiler.CreateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.R8);

			for (int i = 0; i < largeAlignedTypeSize; i += LargeAlignment)
			{
				// Large aligned moves allow 128bits to be copied at a time
				var index = methodCompiler.CreateConstant((int)i);
				context.AppendInstruction(X64.MovupsLoad, tmpLarge, srcReg, index);
				context.AppendInstruction(X64.MovupsStore, null, dstReg, index, tmpLarge);
			}
			for (int i = largeAlignedTypeSize; i < alignedSize; i += 8)
			{
				var index = methodCompiler.CreateConstant(i);
				context.AppendInstruction(X64.MovLoad64, tmp, srcReg, index);
				context.AppendInstruction(X64.MovStore64, null, dstReg, index, tmp);
			}
			for (int i = alignedSize; i < size; i++)
			{
				var index = methodCompiler.CreateConstant(i);
				context.AppendInstruction(X64.MovLoad8, tmp, srcReg, index);
				context.AppendInstruction(X64.MovStore8, null, dstReg, index, tmp);
			}

			context.AppendInstruction(IRInstruction.StableObjectTracking);
		}

		/// <summary>
		/// Inserts the exchange instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		public override void InsertExchangeInstruction(Context context, Operand destination, Operand source)
		{
			if (source.IsR4)
			{
				// TODO
				throw new CompilerException("R4 not implemented in InsertExchangeInstruction method");
			}
			else if (source.IsR8)
			{
				// TODO
				throw new CompilerException("R8 not implemented in InsertExchangeInstruction method");
			}
			else
			{
				context.AppendInstruction2(X64.XChg64, destination, source, source, destination);
			}
		}

		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="destination">The destination.</param>
		public override void InsertJumpInstruction(Context context, BasicBlock destination)
		{
			context.AppendInstruction(X64.Jmp, destination);
		}

		/// <summary>
		/// Determines whether [is instruction move] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
		public override bool IsInstructionMove(BaseInstruction instruction)
		{
			return instruction == X64.Mov64 || instruction == X64.Mov32 || instruction == X64.Movsd || instruction == X64.Movss;
		}
	}
}
