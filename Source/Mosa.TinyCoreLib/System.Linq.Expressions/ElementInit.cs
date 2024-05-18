using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions;

public sealed class ElementInit : IArgumentProvider
{
	public MethodInfo AddMethod
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<Expression> Arguments
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

	internal ElementInit()
	{
	}

	Expression IArgumentProvider.GetArgument(int index)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public ElementInit Update(IEnumerable<Expression> arguments)
	{
		throw null;
	}
}
