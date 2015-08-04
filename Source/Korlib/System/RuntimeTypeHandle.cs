// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public bool Equals(RuntimeTypeHandle obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is RuntimeTypeHandle))
				return false;

			return ((RuntimeTypeHandle)obj).m_ptr == m_ptr;
		}

		public static bool operator ==(RuntimeTypeHandle left, RuntimeTypeHandle right)
		{
			return left.m_ptr == right.m_ptr;
		}

		public static bool operator !=(RuntimeTypeHandle left, RuntimeTypeHandle right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return this.m_ptr.ToInt32();
		}
	}
}
