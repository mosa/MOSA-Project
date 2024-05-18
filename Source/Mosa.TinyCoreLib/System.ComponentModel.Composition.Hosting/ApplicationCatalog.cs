using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace System.ComponentModel.Composition.Hosting;

public class ApplicationCatalog : ComposablePartCatalog, ICompositionElement
{
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

	public ApplicationCatalog()
	{
	}

	public ApplicationCatalog(ICompositionElement definitionOrigin)
	{
	}

	public ApplicationCatalog(ReflectionContext reflectionContext)
	{
	}

	public ApplicationCatalog(ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
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
