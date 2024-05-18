using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class CompositionBatch
{
	public ReadOnlyCollection<ComposablePart> PartsToAdd
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<ComposablePart> PartsToRemove
	{
		get
		{
			throw null;
		}
	}

	public CompositionBatch()
	{
	}

	public CompositionBatch(IEnumerable<ComposablePart>? partsToAdd, IEnumerable<ComposablePart>? partsToRemove)
	{
	}

	public ComposablePart AddExport(Export export)
	{
		throw null;
	}

	public void AddPart(ComposablePart part)
	{
	}

	public void RemovePart(ComposablePart part)
	{
	}
}
