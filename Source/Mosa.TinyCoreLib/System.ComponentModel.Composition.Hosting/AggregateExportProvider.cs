using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class AggregateExportProvider : ExportProvider, IDisposable
{
	public ReadOnlyCollection<ExportProvider> Providers
	{
		get
		{
			throw null;
		}
	}

	public AggregateExportProvider(IEnumerable<ExportProvider>? providers)
	{
	}

	public AggregateExportProvider(params ExportProvider[]? providers)
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
