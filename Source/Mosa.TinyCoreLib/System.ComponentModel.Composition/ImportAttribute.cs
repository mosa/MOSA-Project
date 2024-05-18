namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class ImportAttribute : Attribute
{
	public bool AllowDefault
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool AllowRecomposition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public CreationPolicy RequiredCreationPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ImportSource Source
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ImportAttribute()
	{
	}

	public ImportAttribute(string? contractName)
	{
	}

	public ImportAttribute(string? contractName, Type? contractType)
	{
	}

	public ImportAttribute(Type? contractType)
	{
	}
}
