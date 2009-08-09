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
				throw new InvalidOperationException(@"Operand Determination stage requires basic _blocks.");

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

        private void ProcessInstructions(BasicBlock block, IList<Operand> currentStack, IMethodCompiler compiler)
        {
            for (int i = 0; i < block.Instructions.Count; i++)
            {
                Instruction instruction = block.Instructions[i];

                if (!(instruction is ILInstruction))
                    continue;

                AssignOperandsFromILStack(instruction, currentStack);
                instruction.Validate(compiler);
                PushResultOperands(instruction, currentStack);
            }
        }

        private static void AssignOperandsFromILStack(Instruction instruction, IList<Operand> currentStack)
        {
            for (int opCount = instruction.Operands.Length - 1; opCount >= 0; --opCount)
            {
                if (instruction.Operands[opCount] == null)
                {
                    Operand operand = currentStack[currentStack.Count - 1];
                    currentStack.RemoveAt(currentStack.Count - 1);
                    instruction.SetOperand(opCount, operand);
                }
            }
        }

        private static void PushResultOperands(Instruction instruction, IList<Operand> currentStack)
        {
            Operand[] ops = instruction.Results;
            if (ops != null && (instruction as ILInstruction).PushResult && ops.Length != 0)
            {
                foreach (Operand operand in ops)
                {
                    currentStack.Add(operand);
                }
            }
        }

        private void UpdateWorkList(BasicBlock block, List<Operand> currentStack)
        {
            foreach (BasicBlock nextBlock in block.NextBlocks)
            {
                if (!WorkArray.Get(nextBlock.Index))
                {
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
