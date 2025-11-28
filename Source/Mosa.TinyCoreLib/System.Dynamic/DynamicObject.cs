using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Dynamic;

public class DynamicObject : IDynamicMetaObjectProvider
{
	protected DynamicObject()
	{
	}

	public virtual IEnumerable<string> GetDynamicMemberNames()
	{
		throw null;
	}

	public virtual DynamicMetaObject GetMetaObject(Expression parameter)
	{
		throw null;
	}

	public virtual bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object? result)
	{
		throw null;
	}

	public virtual bool TryConvert(ConvertBinder binder, out object? result)
	{
		throw null;
	}

	public virtual bool TryCreateInstance(CreateInstanceBinder binder, object?[]? args, [NotNullWhen(true)] out object? result)
	{
		throw null;
	}

	public virtual bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
	{
		throw null;
	}

	public virtual bool TryDeleteMember(DeleteMemberBinder binder)
	{
		throw null;
	}

	public virtual bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
	{
		throw null;
	}

	public virtual bool TryGetMember(GetMemberBinder binder, out object? result)
	{
		throw null;
	}

	public virtual bool TryInvoke(InvokeBinder binder, object?[]? args, out object? result)
	{
		throw null;
	}

	public virtual bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
	{
		throw null;
	}

	public virtual bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
	{
		throw null;
	}

	public virtual bool TrySetMember(SetMemberBinder binder, object? value)
	{
		throw null;
	}

	public virtual bool TryUnaryOperation(UnaryOperationBinder binder, out object? result)
	{
		throw null;
	}
}
