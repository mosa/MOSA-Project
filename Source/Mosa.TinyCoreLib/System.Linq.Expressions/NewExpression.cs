using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions;

public class NewExpression : Expression, IArgumentProvider
{
	public ReadOnlyCollection<Expression> Arguments
	{
		get
		{
			throw null;
		}
	}

	public ConstructorInfo? Constructor
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<MemberInfo>? Members
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

	int IArgumentProvider.ArgumentCount
	{
		get
		{
			throw null;
		}
	}

	public override Type Type
	{
		get
		{
			throw null;
		}
	}

	internal NewExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	Expression IArgumentProvider.GetArgument(int index)
	{
		throw null;
	}

	public NewExpression Update(IEnumerable<Expression>? arguments)
	{
		throw null;
	}
}
