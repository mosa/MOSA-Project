using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Composition.Hosting;

public class ComposablePartExportProvider : ExportProvider, IDisposable
{
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

	public ComposablePartExportProvider()
	{
	}

	public ComposablePartExportProvider(bool isThreadSafe)
	{
	}

	public ComposablePartExportProvider(CompositionOptions compositionOptions)
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
}
