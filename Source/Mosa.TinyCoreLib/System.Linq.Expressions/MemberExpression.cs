using System.Reflection;

namespace System.Linq.Expressions;

public class MemberExpression : Expression
{
	public Expression? Expression
	{
		get
		{
			throw null;
		}
	}

	public MemberInfo Member
	{
		get
		{
			throw null;
		}
	}

	public sealed override ExpressionType NodeType
	{
		get
		{
			throw null;
		}
	}

	internal MemberExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public MemberExpression Update(Expression? expression)
	{
		throw null;
	}
}
