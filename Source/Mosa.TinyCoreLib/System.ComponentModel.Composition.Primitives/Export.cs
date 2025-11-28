using System.Collections.Generic;

namespace System.ComponentModel.Composition.Primitives;

public class Export
{
	public virtual ExportDefinition Definition
	{
		get
		{
			throw null;
		}
	}

	public IDictionary<string, object?> Metadata
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	protected Export()
	{
	}

	public Export(ExportDefinition definition, Func<object?> exportedValueGetter)
	{
	}

	public Export(string contractName, IDictionary<string, object?>? metadata, Func<object?> exportedValueGetter)
	{
	}

	public Export(string contractName, Func<object?> exportedValueGetter)
	{
	}

	protected virtual object? GetExportedValueCore()
	{
		throw null;
	}
}
