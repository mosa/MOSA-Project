using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Dynamic;

public abstract class DynamicMetaObjectBinder : CallSiteBinder
{
	public virtual Type ReturnType
	{
		get
		{
			throw null;
		}
	}

	public abstract DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args);

	public sealed override Expression Bind(object[] args, ReadOnlyCollection<ParameterExpression> parameters, LabelTarget returnLabel)
	{
		throw null;
	}

	public DynamicMetaObject Defer(DynamicMetaObject target, params DynamicMetaObject[]? args)
	{
		throw null;
	}

	public DynamicMetaObject Defer(params DynamicMetaObject[] args)
	{
		throw null;
	}

	public Expression GetUpdateExpression(Type type)
	{
		throw null;
	}
}
