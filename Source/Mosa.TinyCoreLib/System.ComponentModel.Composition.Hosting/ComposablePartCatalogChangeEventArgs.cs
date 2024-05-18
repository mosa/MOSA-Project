using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class ComposablePartCatalogChangeEventArgs : EventArgs
{
	public IEnumerable<ComposablePartDefinition> AddedDefinitions
	{
		get
		{
			throw null;
		}
	}

	public AtomicComposition? AtomicComposition
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<ComposablePartDefinition> RemovedDefinitions
	{
		get
		{
			throw null;
		}
	}

	public ComposablePartCatalogChangeEventArgs(IEnumerable<ComposablePartDefinition> addedDefinitions, IEnumerable<ComposablePartDefinition> removedDefinitions, AtomicComposition? atomicComposition)
	{
	}
}
