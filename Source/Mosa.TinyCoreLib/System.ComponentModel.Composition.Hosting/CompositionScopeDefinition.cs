using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionScopeDefinition : ComposablePartCatalog, INotifyComposablePartCatalogChanged
{
	public virtual IEnumerable<CompositionScopeDefinition> Children
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<ExportDefinition> PublicSurface
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

	protected CompositionScopeDefinition()
	{
	}

	public CompositionScopeDefinition(ComposablePartCatalog catalog, IEnumerable<CompositionScopeDefinition> children)
	{
	}

	public CompositionScopeDefinition(ComposablePartCatalog catalog, IEnumerable<CompositionScopeDefinition> children, IEnumerable<ExportDefinition> publicSurface)
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
