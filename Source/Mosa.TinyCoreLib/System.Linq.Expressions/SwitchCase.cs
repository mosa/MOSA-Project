using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class SwitchCase
{
	public Expression Body
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<Expression> TestValues
	{
		get
		{
			throw null;
		}
	}

	internal SwitchCase()
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public SwitchCase Update(IEnumerable<Expression> testValues, Expression body)
	{
		throw null;
	}
}
