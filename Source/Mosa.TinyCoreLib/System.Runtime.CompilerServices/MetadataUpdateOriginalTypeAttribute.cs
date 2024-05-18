namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class MetadataUpdateOriginalTypeAttribute : Attribute
{
	public Type OriginalType
	{
		get
		{
			throw null;
		}
	}

	public MetadataUpdateOriginalTypeAttribute(Type originalType)
	{
		throw null;
	}
}
