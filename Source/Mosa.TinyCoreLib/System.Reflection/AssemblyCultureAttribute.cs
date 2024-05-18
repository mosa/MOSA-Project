namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCultureAttribute : Attribute
{
	public string Culture
	{
		get
		{
			throw null;
		}
	}

	public AssemblyCultureAttribute(string culture)
	{
	}
}
