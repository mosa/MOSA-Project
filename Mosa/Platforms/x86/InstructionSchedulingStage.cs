/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class InstructionSchedulingStage : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		//private static int latencySum = 0;

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name { get { return @"InstructionSchedulingStage"; } }

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder
		{
			get
			{
				return PipelineStageOrder.CreatePipelineOrder(typeof(IRTransformationStage), typeof(AddressModeConversionStage));
			}
		}

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			foreach (BasicBlock block in BasicBlocks)
				ScheduleBlock(block);
		}

		/// <summary>
		/// Schedules the block.
		/// </summary>
		/// <param name="block">The block.</param>
		private void ScheduleBlock(BasicBlock block)
		{
			// TODO
		}
	}
}
