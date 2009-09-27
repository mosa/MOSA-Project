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
using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Operand Determination Stage determines the operands for each instructions.
	/// </summary>
	public class OperandDeterminationStage : IMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture Architecture;
		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> _blocks;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _firstBlock;
		/// <summary>
		/// 
		/// </summary>
		protected BitArray WorkArray;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<BasicBlock> WorkList;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<List<Operand>> WorkListStack;

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
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			if (blockProvider == null)
				throw new InvalidOperationException(@"Operand Determination stage requires basic Blocks.");

			_blocks = blockProvider.Blocks;
			_firstBlock = blockProvider.FromLabel(-1);

			InitializeWorkItems();

			while (WorkList.Count != 0) {
				BasicBlock block = WorkList.Pop();
				List<Operand> stack = WorkListStack.Pop();

				if (!WorkArray.Get(block.Index)) {
					List<Operand> currentStack = GetCurrentStack(stack);

					ProcessInstructions(block, currentStack, compiler);
					WorkArray.Set(block.Index, true);
					UpdateWorkList(block, currentStack);
				}
			}
		}

		private void InitializeWorkItems()
		{
			WorkList = new Stack<BasicBlock>();
			WorkListStack = new Stack<List<Operand>>();
			WorkList.Push(_firstBlock);
			WorkListStack.Push(new List<Operand>());
			WorkArray = new BitArray(_blocks.Count);
		}

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
			Context ctx = new Context(block);

			while (!ctx.EndOfInstruction) {
				if (!(ctx.Instruction is CIL.ICILInstruction))
					continue;

				AssignOperandsFromCILStack(ctx, currentStack);

				(ctx.Instruction as ICILInstruction).Validate(ctx, compiler);
				//instruction.Validate(compiler);

				PushResultOperands(ctx, currentStack);

				ctx.GotoNext();
			}
		}

		private static void AssignOperandsFromCILStack(Context ctx, IList<Operand> currentStack)
		{
			for (int index = ctx.OperandCount - 1; index >= 0; --index) {
				Operand operand = ctx.GetOperand(index);
				if (operand == null) {
					currentStack.RemoveAt(currentStack.Count - 1);
					ctx.SetOperand(index, operand);
				}
			}
		}

		private static void PushResultOperands(Context ctx, IList<Operand> currentStack)
		{
			if ((ctx.Instruction as ICILInstruction).PushResult)
				foreach (Operand operand in ctx.Operands) 
					currentStack.Add(operand);
		}

		private void UpdateWorkList(BasicBlock block, List<Operand> currentStack)
		{
			foreach (BasicBlock nextBlock in block.NextBlocks) {
				if (!WorkArray.Get(nextBlock.Index)) {
					WorkList.Push(nextBlock);
					WorkListStack.Push(currentStack);
				}
			}
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CilToIrTransformationStage>(this);
		}

		#endregion // Methods
	}
}
