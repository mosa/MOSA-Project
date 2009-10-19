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
	public sealed class InstructionSchedulingStage : BaseStage, IMethodCompilerStage
	{
		//private static int latencySum = 0;

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get
			{
				return @"InstructionSchedulingStage";
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
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<LinearRegisterAllocator>(this);
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
