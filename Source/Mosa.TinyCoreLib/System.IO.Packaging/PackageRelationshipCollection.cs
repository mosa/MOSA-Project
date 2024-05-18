using System.Collections;
using System.Collections.Generic;

namespace System.IO.Packaging;

public class PackageRelationshipCollection : IEnumerable<PackageRelationship>, IEnumerable
{
	internal PackageRelationshipCollection()
	{
	}

	public IEnumerator<PackageRelationship> GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
