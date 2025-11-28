namespace System.Security;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class SecurityRulesAttribute : Attribute
{
	public SecurityRuleSet RuleSet
	{
		get
		{
			throw null;
		}
	}

	public bool SkipVerificationInFullTrust
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityRulesAttribute(SecurityRuleSet ruleSet)
	{
	}
}
