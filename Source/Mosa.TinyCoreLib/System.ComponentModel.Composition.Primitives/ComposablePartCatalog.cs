using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.ComponentModel.Composition.Primitives;

public abstract class ComposablePartCatalog : IEnumerable<ComposablePartDefinition>, IEnumerable, IDisposable
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual IQueryable<ComposablePartDefinition> Parts
	{
		get
		{
			throw null;
		}
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		throw null;
	}

	public virtual IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
