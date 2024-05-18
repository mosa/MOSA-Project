using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionService : ICompositionService, IDisposable
{
	internal CompositionService()
	{
	}

	public void Dispose()
	{
	}

	public void SatisfyImportsOnce(ComposablePart part)
	{
	}
}
