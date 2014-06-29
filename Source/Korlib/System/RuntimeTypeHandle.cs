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
	/// Represents a type using an internal metadata token.
	/// </summary>
	public struct RuntimeTypeHandle
	{
		internal RuntimeTypeHandle(IntPtr handle)
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
			if (!(obj is System.RuntimeTypeHandle))
				return false;

			return ((RuntimeTypeHandle)obj).m_ptr == m_ptr;
		}

		public static bool operator ==(RuntimeTypeHandle value1, RuntimeTypeHandle value2)
		{
			return value1.m_ptr == value2.m_ptr;
		}

		public static bool operator !=(RuntimeTypeHandle value1, RuntimeTypeHandle value2)
		{
			return value1.m_ptr != value2.m_ptr;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}