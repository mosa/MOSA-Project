// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections
{
	/// <summary>
	/// Interface for "System.Collections.IStructuralEquatable"
	/// </summary>
	internal interface IStructuralEquatable
	{
		bool Equals(Object other, IEqualityComparer comparer);

		int GetHashCode(IEqualityComparer comparer);
	}
}
