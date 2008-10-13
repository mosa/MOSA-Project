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
    public interface IEnumerable<T> : IEnumerable
    {
		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
        new IEnumerator<T> GetEnumerator();
    }
}
