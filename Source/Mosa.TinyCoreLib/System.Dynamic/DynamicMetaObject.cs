using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Dynamic;

public class DynamicMetaObject
{
	public static readonly DynamicMetaObject[] EmptyMetaObjects;

	public Expression Expression
	{
		get
		{
			throw null;
		}
	}

	public bool HasValue
	{
		get
		{
			throw null;
		}
	}

	public Type LimitType
	{
		get
		{
			throw null;
		}
	}

	public BindingRestrictions Restrictions
	{
		get
		{
			throw null;
		}
	}

	public Type? RuntimeType
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public DynamicMetaObject(Expression expression, BindingRestrictions restrictions)
	{
	}

	public DynamicMetaObject(Expression expression, BindingRestrictions restrictions, object value)
	{
	}

	public virtual DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindConvert(ConvertBinder binder)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindGetMember(GetMemberBinder binder)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
	{
		throw null;
	}

	public virtual DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
	{
		throw null;
	}

	public static DynamicMetaObject Create(object value, Expression expression)
	{
		throw null;
	}

	public virtual IEnumerable<string> GetDynamicMemberNames()
	{
		throw null;
	}
}
