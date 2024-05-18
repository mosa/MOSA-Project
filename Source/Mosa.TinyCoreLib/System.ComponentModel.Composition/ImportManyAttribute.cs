namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class ImportManyAttribute : Attribute
{
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

	public ImportManyAttribute()
	{
	}

	public ImportManyAttribute(string? contractName)
	{
	}

	public ImportManyAttribute(string? contractName, Type? contractType)
	{
	}

	public ImportManyAttribute(Type? contractType)
	{
	}
}
