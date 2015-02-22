/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class InlineStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.Call)
						continue;

					Debug.Assert(node.InvokeMethod != null);

					var invoked = MethodCompiler.Compiler.CompilerData.GetCompilerMethodData(node.InvokeMethod);

					if (!invoked.CanInline)
						continue;

					Inline(node, invoked.BasicBlocks);
				}
			}
		}

		protected void Inline(InstructionNode callNode, BasicBlocks blocks)
		{
			System.Diagnostics.Debug.WriteLine(this.MethodCompiler.Method.FullName);

			var mapBlocks = new Dictionary<BasicBlock, BasicBlock>(blocks.Count);
			var map = new Dictionary<Operand, Operand>();

			// create basic blocks
			foreach (var block in blocks)
			{
				var newBlock = CreateNewBlock();
				mapBlocks.Add(block, newBlock);
			}

			// copy instructions
			foreach (var block in blocks)
			{
				var newBlock = mapBlocks[block];

				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var newNode = new InstructionNode(node.Instruction, node.OperandCount, node.ResultCount);

					if (node.BranchTargets != null)
					{
						// copy targets
						foreach (var target in node.BranchTargets)
						{
							newNode.AddBranchTarget(target);
						}
					}

					// copy results
					for (int i = 0; i < node.ResultCount; i++)
					{
						var op = node.GetResult(i);

						var newOp = Map(op, map, callNode);

						newNode.SetResult(i, newOp);
					}

					// copy operands
					for (int i = 0; i < node.OperandCount; i++)
					{
						var op = node.GetOperand(i);

						var newOp = Map(op, map, callNode);

						newNode.SetOperand(i, newOp);
					}

					// copy other
					if (node.MosaType != null)
						newNode.MosaType = node.MosaType;
					if (node.MosaField != null)
						newNode.MosaField = node.MosaField;
					if (node.InvokeMethod != null)
						newNode.InvokeMethod = node.InvokeMethod;

					newBlock.Last.Previous.Insert(newNode);
				}
			}
		}

		private Operand Map(Operand operand, Dictionary<Operand, Operand> map, InstructionNode callNode)
		{
			if (operand == null)
				return null;

			Operand mappedOperand;

			if (map.TryGetValue(operand, out mappedOperand))
			{
				return mappedOperand;
			}

			if (operand.IsSymbol)
			{
				if (operand.StringData != null)
				{
					mappedOperand = Operand.CreateStringSymbol(operand.Type.TypeSystem, operand.Name, operand.StringData);
				}
				else if (operand.Method != null)
				{
					mappedOperand = Operand.CreateSymbolFromMethod(operand.Type.TypeSystem, operand.Method);
				}
				else if (operand.Name != null)
				{
					mappedOperand = Operand.CreateManagedSymbol(operand.Type, operand.Name);
				}
			}
			else if (operand.IsParameter)
			{
				mappedOperand = callNode.GetOperand(operand.Index);
			}
			else if (operand.IsStackLocal)
			{
				mappedOperand = this.MethodCompiler.StackLayout.AddStackLocal(operand.Type);
			}
			else if (operand.IsVirtualRegister)
			{
				mappedOperand = MethodCompiler.CreateVirtualRegister(operand.Type);
			}
			else if (operand.IsField)
			{
				mappedOperand = operand;
			}
			else if (operand.IsConstant)
			{
				mappedOperand = operand;
			}
			else if (operand.IsCPURegister)
			{
				mappedOperand = operand;
			}

			Debug.Assert(mappedOperand != null);

			map.Add(operand, mappedOperand);

			return mappedOperand;
		}
	}
}
