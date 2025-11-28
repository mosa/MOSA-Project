namespace System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DatabaseGeneratedAttribute : Attribute
{
	public DatabaseGeneratedOption DatabaseGeneratedOption
	{
		get
		{
			throw null;
		}
	}

	public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption)
	{
	}
}
