using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Linq;

public abstract class EnumerableQuery
{
	internal EnumerableQuery()
	{
	}
}
[RequiresUnreferencedCode("Enumerating in-memory collections as IQueryable can require unreferenced code because expressions referencing IQueryable extension methods can get rebound to IEnumerable extension methods. The IEnumerable extension methods could be trimmed causing the application to fail at runtime.")]
[RequiresDynamicCode("Enumerating in-memory collections as IQueryable can require creating new generic types or methods, which requires creating code at runtime. This may not work when AOT compiling.")]
public class EnumerableQuery<T> : EnumerableQuery, IEnumerable<T>, IEnumerable, IOrderedQueryable, IQueryable, IOrderedQueryable<T>, IQueryable<T>, IQueryProvider
{
	Type IQueryable.ElementType
	{
		get
		{
			throw null;
		}
	}

	Expression IQueryable.Expression
	{
		get
		{
			throw null;
		}
	}

	IQueryProvider IQueryable.Provider
	{
		get
		{
			throw null;
		}
	}

	public EnumerableQuery(IEnumerable<T> enumerable)
	{
	}

	public EnumerableQuery(Expression expression)
	{
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	IQueryable IQueryProvider.CreateQuery(Expression expression)
	{
		throw null;
	}

	IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
	{
		throw null;
	}

	object IQueryProvider.Execute(Expression expression)
	{
		throw null;
	}

	TElement IQueryProvider.Execute<TElement>(Expression expression)
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
