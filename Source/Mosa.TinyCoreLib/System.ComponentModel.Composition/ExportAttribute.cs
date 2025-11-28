namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class ExportAttribute : Attribute
{
	public string? ContractName
	{
		get
		{
			throw null;
		}
	}

	public Type? ContractType
	{
		get
		{
			throw null;
		}
	}

	public ExportAttribute()
	{
	}

	public ExportAttribute(string? contractName)
	{
	}

	public ExportAttribute(string? contractName, Type? contractType)
	{
	}

	public ExportAttribute(Type? contractType)
	{
	}
}
