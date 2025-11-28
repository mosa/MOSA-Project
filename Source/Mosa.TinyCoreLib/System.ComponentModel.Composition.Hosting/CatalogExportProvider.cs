using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Composition.Hosting;

public class CatalogExportProvider : ExportProvider, IDisposable
{
	public ComposablePartCatalog Catalog
	{
		get
		{
			throw null;
		}
	}

	public ExportProvider? SourceProvider
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public CatalogExportProvider(ComposablePartCatalog catalog)
	{
	}

	public CatalogExportProvider(ComposablePartCatalog catalog, bool isThreadSafe)
	{
	}

	public CatalogExportProvider(ComposablePartCatalog catalog, CompositionOptions compositionOptions)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition? atomicComposition)
	{
		throw null;
	}
}
