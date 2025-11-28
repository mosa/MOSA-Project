namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
public sealed class KnownTypeAttribute : Attribute
{
	public string? MethodName
	{
		get
		{
			throw null;
		}
	}

	public Type? Type
	{
		get
		{
			throw null;
		}
	}

	public KnownTypeAttribute(string methodName)
	{
	}

	public KnownTypeAttribute(Type type)
	{
	}
}
