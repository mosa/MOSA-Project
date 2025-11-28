using System.Collections.Generic;

namespace System.ComponentModel.Composition.Primitives;

public abstract class ComposablePart
{
	public abstract IEnumerable<ExportDefinition> ExportDefinitions { get; }

	public abstract IEnumerable<ImportDefinition> ImportDefinitions { get; }

	public virtual IDictionary<string, object?> Metadata
	{
		get
		{
			throw null;
		}
	}

	public virtual void Activate()
	{
	}

	public abstract object? GetExportedValue(ExportDefinition definition);

	public abstract void SetImport(ImportDefinition definition, IEnumerable<Export> exports);
}
