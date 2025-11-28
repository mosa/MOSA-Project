using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public class ExportsChangeEventArgs : EventArgs
{
	public IEnumerable<ExportDefinition> AddedExports
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

	public IEnumerable<string> ChangedContractNames
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<ExportDefinition> RemovedExports
	{
		get
		{
			throw null;
		}
	}

	public ExportsChangeEventArgs(IEnumerable<ExportDefinition> addedExports, IEnumerable<ExportDefinition> removedExports, AtomicComposition? atomicComposition)
	{
	}
}
