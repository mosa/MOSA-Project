using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class MemberMemberBinding : MemberBinding
{
	public ReadOnlyCollection<MemberBinding> Bindings
	{
		get
		{
			throw null;
		}
	}

	internal MemberMemberBinding()
		: base(MemberBindingType.Assignment, null)
	{
	}

	public MemberMemberBinding Update(IEnumerable<MemberBinding> bindings)
	{
		throw null;
	}
}
