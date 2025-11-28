using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace System.ComponentModel.Composition.Hosting;

public class AssemblyCatalog : ComposablePartCatalog, ICompositionElement
{
	public Assembly Assembly
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

	public AssemblyCatalog(Assembly assembly)
	{
	}

	public AssemblyCatalog(Assembly assembly, ICompositionElement definitionOrigin)
	{
	}

	public AssemblyCatalog(Assembly assembly, ReflectionContext reflectionContext)
	{
	}

	public AssemblyCatalog(Assembly assembly, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
	}

	public AssemblyCatalog(string codeBase)
	{
	}

	public AssemblyCatalog(string codeBase, ICompositionElement definitionOrigin)
	{
	}

	public AssemblyCatalog(string codeBase, ReflectionContext reflectionContext)
	{
	}

	public AssemblyCatalog(string codeBase, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
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

	public override string ToString()
	{
		throw null;
	}
}
