// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections
{
	[Obsolete("Please use IEqualityComparer instead.")]
	public interface IHashCodeProvider
	{
		// Interfaces are not serializable
		// Returns a hash code for the given object.
		//
		int GetHashCode(Object obj);
	}
}
