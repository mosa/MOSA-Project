using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace System.ComponentModel.Composition.Hosting;

public class DirectoryCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged, ICompositionElement
{
	public string FullPath
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<string> LoadedFiles
	{
		get
		{
			throw null;
		}
	}

	public string Path
	{
		get
		{
			throw null;
		}
	}

	public string SearchPattern
	{
		get
		{
			throw null;
		}
	}

	string ICompositionElement.DisplayName
	{
		get
		{
			throw null;
		}
	}

	ICompositionElement? ICompositionElement.Origin
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing
	{
		add
		{
		}
		remove
		{
		}
	}

	public DirectoryCatalog(string path)
	{
	}

	public DirectoryCatalog(string path, ICompositionElement definitionOrigin)
	{
	}

	public DirectoryCatalog(string path, ReflectionContext reflectionContext)
	{
	}

	public DirectoryCatalog(string path, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
	}

	public DirectoryCatalog(string path, string searchPattern)
	{
	}

	public DirectoryCatalog(string path, string searchPattern, ICompositionElement definitionOrigin)
	{
	}

	public DirectoryCatalog(string path, string searchPattern, ReflectionContext reflectionContext)
	{
	}

	public DirectoryCatalog(string path, string searchPattern, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		throw null;
	}

	public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		throw null;
	}

	protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
	{
	}

	protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
	{
	}

	public void Refresh()
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
