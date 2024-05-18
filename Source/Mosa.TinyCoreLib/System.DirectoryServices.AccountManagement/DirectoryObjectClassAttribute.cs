namespace System.DirectoryServices.AccountManagement;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DirectoryObjectClassAttribute : Attribute
{
	public ContextType? Context
	{
		get
		{
			throw null;
		}
	}

	public string ObjectClass
	{
		get
		{
			throw null;
		}
	}

	public DirectoryObjectClassAttribute(string objectClass)
	{
	}
}
