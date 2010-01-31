/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// Interface for "System.IDisposable"
	/// </summary>
    public interface IDisposable
    {
		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
        void Dispose();
    }
}
