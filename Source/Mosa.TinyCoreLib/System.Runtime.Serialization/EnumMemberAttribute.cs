namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class EnumMemberAttribute : Attribute
{
	public bool IsValueSetExplicitly
	{
		get
		{
			throw null;
		}
	}

	public string? Value
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
