using System.Collections;
using System.Collections.Generic;

namespace System.IO.Packaging;

public class PackagePartCollection : IEnumerable<PackagePart>, IEnumerable
{
	internal PackagePartCollection()
	{
	}

	public IEnumerator<PackagePart> GetEnumerator()
	{
		throw null;
	}

	IEnumerator<PackagePart> IEnumerable<PackagePart>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
