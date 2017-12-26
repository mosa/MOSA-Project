// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	public delegate Value Evaluation(ExpressionNode node);

	public class ExpressionEvaluator
	{
		protected ExpressionNode Root { get; }

		//protected static IMethodSource BuiltInMethods = new BuiltInMethods();

		public ExpressionEvaluator(ExpressionNode root)
		{
			Root = root;
		}

		public Value Evaluate()
		{
			if (Root == null)
				throw new CompilerException("invalid parameter to parser");

			var result = Evaluate(Root);

			return result;
		}

		protected static Value Evaluate(ExpressionNode root)
		{
			Debug.Assert(root != null);

			if (root.Left != null && root.Right != null)
			{
				var left = Evaluate(root.Left);

				// shortcut evaluation
				if (root.Token.TokenType == TokenType.And && left.IsFalse)
				{
					return left;
				}
				else if (root.Token.TokenType == TokenType.Or && left.IsTrue)
				{
					return left;
				}

				var right = Evaluate(root.Right);

				return Eval(root.Token.TokenType, left, right);
			}
			else if (root.Left != null && root.Right == null)
			{
				var left = Evaluate(root.Left);

				return Eval(root.Token.TokenType, left);
			}
			else if (root.Left == null && root.Right == null)
			{
				if (root.Token.TokenType == TokenType.If)
				{
					return IfStatement(root);
				}
				else if (root.Token.TokenType == TokenType.OperandVariable)
				{
					return OperandVariable(root);
				}
				else if (root.Token.TokenType == TokenType.Method)
				{
					return Method(root);
				}
				else
				{
					// must be a literal
					return EvalLiteral(root.Token);
				}
			}

			throw new CompilerException("unexpected token type: " + root.Token);
		}

		protected static Value OperandVariable(ExpressionNode node)
		{
			var operandName = node.Token.Value;

			// TODO

			return null;
		}

		protected static Value EvalLiteral(Token token)
		{
			switch (token.TokenType)
			{
				case TokenType.SignedIntegerConstant: return new Value((long)token.Integer);
				case TokenType.UnsignedIntegerConstant: return new Value((ulong)token.Integer);
				case TokenType.DoubleConstant: return new Value(token.Double);
				default: break;
			}

			throw new CompilerException("invalid token type: " + token);
		}

		protected static Value Eval(TokenType tokenType, Value left, Value right)
		{
			switch (tokenType)
			{
				case TokenType.Addition: return Addition(left, right);
				case TokenType.Subtract: return Subtract(left, right);
				case TokenType.Multiplication: return Multiplication(left, right);
				case TokenType.Division: return Division(left, right);
				case TokenType.And: return And(left, right);
				case TokenType.Or: return Or(left, right);
				case TokenType.CompareEqual: return CompareEqual(left, right);
				case TokenType.CompareNotEqual: return CompareNotEqual(left, right);
				case TokenType.CompareGreaterThanOrEqual: return CompareGreaterThanOrEqual(left, right);
				case TokenType.CompareLessThanOrEqual: return CompareLessThanOrEqual(left, right);
				case TokenType.CompareLessThan: return CompareLessThan(left, right);
				case TokenType.CompareGreaterThan: return CompareGreaterThan(left, right);
				default: break;
			}

			throw new CompilerException("invalid token type: " + tokenType.ToString());
		}

		protected static Value Eval(TokenType tokenType, Value left)
		{
			switch (tokenType)
			{
				case TokenType.Not: return Not(left);
				case TokenType.Negate: return Negate(left);
				default: break;
			}

			throw new CompilerException("invalid token type: " + tokenType.ToString());
		}

		protected static Value Not(Value left)
		{
			if (left.IsUnsignedInteger)
			{
				return new Value(~left.UnsignedInteger);
			}
			else if (left.IsSignInteger)
			{
				return new Value(~left.SignedInteger);
			}

			throw new CompilerException("incompatible type for not operator: " + left);
		}

		protected static Value Negate(Value left)
		{
			if (left.IsInteger)
			{
				return new Value(-left.SignedInteger);
			}
			else if (left.IsDouble)
			{
				return new Value(-left.Double);
			}

			throw new CompilerException("incompatible type for negate operator: " + left);
		}

		protected static Value Addition(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double + right.Double);
			}
			else if (left.IsDouble && right.IsUnsignedInteger)
			{
				return new Value(left.Double + right.UnsignedInteger);
			}
			else if (left.IsDouble && right.IsSignInteger)
			{
				return new Value(left.Double + right.SignedInteger);
			}
			else if (left.IsUnsignedInteger && right.IsDouble)
			{
				return new Value(left.UnsignedInteger + right.Double);
			}
			else if (left.IsSignInteger && right.IsDouble)
			{
				return new Value(left.SignedInteger + right.Double);
			}
			else if (left.IsSignInteger || right.IsSignInteger)
			{
				return new Value((long)left.SignedInteger + right.SignedInteger);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value((ulong)left.UnsignedInteger + right.UnsignedInteger);
			}

			throw new CompilerException("incompatible types for addition operator: " + left + " and " + right);
		}

		protected static Value Subtract(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double - right.Double);
			}
			else if (left.IsDouble && right.IsUnsignedInteger)
			{
				return new Value(left.Double - right.UnsignedInteger);
			}
			else if (left.IsDouble && right.IsSignInteger)
			{
				return new Value(left.Double - right.SignedInteger);
			}
			else if (left.IsUnsignedInteger && right.IsDouble)
			{
				return new Value(left.UnsignedInteger - right.Double);
			}
			else if (left.IsSignInteger && right.IsDouble)
			{
				return new Value(left.SignedInteger - right.Double);
			}
			else if (left.IsSignInteger || right.IsSignInteger)
			{
				return new Value((long)left.SignedInteger - right.SignedInteger);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value((ulong)left.UnsignedInteger - right.UnsignedInteger);
			}

			throw new CompilerException("incompatible types for substraction operator: " + left + " and " + right);
		}

		protected static Value Multiplication(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double * right.Double);
			}
			else if (left.IsDouble && right.IsUnsignedInteger)
			{
				return new Value(left.Double * right.UnsignedInteger);
			}
			else if (left.IsDouble && right.IsSignInteger)
			{
				return new Value(left.Double * right.SignedInteger);
			}
			else if (left.IsUnsignedInteger && right.IsDouble)
			{
				return new Value(left.UnsignedInteger * right.Double);
			}
			else if (left.IsSignInteger && right.IsDouble)
			{
				return new Value(left.SignedInteger * right.Double);
			}
			else if (left.IsSignInteger || right.IsSignInteger)
			{
				return new Value((long)left.SignedInteger * right.SignedInteger);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value((ulong)left.UnsignedInteger * right.UnsignedInteger);
			}

			throw new CompilerException("incompatible types for multiplication operator: " + left + " and " + right);
		}

		protected static Value Division(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double / right.Double);
			}
			else if (left.IsDouble && right.IsUnsignedInteger)
			{
				return new Value(left.Double / right.UnsignedInteger);
			}
			else if (left.IsDouble && right.IsSignInteger)
			{
				return new Value(left.Double / right.SignedInteger);
			}
			else if (left.IsUnsignedInteger && right.IsDouble)
			{
				return new Value(left.UnsignedInteger / right.Double);
			}
			else if (left.IsSignInteger && right.IsDouble)
			{
				return new Value(left.SignedInteger / right.Double);
			}
			else if (left.IsSignInteger || right.IsSignInteger)
			{
				return new Value((long)left.SignedInteger / right.SignedInteger);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value((ulong)left.UnsignedInteger / right.UnsignedInteger);
			}

			throw new CompilerException("incompatible types for division operator: " + left + " and " + right);
		}

		protected static Value And(Value left, Value right)
		{
			if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.IsTrue && right.IsTrue);
			}

			throw new CompilerException("incompatible types for and operator: " + left + " and " + right);
		}

		protected static Value Or(Value left, Value right)
		{
			if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.IsTrue || right.IsTrue);
			}

			throw new CompilerException("incompatible types for or operator operator: " + left + " and " + right);
		}

		protected static Value CompareEqual(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double == right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger == right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value CompareNotEqual(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double != right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger != right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value CompareGreaterThanOrEqual(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double >= right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger >= right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value CompareLessThanOrEqual(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double <= right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger <= right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value CompareLessThan(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double < right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger < right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value CompareGreaterThan(Value left, Value right)
		{
			if (left.IsDouble && right.IsDouble)
			{
				return new Value(left.Double > right.Double);
			}
			else if (left.IsInteger && right.IsInteger)
			{
				return new Value(left.SignedInteger > right.SignedInteger);
			}

			throw new CompilerException("incompatible types for comparison operator: " + left + " and " + right);
		}

		protected static Value IfStatement(ExpressionNode node)
		{
			if (node.Parameters.Count < 2 || node.Parameters.Count > 3)
				throw new CompilerException("Incomplete if statement");

			var condition = Evaluate(node.Parameters[0]);

			return Evaluate(node.Parameters[condition.IsTrue ? 1 : 2]);
		}

		protected static Value Method(ExpressionNode node)
		{
			string name = node.Token.Value;

			//result = BuiltInMethods.Evaluate(name, node.Parameters);

			//if (result != null)
			//	return result;

			throw new CompilerException("unknown method: " + name);
		}

		public override string ToString()
		{
			return Root.ToString();
		}

		public static Value ValidateHelper(string method, IList<Value> parameters, int minParameters, IList<ValueType> types)
		{
			if (parameters.Count < minParameters)
			{
				throw new CompilerException(method + "() error too few parameters");
			}
			if (parameters.Count > types.Count)
			{
				throw new CompilerException(method + "() too many parameters");
			}

			for (int i = 0; i < types.Count; i++)
			{
				if (i >= parameters.Count)
					return null;

				var type = types[i];
				var parameter = parameters[i];

				if (parameter.ValueType != type)
				{
					var typename = Enum.GetName(typeof(ValueType), type);

					throw new CompilerException(method + "() parameter #" + i.ToString() + " is not of the expected type: " + typename);
				}
			}

			return null;
		}
	}
}
