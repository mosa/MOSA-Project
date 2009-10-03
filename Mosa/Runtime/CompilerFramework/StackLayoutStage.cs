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
using System.Diagnostics;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Calculates the layout of the stack of the method.
	/// </summary>
	public sealed class StackLayoutStage : BaseStage, IMethodCompilerStage, IStackLayoutProvider
	{
		#region Tracing

		/// <summary>
		/// Controls the tracing of the <see cref="StackLayoutStage"/>.
		/// </summary>
		/// <remarks>
		/// The stack layout tracing is done with the TraceLevel.Info. Set the TraceSwitch to this value
		/// to receive full stack layout tracing.
		/// </remarks>
		private static readonly TraceSwitch TRACING = new TraceSwitch(@"Mosa.Runtime.CompilerFramework.StackLayoutStage", @"Controls tracing of the Mosa.Runtime.CompilerFramework.StackLayoutStage method compiler stage.");

		#endregion // Tracing

		#region Data members

		/// <summary>
		/// Holds the total stack requirements of local variables of the compiled method.
		/// </summary>
		private int _localsSize;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"StackLayoutStage"; }
		}

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Allocate a list of locals
			List<StackOperand> locals = new List<StackOperand>();

			// Architecture
			IArchitecture arch = compiler.Architecture;

			// Retrieve the calling convention of the method
			ICallingConvention cc = compiler.Architecture.GetCallingConvention(compiler.Method.Signature.CallingConvention);
			Debug.Assert(null != cc, @"Failed to retrieve the calling convention of the method.");

			// Iterate all Blocks and collect locals From all Blocks
			foreach (BasicBlock block in BasicBlocks)
				CollectLocalVariables(locals, block);

			// Sort all found locals
			OrderVariables(locals, cc);

			// Now we assign increasing stack offsets to each variable
			_localsSize = LayoutVariables(locals, cc, cc.OffsetOfFirstLocal, 1);
			if (TRACING.TraceInfo == true) {
				Trace.WriteLine(String.Format(@"Stack layout for method {0}", compiler.Method));
				LogOperands(locals);
			}

			// Layout parameters
			LayoutParameters(compiler, cc);

			// Create a prologue instruction
			Context prologueCtx = new Context(InstructionSet, BlockProvider.FromLabel(-1));
			prologueCtx.InsertInstructionAfter(IR.Instruction.PrologueInstruction);
			prologueCtx.Other = _localsSize;

			// Create an epilogue instruction
			Context epilogueCtx = new Context(InstructionSet, BlockProvider.FromLabel(Int32.MaxValue));
			epilogueCtx.GotoLast();
			epilogueCtx.InsertInstructionAfter(IR.Instruction.EpilogueInstruction);
			epilogueCtx.Other = _localsSize;
		}

		/// <summary>
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<LeaveSSA>(this);
		}

		#endregion // IMethodCompilerStage Members

		#region IStackLayoutStage Members

		int IStackLayoutProvider.LocalsSize
		{
			get { return _localsSize; }
		}

		#endregion // IStackLayoutStage Members

		#region Internals

		/// <summary>
		/// Collects all local variables assignments into a list.
		/// </summary>
		/// <param name="locals">Holds all locals found by the stage.</param>
		/// <param name="block">The block.</param>
		private void CollectLocalVariables(List<StackOperand> locals, BasicBlock block)
		{
			for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext()) {
				// Does this instruction define a new stack variable?
				foreach (Operand op in ctx.Results) {
					// The instruction list may not be in SSA form, so we have to check existence again here unfortunately.
					// FIXME: Allow us to detect the state of Blocks
					LocalVariableOperand lvop = op as LocalVariableOperand;
					if (lvop != null && !locals.Contains(lvop))
						locals.Add(lvop);
				}
			}
		}

		/// <summary>
		/// Lays out all parameters of the method.
		/// </summary>
		/// <param name="compiler">The method compiler providing the parameters.</param>
		/// <param name="cc">The calling convention used to invoke the method, which controls parameter layout.</param>
		private void LayoutParameters(IMethodCompiler compiler, ICallingConvention cc)
		{
			List<StackOperand> paramOps = new List<StackOperand>();

			for (int i = 0; i < compiler.Method.Parameters.Count; i++)
				paramOps.Add((StackOperand)compiler.GetParameterOperand(i));

			LayoutVariables(paramOps, cc, cc.OffsetOfFirstParameter, -1);

			if (TRACING.TraceInfo)
				LogOperands(paramOps);
		}

		/// <summary>
		/// Performs a stack layout of all local variables in the list.
		/// </summary>
		/// <param name="locals">The enumerable holding all locals.</param>
		/// <param name="cc">The cc.</param>
		/// <param name="offsetOfFirst">Specifies the offset of the first stack operand in the list.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		private static int LayoutVariables(IEnumerable<StackOperand> locals, ICallingConvention cc, int offsetOfFirst, int direction)
		{
			int offset = offsetOfFirst;

			foreach (StackOperand lvo in locals) {
				// Does the offset fit the alignment requirement?
				int alignment;
				int size;
				int padding;
				int thisOffset;

				cc.GetStackRequirements(lvo, out size, out alignment);
				if (1 == direction) {
					padding = (offset % alignment);
					offset -= (padding + size);
					thisOffset = offset;
				}
				else {
					padding = (offset % alignment);
					if (0 != padding)
						padding = alignment - padding;

					thisOffset = offset;
					offset += (padding + size);
				}

				lvo.Offset = new IntPtr(thisOffset);
			}

			return offset;
		}

		/// <summary>
		/// Logs all operands in <paramref name="locals"/>.
		/// </summary>
		/// <param name="locals">The operands to log.</param>
		private void LogOperands(List<StackOperand> locals)
		{
			foreach (StackOperand local in locals)
				Trace.WriteLine(String.Format(@"\t{0} at {1}", local, local.Offset));
		}

		/// <summary>
		/// Sorts all local variables by their size requirements.
		/// </summary>
		/// <param name="locals">Holds all local variables to sort..</param>
		/// <param name="cc">The calling convention used to determine size and alignment requirements.</param>
		private static void OrderVariables(List<StackOperand> locals, ICallingConvention cc)
		{
			/* Sort the list by stack size requirements - this moves equally sized operands closer together,
			 * in the hope that this reduces padding on the stack to enforce HW alignment requirements.
			 */
			locals.Sort(delegate(StackOperand op1, StackOperand op2)
			{
				int size1, size2, alignment;
				cc.GetStackRequirements(op1, out size1, out alignment);
				cc.GetStackRequirements(op2, out size2, out alignment);
				return size2 - size1;
			});
		}

		#endregion // Internals
	}
}
