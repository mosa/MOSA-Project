// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections.Generic
{
	/// <summary>
	/// Supports a simple iteration over a generic collection.
	/// </summary>
	/// <typeparam name="T">The type of objects to enumerate.</typeparam>
	public interface IEnumerator<out T> : IDisposable, IEnumerator
	{
		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
		new T Current { get; }
	}
}
