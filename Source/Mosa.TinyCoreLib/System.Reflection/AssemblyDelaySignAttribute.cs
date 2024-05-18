namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyDelaySignAttribute : Attribute
{
	public bool DelaySign
	{
		get
		{
			throw null;
		}
	}

	public AssemblyDelaySignAttribute(bool delaySign)
	{
	}
}
