/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;

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
