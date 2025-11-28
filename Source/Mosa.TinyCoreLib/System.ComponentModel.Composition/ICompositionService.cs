using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition;

public interface ICompositionService
{
	void SatisfyImportsOnce(ComposablePart part);
}
