// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Interface for "System.IComparable"
	/// </summary>
	public interface IComparable
	{
		int CompareTo(object obj);
	}

	public interface IComparable<T>
	{
		int CompareTo(T other);
	}
}
