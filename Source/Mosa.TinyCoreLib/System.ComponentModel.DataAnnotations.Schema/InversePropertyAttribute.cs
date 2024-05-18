namespace System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class InversePropertyAttribute : Attribute
{
	public string Property
	{
		get
		{
			throw null;
		}
	}

	public InversePropertyAttribute(string property)
	{
	}
}
