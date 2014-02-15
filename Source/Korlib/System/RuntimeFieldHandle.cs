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
    /// Represents a field using an internal metadata token.
	/// </summary>
    [SerializableAttribute]
	public struct RuntimeFieldHandle
	{
        /// <summary>
        /// An IntPtr that contains the handle to the field represented by the current instance.
        /// </summary>
        public IntPtr Value;
	}
}