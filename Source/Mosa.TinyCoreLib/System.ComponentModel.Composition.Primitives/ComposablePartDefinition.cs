using System.Collections.Generic;

namespace System.ComponentModel.Composition.Primitives;

public abstract class ComposablePartDefinition
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

	public abstract ComposablePart CreatePart();
}
