/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MemToMemConversionStage : BaseTransformationStage, IMethodCompilerStage, IPlatformTransformationStage, IPipelineStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"X86.MemToMemConversionStage"; } }

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(TweakTransformationStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(SimplePeepholeOptimizationStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IBlockOptimizationStage)),				
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IBlockReorderStage)),	
		        new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(CodeGenerationStage))
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			foreach (BasicBlock block in BasicBlocks)
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						if (!ctx.Ignore && ctx.Instruction is CPUx86.IX86Instruction)
							if (ctx.Result is MemoryOperand && ctx.Operand1 is MemoryOperand)
								HandleMemoryToMemoryOperation(ctx, null, false);
		}

		#endregion // IMethodCompilerStage Members

		private void HandleMemoryToMemoryOperation(Context ctx, Operand register, bool useStack)
		{
			Operand destination = ctx.Result;
			Operand source = ctx.Operand1;

			Debug.Assert(destination is MemoryOperand && source is MemoryOperand);

			if (register == null)
				register = new RegisterOperand(destination.Type, GeneralPurposeRegister.EDX);

			ctx.Operand1 = register;

			Context before = ctx.InsertBefore();

			if (useStack) {
				before.SetInstruction(CPUx86.Instruction.PushInstruction, null, register);
				before.AppendInstruction(CPUx86.Instruction.MovInstruction, register, source);
			}
			else
				before.SetInstruction(CPUx86.Instruction.MovInstruction, register, source);

			if (useStack)
				ctx.AppendInstruction(CPUx86.Instruction.PopInstruction, register);
		}
	}
}
