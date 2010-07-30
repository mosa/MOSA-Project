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
	public sealed class InstructionLogger : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		public static readonly InstructionLogger Instance = new InstructionLogger();

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"InstructionLogger"; } }

		#endregion// IPipelineStage

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			if (MethodCompiler.Method.Name.Contains("<$>"))
				return;

			// Previous stage
			IPipelineStage prevStage = MethodCompiler.GetPreviousStage(typeof(IMethodCompilerStage));

			// Line number
			int index = 1;

			Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", MethodCompiler.Method, prevStage.Name));

            if (this.BasicBlocks.Count > 0)
            {
                foreach (BasicBlock block in BasicBlocks)
                {
                    Debug.WriteLine(String.Format("Block #{0} - label L_{1:X4}", index, block.Label));

                    foreach (BasicBlock prev in block.PreviousBlocks)
                        Debug.WriteLine(String.Format("  Prev: L_{0:X4}", prev.Label));

                    Debug.Indent();
                    LogInstructions(new Context(InstructionSet, block));
                    Debug.Unindent();

                    foreach (BasicBlock next in block.NextBlocks)
                        Debug.WriteLine(String.Format("  Next: L_{0:X4}", next.Label));

                    index++;
                }
            }
            else
            {
                LogInstructions(new Context(InstructionSet, 0));
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
