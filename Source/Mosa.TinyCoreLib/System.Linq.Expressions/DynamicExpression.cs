using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions;

public class DynamicExpression : Expression, IArgumentProvider, IDynamicExpression
{
	public ReadOnlyCollection<Expression> Arguments
	{
		get
		{
			throw null;
		}
	}

	public CallSiteBinder Binder
	{
		get
		{
			throw null;
		}
	}

	public Type DelegateType
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

	internal DynamicExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, IEnumerable<Expression> arguments)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public new static DynamicExpression Dynamic(CallSiteBinder binder, Type returnType, params Expression[] arguments)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, IEnumerable<Expression>? arguments)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
	{
		throw null;
	}

	public new static DynamicExpression MakeDynamic(Type delegateType, CallSiteBinder binder, params Expression[]? arguments)
	{
		throw null;
	}

	Expression IArgumentProvider.GetArgument(int index)
	{
		throw null;
	}

	object IDynamicExpression.CreateCallSite()
	{
		throw null;
	}

	Expression IDynamicExpression.Rewrite(Expression[] args)
	{
		throw null;
	}

	public DynamicExpression Update(IEnumerable<Expression>? arguments)
	{
		throw null;
	}
}
