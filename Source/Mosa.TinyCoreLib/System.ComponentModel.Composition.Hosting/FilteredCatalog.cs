using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class FilteredCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	public FilteredCatalog Complement
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

	public FilteredCatalog(ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter)
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

	public FilteredCatalog IncludeDependencies()
	{
		throw null;
	}

	public FilteredCatalog IncludeDependencies(Func<ImportDefinition, bool> importFilter)
	{
		throw null;
	}

	public FilteredCatalog IncludeDependents()
	{
		throw null;
	}

	public FilteredCatalog IncludeDependents(Func<ImportDefinition, bool> importFilter)
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
