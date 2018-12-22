// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Represents a type using an internal metadata token.
	/// </summary>
	public struct RuntimeTypeHandle
	{
		public RuntimeTypeHandle(IntPtr handle) // FIXME: hack - should be internal
		{
			m_type = handle;
		}

		private IntPtr m_type;

		/// <summary>
		/// Gets a handle to the type represented by this instance.
		/// </summary>
		public IntPtr Value { get { return m_type; } }

		public bool Equals(RuntimeTypeHandle obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is RuntimeTypeHandle))
				return false;

			RuntimeTypeHandle handle = (RuntimeTypeHandle)obj;
			return handle.m_type == m_type;
		}

		public static bool operator ==(RuntimeTypeHandle left, object right)
		{
			return left.Equals(right);
		}

		public static bool operator ==(object left, RuntimeTypeHandle right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(RuntimeTypeHandle left, object right)
		{
			return !left.Equals(right);
		}

		public static bool operator !=(object left, RuntimeTypeHandle right)
		{
			return !right.Equals(left);
		}

		public override int GetHashCode()
		{
			return m_type.ToInt32();
		}
	}
}
