using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class AggregateCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	public ICollection<ComposablePartCatalog> Catalogs
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing
	{
		add
		{
		}
		remove
		{
		}
	}

	public AggregateCatalog()
	{
	}

	public AggregateCatalog(IEnumerable<ComposablePartCatalog>? catalogs)
	{
	}

	public AggregateCatalog(params ComposablePartCatalog[]? catalogs)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		throw null;
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		throw null;
	}

	protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
	{
	}

	protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
	{
	}
}
