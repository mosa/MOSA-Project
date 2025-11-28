namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class OptionalFieldAttribute : Attribute
{
	public int VersionAdded
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
