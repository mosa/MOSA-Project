// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Optimization Rules Stage
	/// </summary>
	public sealed class OptimizationRulesStage : BaseMethodCompilerStage
	{
		private Stack<InstructionNode> worklist;

		private TraceLog trace;

		private List<TransformRule> transformRules;

		protected override void Initialize()
		{
			base.Initialize();

			worklist = new Stack<InstructionNode>();
			transformRules = new List<TransformRule>();

			var symbolDictionary = new SymbolDictionary();

			symbolDictionary.Add(Architecture);
			symbolDictionary.Add(IRInstructionList.List);

			foreach (var rule in Rules.List)
			{
				var matchTokens = Tokenizer.Parse(rule.Match, ParseType.Instructions);
				var criteriaTokens = Tokenizer.Parse(rule.Criteria, ParseType.Expression);
				var transformTokens = Tokenizer.Parse(rule.Transform, ParseType.Instructions);

				var matchNodes = NodeParser.Parse(matchTokens, symbolDictionary);
				var criteriaExpression = ExpressionParser.Parse(criteriaTokens);
				var transformNodes = NodeParser.Parse(transformTokens, symbolDictionary);

				var transformRule = new TransformRule(matchNodes, criteriaExpression, transformNodes);

				transformRules.Add(transformRule);
			}
		}

		protected override void Setup()
		{
			base.Setup();

			// TODO
		}

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (!HasCode)
				return;

			trace = CreateTraceLog();

			Optimize();
		}

		protected override void Finish()
		{
			base.Finish();

			//UpdateCounter("IROptimizations.IRInstructionRemoved", instructionsRemovedCount);

			worklist.Clear();
			trace = null;
		}

		private void Optimize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.ResultCount == 0 && node.OperandCount == 0)
						continue;

					Do(node);

					ProcessWorkList();
				}
			}
		}

		private void ProcessWorkList()
		{
			while (worklist.Count != 0)
			{
				var node = worklist.Pop();
				Do(node);
			}
		}

		private void Do(InstructionNode node)
		{
			//foreach (var method in transformations)
			//{
			//	if (node.IsEmpty)
			//		return;

			//	method.Invoke(node);
			//}
		}

		private void AddToWorkList(InstructionNode node)
		{
			if (node.IsEmpty)
				return;

			// work list stays small, so the check is inexpensive
			if (worklist.Contains(node))
				return;

			worklist.Push(node);
		}

		/// <summary>
		/// Adds the operand usage and definitions to work list.
		/// </summary>
		/// <param name="operand">The operand.</param>
		private void AddOperandUsageToWorkList(Operand operand)
		{
			if (!operand.IsVirtualRegister)
				return;

			foreach (var index in operand.Uses)
			{
				AddToWorkList(index);
			}

			foreach (var index in operand.Definitions)
			{
				AddToWorkList(index);
			}
		}

		/// <summary>
		/// Adds the all the operands usage and definitions to work list.
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddOperandUsageToWorkList(InstructionNode node)
		{
			if (node.Result != null)
			{
				AddOperandUsageToWorkList(node.Result);
			}
			if (node.Result2 != null)
			{
				AddOperandUsageToWorkList(node.Result2);
			}
			foreach (var operand in node.Operands)
			{
				AddOperandUsageToWorkList(operand);
			}
		}

		private static BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInt64 : IRInstruction.MoveInt32;
		}

		private static bool ValidateSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}

		private static bool IsPowerOfTwo(ulong n)
		{
			return (n & (n - 1)) == 0;
		}

		private static int GetPowerOfTwo(ulong n)
		{
			int bits = 0;
			while (n > 0)
			{
				bits++;
				n >>= 1;
			}

			return bits - 1;
		}
	}
}
