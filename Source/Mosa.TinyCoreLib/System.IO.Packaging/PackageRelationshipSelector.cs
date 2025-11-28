using System.Collections.Generic;

namespace System.IO.Packaging;

public sealed class PackageRelationshipSelector
{
	public string SelectionCriteria
	{
		get
		{
			throw null;
		}
	}

	public PackageRelationshipSelectorType SelectorType
	{
		get
		{
			throw null;
		}
	}

	public Uri SourceUri
	{
		get
		{
			throw null;
		}
	}

	public PackageRelationshipSelector(Uri sourceUri, PackageRelationshipSelectorType selectorType, string selectionCriteria)
	{
	}

	public List<PackageRelationship> Select(Package package)
	{
		throw null;
	}
}
