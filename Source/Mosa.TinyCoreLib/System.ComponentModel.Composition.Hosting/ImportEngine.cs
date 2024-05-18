using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class ImportEngine : ICompositionService, IDisposable
{
	public ImportEngine(ExportProvider sourceProvider)
	{
	}

	public ImportEngine(ExportProvider sourceProvider, bool isThreadSafe)
	{
	}

	public ImportEngine(ExportProvider sourceProvider, CompositionOptions compositionOptions)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void PreviewImports(ComposablePart part, AtomicComposition? atomicComposition)
	{
	}

	public void ReleaseImports(ComposablePart part, AtomicComposition? atomicComposition)
	{
	}

	public void SatisfyImports(ComposablePart part)
	{
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
	}
}
