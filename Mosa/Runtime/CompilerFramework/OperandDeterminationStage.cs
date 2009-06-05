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
using Mosa.Runtime.CompilerFramework.IL;

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
		protected IArchitecture arch;
		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> blocks;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock firstBlock;
		/// <summary>
		/// 
		/// </summary>
		protected BitArray workArray;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<BasicBlock> workList;
		/// <summary>
		/// 
		/// </summary>
		protected Stack<List<Operand>> workListStack;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Operand Determination Stage"; }
		}

		#endregion // Properties

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
				throw new InvalidOperationException(@"Operand Determination stage requires basic blocks.");

			blocks = blockProvider.Blocks;

			// Retreive the first block
			firstBlock = blockProvider.FromLabel(-1);

			workList = new Stack<BasicBlock>();
			workListStack = new Stack<List<Operand>>();
			workList.Push(firstBlock);
			workListStack.Push(new List<Operand>());
			workArray = new BitArray(blocks.Count);

			while (workList.Count != 0) {
				BasicBlock block = workList.Pop();
				List<Operand> stack = workListStack.Pop();

				if (!workArray.Get(block.Index)) {
					List<Operand> currentStack = new List<Operand>();

					// Dump Block
					Console.WriteLine();
					Console.WriteLine("Current: " + block.Index.ToString() + ":" + block.Label.ToString() + " - Stack In: " + stack.Count.ToString());
					//					for (int i = 0; i < block.Instructions.Count; i++)
					//						Console.WriteLine(i.ToString() + ": " + block.Instructions[i].ToString());

					// Copy stack (yeah, yeah, slow - hopefully this can be optimized later)
					foreach (Operand operand in stack)
						currentStack.Add(operand);

					for (int i = 0; i < block.Instructions.Count; i++) {
						//foreach (Instruction instruction in block.Instructions) {
						Instruction instruction = block.Instructions[i];

						Console.Write((block.Label + i).ToString("x") + ": " + block.Instructions[i].ToString());

						if (!(instruction is ILInstruction))
							continue;

						Console.Write("  (" + currentStack.Count.ToString() + ":");

						// Assign the operands of the instruction from the IL stack
						for (int opCount = instruction.Operands.Length - 1; opCount >= 0; opCount--)
							if (instruction.Operands[opCount] == null) {
								Operand operand = currentStack[currentStack.Count - 1];
								currentStack.RemoveAt(currentStack.Count - 1);
								instruction.SetOperand(opCount, operand);
							}

						// Validate the instruction
						instruction.Validate(compiler);

						Console.WriteLine(instruction.Operands.Length.ToString() + "/" + ((instruction as ILInstruction).PushResult ? instruction.Results.Length.ToString() : "0") + ")");

						// Push the result operands on the IL stack
						Operand[] ops = instruction.Results;
						if (ops != null && (instruction as ILInstruction).PushResult && ops.Length != 0)
							foreach (Operand operand in ops)
								currentStack.Add(operand);
					}

					workArray.Set(block.Index, true);

					Console.WriteLine(block.Index.ToString() + ":" + block.Label.ToString() + " - Stack Out: " + currentStack.Count.ToString());

					foreach (BasicBlock nextBlock in block.NextBlocks) {
						Console.WriteLine("Next Block: " + nextBlock.Index.ToString() + ":" + nextBlock.Label.ToString());
						if (!workArray.Get(nextBlock.Index)) {
							workList.Push(nextBlock);
							workListStack.Push(currentStack);
						}
					}

					Console.WriteLine();
				}
			}
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
		}

		#endregion // Methods
	}
}
