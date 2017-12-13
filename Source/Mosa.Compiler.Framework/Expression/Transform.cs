// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class Transform
	{
		public ExpressionNode MatchExpression { get; }
		public ExpressionNode TransformationExpression { get; }

		protected Dictionary<string, int> AliasIndex { get; } = new Dictionary<string, int>();
		protected Dictionary<string, int> TypeAliasIndex { get; } = new Dictionary<string, int>();

		public Transform(ExpressionNode match, ExpressionNode transform)
		{
			MatchExpression = match;
			TransformationExpression = transform;

			FindAliases();
		}

		protected void FindAliases()
		{
			AddAlias(MatchExpression.Alias);

			foreach (var node in MatchExpression.ParentNodes)
			{
				AddAlias(node.Alias);
				AddTypeAlias(node.TypeAlias);
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
			return MatchExpression.Validate(node);
		}
	}
}
