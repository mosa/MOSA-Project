using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition.Hosting;

public abstract class ExportProvider
{
	public event EventHandler<ExportsChangeEventArgs>? ExportsChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<ExportsChangeEventArgs>? ExportsChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public T? GetExportedValueOrDefault<T>()
	{
		throw null;
	}

	public T? GetExportedValueOrDefault<T>(string? contractName)
	{
		throw null;
	}

	public IEnumerable<T> GetExportedValues<T>()
	{
		throw null;
	}

	public IEnumerable<T> GetExportedValues<T>(string? contractName)
	{
		throw null;
	}

	public T? GetExportedValue<T>()
	{
		throw null;
	}

	public T? GetExportedValue<T>(string? contractName)
	{
		throw null;
	}

	public IEnumerable<Export> GetExports(ImportDefinition definition)
	{
		throw null;
	}

	public IEnumerable<Export> GetExports(ImportDefinition definition, AtomicComposition? atomicComposition)
	{
		throw null;
	}

	public IEnumerable<Lazy<object, object>> GetExports(Type type, Type? metadataViewType, string? contractName)
	{
		throw null;
	}

	protected abstract IEnumerable<Export>? GetExportsCore(ImportDefinition definition, AtomicComposition? atomicComposition);

	public IEnumerable<Lazy<T>> GetExports<T>()
	{
		throw null;
	}

	public IEnumerable<Lazy<T>> GetExports<T>(string? contractName)
	{
		throw null;
	}

	public IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>()
	{
		throw null;
	}

	public IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>(string? contractName)
	{
		throw null;
	}

	public Lazy<T>? GetExport<T>()
	{
		throw null;
	}

	public Lazy<T>? GetExport<T>(string? contractName)
	{
		throw null;
	}

	public Lazy<T, TMetadataView>? GetExport<T, TMetadataView>()
	{
		throw null;
	}

	public Lazy<T, TMetadataView>? GetExport<T, TMetadataView>(string? contractName)
	{
		throw null;
	}

	protected virtual void OnExportsChanged(ExportsChangeEventArgs e)
	{
	}

	protected virtual void OnExportsChanging(ExportsChangeEventArgs e)
	{
	}

	public bool TryGetExports(ImportDefinition definition, AtomicComposition? atomicComposition, out IEnumerable<Export>? exports)
	{
		throw null;
	}
}
