namespace System.Linq.Expressions;

public sealed class MemberAssignment : MemberBinding
{
	public Expression Expression
	{
		get
		{
			throw null;
		}
	}

	internal MemberAssignment()
		: base(MemberBindingType.Assignment, null)
	{
	}

	public MemberAssignment Update(Expression expression)
	{
		throw null;
	}
}
