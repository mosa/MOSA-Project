/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System
{
	/// <summary>
	/// Represents a method using an internal metadata token.
	/// </summary>
	public struct RuntimeMethodHandle
	{
		internal RuntimeMethodHandle(IntPtr handle)
		{
			this.m_ptr = handle;
		}

		private IntPtr m_ptr;

		/// <summary>
		/// Gets a handle to the type represented by this instance.
		/// </summary>
		public IntPtr Value
		{
			get
			{
				return m_ptr;
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is System.RuntimeMethodHandle))
				return false;

			return ((RuntimeMethodHandle)obj).m_ptr == m_ptr;
		}

		public static bool operator ==(RuntimeMethodHandle value1, RuntimeMethodHandle value2)
		{
			return value1.m_ptr == value2.m_ptr;
		}

		public static bool operator !=(RuntimeMethodHandle value1, RuntimeMethodHandle value2)
		{
			return value1.m_ptr != value2.m_ptr;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}