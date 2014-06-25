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
	public struct RuntimeTypeHandle
	{
		internal RuntimeTypeHandle(IntPtr handle)
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

		public override bool Equals(object obj)
		{
			if (!(obj is System.RuntimeTypeHandle))
				return false;

			return ((RuntimeTypeHandle)obj).m_ptr == m_ptr;
		}

		public static bool operator ==(RuntimeTypeHandle value1, RuntimeTypeHandle value2)
		{
			return value1.Equals(value2);
		}

		public static bool operator !=(RuntimeTypeHandle value1, RuntimeTypeHandle value2)
		{
			return !value1.Equals(value2);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}