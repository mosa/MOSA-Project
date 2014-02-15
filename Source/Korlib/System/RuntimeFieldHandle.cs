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
	///
	/// </summary>
	public struct RuntimeFieldHandle
	{
        internal RuntimeFieldHandle(IntPtr handle)
        {
            this.m_ptr = handle;
        }

        private IntPtr m_ptr;
        public IntPtr Value
        {
            get
            {
                return m_ptr;
            }
        }
	}
}