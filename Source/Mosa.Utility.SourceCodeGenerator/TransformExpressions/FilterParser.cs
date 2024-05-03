// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

public static class FilterParser
{
	public static List<Method> ParseAll(List<Token> tokens)
	{
		var methods = new List<Method>();

		var length = tokens.Count;
		var index = 0;

		while (index < length)
		{
			var (method, end) = ParseMethod(tokens, index);

			index = end;
			methods.Add(method);
		}

		return methods;
	}

	private static (Method method, int end) ParseMethod(List<Token> tokens, int start)
	{
		var method = new Method();
		var length = tokens.Count;

		// skip ANDs
		while (start != length && tokens[start].TokenType == TokenType.And)
		{
			start++;
		}

		for (var index = start; index < length;)
		{
			var token = tokens[index++];

			if (token.TokenType == TokenType.Not && method.MethodName == null)
			{
				method.IsNegated = true;
			}
			else if (token.TokenType == TokenType.Word && method.MethodName == null)
			{
				method.MethodName = token.Value;
				index++; // skip open param
			}
			else if (token.TokenType == TokenType.Word && method.MethodName != null)
			{
				// peak ahead
				var next = tokens[index];

				if (next.TokenType == TokenType.OpenParens)
				{
					var (innermethod, end) = ParseMethod(tokens, index - 1);

					index = end;

					method.Parameters.Add(new Operand(innermethod, method.Parameters.Count));
				}
				else
				{
					method.Parameters.Add(new Operand(new Token(TokenType.Label, token.Position, token.Value), method.Parameters.Count));
				}
			}
			else if (token.TokenType == TokenType.IntegerConstant)
			{
				method.Parameters.Add(new Operand(token, method.Parameters.Count));
			}
			else if (token.TokenType == TokenType.DoubleConstant)
			{
				method.Parameters.Add(new Operand(token, method.Parameters.Count));
			}
			else if (token.TokenType == TokenType.FloatConstant)
			{
				method.Parameters.Add(new Operand(token, method.Parameters.Count));
			}
			else if (token.TokenType == TokenType.At && method.MethodName != null)
			{
				method.Parameters.Add(new Operand(token, method.Parameters.Count));
			}
			else if (token.TokenType == TokenType.Percent && method.MethodName != null)
			{
				method.Parameters.Add(new Operand(token, method.Parameters.Count));
			}
			else if (token.TokenType == TokenType.Dollar && method.MethodName != null)
			{
				// peak ahead
				var next = tokens[index];

				if (next.TokenType == TokenType.Word)
				{
					method.Parameters.Add(new Operand(new Token(TokenType.Dollar, token.Position, next.Value), method.Parameters.Count));
					index++; // skip word
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}
			else if (token.TokenType == TokenType.Comma)
			{
				// skip
			}
			else if (token.TokenType == TokenType.CloseParens)
			{
				return (method, index);
			}
			else
			{
				throw new Exception($"parsing error {token}");
			}
		}

		throw new Exception($"parsing error unexpected end");
	}
}
