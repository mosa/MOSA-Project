// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections
{
	/// <summary>
	/// Interface for "System.Collections.IStructuralComparable"
	/// </summary>
	public interface IStructuralComparable
	{
		int CompareTo(object other, IComparer comparer);
	}
}
