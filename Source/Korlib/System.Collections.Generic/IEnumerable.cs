// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections.Generic
{
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEnumerable<T> : IEnumerable
	{
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		new IEnumerator<T> GetEnumerator();
	}
}
