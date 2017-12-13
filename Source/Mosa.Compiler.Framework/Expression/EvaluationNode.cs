// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class EvaluationNode
	{
		public Token Token { get; protected set; }

		public EvaluationNode Left { get; protected set; }
		public EvaluationNode Right { get; protected set; }

		public List<EvaluationNode> Parameters { get; protected set; }

		public EvaluationNode(Token token)
		{
			Token = token;
		}

		public EvaluationNode(Token token, EvaluationNode left)
		{
			Token = token;
			Left = left;
		}

		public EvaluationNode(Token token, EvaluationNode left, EvaluationNode right)
		{
			Token = token;
			Left = left;
			Right = right;
		}

		public EvaluationNode(Token token, List<EvaluationNode> parameters)
		{
			Token = token;
			Parameters = parameters;
		}

		public override string ToString()
		{
			return Token.ToString();
		}
	}
}
