// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections.Generic
{
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEnumerator<T> : IDisposable, IEnumerator
	{
		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
		new T Current { get; }
	}
}
