namespace System.Data.Common;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class DbProviderSpecificTypePropertyAttribute : Attribute
{
	public bool IsProviderSpecificTypeProperty
	{
		get
		{
			throw null;
		}
	}

	public DbProviderSpecificTypePropertyAttribute(bool isProviderSpecificTypeProperty)
	{
	}
}
