// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
	/// <summary>
	/// Supports all classes in the .NET Framework class hierarchy and provides low-level
	/// services to derived classes. This is the ultimate base class of all classes
	/// in the .NET Framework; it is the root of the type hierarchy.
	/// </summary>
	[Serializable]
	public class Object
	{
		private readonly IntPtr typeDefinitionPtr;

		private readonly IntPtr syncBlock;

		/// <summary>
		/// Initializes a new instance of the System.Object class.
		/// </summary>
		[NonVersionable]
		public Object()
		{
		}

		/// <summary>
		/// Object destructor.
		/// </summary>
		[NonVersionable]
		~Object()
		{
		}

		/// <summary>
		/// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Object"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="Object"/> to compare with the current <see cref="Object"/>.
		/// </param>
		/// <returns>
		/// true if the specified <see cref="Object"/> is equal to the current <see cref="Object"/>;
		/// otherwise, false.
		/// </returns>
		public virtual bool Equals(object obj)
		{
			return RuntimeHelpers.Equals(this, obj);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Object"/> instances are considered equal.
		/// </summary>
		/// <param name="left">The first <see cref="Object"/> to compare.</param>
		/// <param name="right">The second <see cref="Object"/> to compare.</param>
		/// <returns>
		/// true if left is the same instance as right or if both are null references
		/// or if left.Equals(right) returns true; otherwise, false.
		/// </returns>
		public static bool Equals(object left, object right)
		{
			if (left == right)
				return true;

			if (left == null || right == null)
				return false;

			return left.Equals(right);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current System.Object.
		/// </returns>
		public virtual int GetHashCode()
		{
			return RuntimeHelpers.GetHashCode(this);
		}

		/// <summary>
		/// Gets the <see cref="Type"/> of the current instance.
		/// </summary>
		/// <returns>
		/// The <see cref="Type"/> instance that represents the exact runtime type of the current
		/// instance.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Type GetType();

		/// <summary>
		/// Creates a shallow copy of the current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// A shallow copy of the current System.Object.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected extern object MemberwiseClone();

		/// <summary>
		/// Determines whether the specified <see cref="Object"/> instances are the same instance.
		/// </summary>
		/// <param name="left">The first <see cref="Object"/> to compare.</param>
		/// <param name="right">The second <see cref="Object"/> to compare.</param>
		/// <returns>
		/// true if left is the same instance as right or if both are null references;
		/// otherwise, false.
		/// </returns>
		[NonVersionable]
		public static bool ReferenceEquals(object left, object right)
		{
			return (left == right);
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents the current <see cref="Object"/>.
		/// </returns>
		public virtual string ToString()
		{
			return GetType().ToString();
		}
	}
}
