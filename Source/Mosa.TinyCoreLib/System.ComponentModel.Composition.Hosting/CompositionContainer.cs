using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionContainer : ExportProvider, ICompositionService, IDisposable
{
	public ComposablePartCatalog? Catalog
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<ExportProvider> Providers
	{
		get
		{
			throw null;
		}
	}

	public CompositionContainer()
	{
	}

	public CompositionContainer(CompositionOptions compositionOptions, params ExportProvider[]? providers)
	{
	}

	public CompositionContainer(params ExportProvider[]? providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog? catalog, bool isThreadSafe, params ExportProvider[]? providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog? catalog, CompositionOptions compositionOptions, params ExportProvider[]? providers)
	{
	}

	public CompositionContainer(ComposablePartCatalog? catalog, params ExportProvider[]? providers)
	{
	}

	public void Compose(CompositionBatch batch)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected override IEnumerable<Export>? GetExportsCore(ImportDefinition definition, AtomicComposition? atomicComposition)
	{
		throw null;
	}

	public void ReleaseExport(Export export)
	{
	}

	public void ReleaseExports(IEnumerable<Export> exports)
	{
	}

	public void ReleaseExports<T>(IEnumerable<Lazy<T>> exports)
	{
	}

	public void ReleaseExports<T, TMetadataView>(IEnumerable<Lazy<T, TMetadataView>> exports)
	{
	}

	public void ReleaseExport<T>(Lazy<T> export)
	{
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
	}
}
