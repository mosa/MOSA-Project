using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace System.ComponentModel.Composition.Hosting;

public class TypeCatalog : ComposablePartCatalog, ICompositionElement
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

	public TypeCatalog(IEnumerable<Type> types)
	{
	}

	public TypeCatalog(IEnumerable<Type> types, ICompositionElement definitionOrigin)
	{
	}

	public TypeCatalog(IEnumerable<Type> types, ReflectionContext reflectionContext)
	{
	}

	public TypeCatalog(IEnumerable<Type> types, ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
	{
	}

	public TypeCatalog(params Type[] types)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override IEnumerator<ComposablePartDefinition> GetEnumerator()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
