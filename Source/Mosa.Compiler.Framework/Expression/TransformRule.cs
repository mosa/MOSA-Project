// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class TransformRule
	{
		public Node InstructionMatchExpression { get; }
		public Node CriteriaExpression { get; }
		public Node TransformationExpression { get; }

		protected Dictionary<string, int> AliasIndex { get; } = new Dictionary<string, int>();
		protected Dictionary<string, int> TypeAliasIndex { get; } = new Dictionary<string, int>();

		public TransformRule(Node instructionMatch, Node transform)
		{
			InstructionMatchExpression = instructionMatch;
			TransformationExpression = transform;

			FindNames();
		}

		protected void FindNames()
		{
			AddAlias(InstructionMatchExpression.Name);

			foreach (var node in InstructionMatchExpression.ParentNodes)
			{
				if (node.NodeType == NodeType.ConstantVariable || node.NodeType == NodeType.OperandVariable)
					AddAlias(node.Name);
				else if (node.NodeType == NodeType.TypeVariable)
					AddTypeAlias(node.Name);
			}
		}

		protected void AddAlias(string alias)
		{
			if (alias == null)
				return;

			if (AliasIndex.ContainsKey(alias))
				return;

			AliasIndex.Add(alias, AliasIndex.Count);
		}

		protected void AddTypeAlias(string alias)
		{
			if (alias == null)
				return;

			if (TypeAliasIndex.ContainsKey(alias))
				return;

			TypeAliasIndex.Add(alias, TypeAliasIndex.Count);
		}

		public int GetAliasIndex(string alias)
		{
			if (AliasIndex.TryGetValue(alias, out int value))
				return value;

			return -1;
		}

		public int GetTypeAliasIndex(string alias)
		{
			if (TypeAliasIndex.TryGetValue(alias, out int value))
				return value;

			return -1;
		}

		public bool Validate(InstructionNode node)
		{
			return InstructionMatchExpression.Validate(node);
		}
	}
}
