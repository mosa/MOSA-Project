using System.Reflection;

namespace System.Linq.Expressions;

public abstract class MemberBinding
{
	public MemberBindingType BindingType
	{
		get
		{
			throw null;
		}
	}

	public MemberInfo Member
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This constructor has been deprecated and is not supported.")]
	protected MemberBinding(MemberBindingType type, MemberInfo member)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
