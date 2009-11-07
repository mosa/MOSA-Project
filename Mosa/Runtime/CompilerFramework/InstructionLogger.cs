/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Logs all incoming instructions and forwards them to the next compiler stage.
	/// </summary>
	public sealed class InstructionLogger : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// Static instance of the instruction logger.
		/// </summary>
		public static readonly InstructionLogger Instance = new InstructionLogger();

		#endregion // Data members

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"InstructionLogger"; }
		}

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder
		{
			get
			{
				return new PipelineStageOrder[] {
					new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IMethodCompilerStage)),
					//new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IR.CILTransformationStage))
				};
			}
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Previous stage
			IPipelineStage prevStage = compiler.GetPreviousStage(typeof(IMethodCompilerStage));

			// Do not dump internal methods
			if (compiler.Method.Name.Contains("<$>"))
				return;

			// Line number
			int index = 1;

			Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", compiler.Method, prevStage.Name));

			foreach (BasicBlock block in BasicBlocks) {
				Debug.WriteLine(String.Format("Block #{0} - label L_{1:X4}", index, block.Label));
				Debug.Indent();
				LogInstructions(new Context(InstructionSet, block));
				Debug.Unindent();
				index++;
			}
		}

		#endregion // IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Logs the instructions in the given enumerable to the trace.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void LogInstructions(Context ctx)
		{
			StringBuilder text = new StringBuilder();

			for (; !ctx.EndOfInstruction; ctx.GotoNext()) {

				text.Length = 0;

				if (ctx.Instruction == null)
					continue;

				if (ctx.Ignore)
					text.Append("; ");

				text.AppendFormat("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx));

				Debug.WriteLine(text.ToString());
			}
		}

		#endregion // Internals
	}
}
