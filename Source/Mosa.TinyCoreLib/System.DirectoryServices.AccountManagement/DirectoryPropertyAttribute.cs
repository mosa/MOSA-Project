namespace System.DirectoryServices.AccountManagement;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class DirectoryPropertyAttribute : Attribute
{
	public ContextType? Context
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SchemaAttributeName
	{
		get
		{
			throw null;
		}
	}

	public DirectoryPropertyAttribute(string schemaAttributeName)
	{
	}
}
