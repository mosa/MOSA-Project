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

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"InstructionLogger"; } }

		private PipelineStageOrder[] _pipelineOrder;

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionLogger"/> class.
		/// </summary>
		/// <param name="after">The after.</param>
		/// <param name="before">The before.</param>
		public InstructionLogger(IPipelineStage after, IPipelineStage before)
		{
			_pipelineOrder = PipelineStageOrder.CreatePipelineOrder(after.GetType(), before.GetType());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionLogger"/> class.
		/// </summary>
		/// <param name="after">The after.</param>
		/// <param name="before">The before.</param>
		public InstructionLogger(Type after, Type before)
		{
			_pipelineOrder = PipelineStageOrder.CreatePipelineOrder(after, before);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			// Previous stage
			IPipelineStage prevStage = MethodCompiler.GetPreviousStage(typeof(IMethodCompilerStage));

			// Do not dump internal methods
			if (MethodCompiler.Method.Name.Contains("<$>"))
				return;

			// Line number
			int index = 1;

			Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", MethodCompiler.Method, prevStage.Name));

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
