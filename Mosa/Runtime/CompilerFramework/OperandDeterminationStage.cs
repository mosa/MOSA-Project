/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Operand Determination Stage determines the operands for each instructions.
	/// </summary>
	public class OperandDeterminationStage : BaseStage, IMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture Architecture;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _firstBlock;
		/// <summary>
		/// 
		/// </summary>
		protected Dictionary<BasicBlock, int> _processed;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<BasicBlock> _unprocessed;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<List<Operand>> _stack;

		#endregion

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Operand Determination Stage"; }
		}

		#endregion

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			_firstBlock = FindBlock(-1);
			_unprocessed = new Stack<BasicBlock>();
			_stack = new Stack<List<Operand>>();
			_processed = new Dictionary<BasicBlock, int>();

			_unprocessed.Push(_firstBlock);
			_stack.Push(new List<Operand>());

			while (_unprocessed.Count != 0) {
				BasicBlock block = _unprocessed.Pop();
				List<Operand> stack = _stack.Pop();

				if (!_processed.ContainsKey(block)) {
					List<Operand> currentStack = GetCurrentStack(stack);

					ProcessInstructions(block, currentStack, compiler);
					_processed.Add(block, 0);

					foreach (BasicBlock nextBlock in block.NextBlocks)
						if (!_processed.ContainsKey(nextBlock)) {
							_unprocessed.Push(nextBlock);
							_stack.Push(currentStack);
						}

				}
			}

			if (_processed.Count != BasicBlocks.Count) {

				foreach (BasicBlock block in BasicBlocks)
					if (!_processed.ContainsKey(block)) {

						if (block.Label == Int32.MaxValue) {
							List<Operand> stack = new List<Operand>();
							ProcessInstructions(block, stack, compiler);
						}
						else
							Console.WriteLine(block);
					}
			}

			//Debug.Assert(_processed.Count == BasicBlocks.Count, @"Did not process all blocks!");

			_unprocessed = null;
			_stack = null;
			_processed = null;
		}

		/// <summary>
		/// Gets the current stack.
		/// </summary>
		/// <param name="stack">The stack.</param>
		/// <returns></returns>
		private static List<Operand> GetCurrentStack(IList<Operand> stack)
		{
			List<Operand> currentStack = new List<Operand>();
			foreach (Operand operand in stack)
				currentStack.Add(operand);
			return currentStack;
		}

		/// <summary>
		/// Processes the instructions.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="currentStack">The current stack.</param>
		/// <param name="compiler">The compiler.</param>
		private void ProcessInstructions(BasicBlock block, IList<Operand> currentStack, IMethodCompiler compiler)
		{
			for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext()) {
				if (!(ctx.Instruction is CIL.ICILInstruction))
					continue;

				AssignOperandsFromCILStack(ctx, currentStack);

				(ctx.Instruction as ICILInstruction).Validate(ctx, compiler);

				PushResultOperands(ctx, currentStack);
			}
		}

		/// <summary>
		/// Assigns the operands from CIL stack.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="currentStack">The current stack.</param>
		private static void AssignOperandsFromCILStack(Context ctx, IList<Operand> currentStack)
		{
			for (int index = ctx.OperandCount - 1; index >= 0; --index) {
				if (ctx.GetOperand(index) == null) {
					Operand operand = currentStack[currentStack.Count - 1];
					currentStack.RemoveAt(currentStack.Count - 1);
					ctx.SetOperand(index, operand);
				}
			}
		}

		/// <summary>
		/// Pushes the result operands on to the stack
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="currentStack">The current stack.</param>
		private static void PushResultOperands(Context ctx, IList<Operand> currentStack)
		{
			if ((ctx.Instruction as ICILInstruction).PushResult)
				foreach (Operand operand in ctx.Results)
					currentStack.Add(operand);
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		void IPipelineStage.SetPipelinePosition(CompilerPipeline<IPipelineStage> pipeline)
		{
			pipeline.RunBefore<IR.CILTransformationStage>(this);
		}

		#endregion // Methods
	}
}
