namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field)]
public sealed class AccessedThroughPropertyAttribute : Attribute
{
	public string PropertyName
	{
		get
		{
			throw null;
		}
	}

	public AccessedThroughPropertyAttribute(string propertyName)
	{
	}
}
