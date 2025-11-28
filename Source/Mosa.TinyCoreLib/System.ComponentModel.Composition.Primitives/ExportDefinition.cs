using System.Collections.Generic;

namespace System.ComponentModel.Composition.Primitives;

public class ExportDefinition
{
	public virtual string ContractName
	{
		get
		{
			throw null;
		}
	}

	public virtual IDictionary<string, object?> Metadata
	{
		get
		{
			throw null;
		}
	}

	protected ExportDefinition()
	{
	}

	public ExportDefinition(string contractName, IDictionary<string, object?>? metadata)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
