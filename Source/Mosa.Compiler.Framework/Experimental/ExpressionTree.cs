// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Experimental
{
	public class ExpressionTree
	{
		public ExpressionNode Root { get; }
		public Dictionary<string, int> AliasIndex { get; } = new Dictionary<string, int>();

		public ExpressionTree(ExpressionNode root)
		{
			Root = root;
			FindAliases();
		}

		protected void FindAliases()
		{
			AddAlias(Root.Alias);

			foreach (var node in Root.ParentNodes)
			{
				AddAlias(node.Alias);
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

		public int GetAliasIndex(string alias)
		{
			if (AliasIndex.TryGetValue(alias, out int value))
				return value;

			return -1;
		}
	}
}
