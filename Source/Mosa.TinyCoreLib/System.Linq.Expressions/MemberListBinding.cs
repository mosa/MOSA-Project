using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class MemberListBinding : MemberBinding
{
	public ReadOnlyCollection<ElementInit> Initializers
	{
		get
		{
			throw null;
		}
	}

	internal MemberListBinding()
		: base(MemberBindingType.Assignment, null)
	{
	}

	public MemberListBinding Update(IEnumerable<ElementInit> initializers)
	{
		throw null;
	}
}
