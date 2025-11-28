using System.Collections;

namespace System.Security.AccessControl;

public sealed class AuthorizationRuleCollection : ReadOnlyCollectionBase
{
	public AuthorizationRule? this[int index]
	{
		get
		{
			throw null;
		}
	}

	public void AddRule(AuthorizationRule? rule)
	{
	}

	public void CopyTo(AuthorizationRule[] rules, int index)
	{
	}
}
