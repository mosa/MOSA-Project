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
	/// Supports all classes in the .NET Framework class hierarchy and provides low-level
	/// services to derived classes. This is the ultimate base class of all classes
	/// in the .NET Framework; it is the root of the type hierarchy.
	/// </summary>
	[Serializable]
	public class Object
	{
		private IntPtr methodTablePtr;

		private IntPtr syncBlock;

		/// <summary>
		/// Initializes a new instance of the System.Object class.
		/// </summary>
		public Object()
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
			return this == obj;
		}

		/// <summary>
		/// Determines whether the specified <see cref="Object"/> instances are considered equal.
		/// </summary>
		/// <param name="objA">The first <see cref="Object"/> to compare.</param>
		/// <param name="objB">The second <see cref="Object"/> to compare.</param>
		/// <returns>
		/// true if objA is the same instance as objB or if both are null references
		/// or if objA.Equals(objB) returns true; otherwise, false.
		/// </returns>
		public static bool Equals(object objA, object objB)
		{
			if (objA == objB)
				return true;

			if (objA == null || objB == null)
				return false;

			return objA.Equals(objB);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current System.Object.
		/// </returns>
		public virtual int GetHashCode()
		{
			// Todo: Implement GetHashCode as location in memory.
			return 0;
		}

		/// <summary>
		/// Gets the <see cref="Type"/> of the current instance.
		/// </summary>
		/// <returns>
		/// The <see cref="Type"/> instance that represents the exact runtime type of the current
		/// instance.
		/// </returns>
		public Type GetType()
		{
			return Type.GetTypeFromHandle(Type.GetTypeHandle(this));
		}

		/// <summary>
		/// Creates a shallow copy of the current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// A shallow copy of the current System.Object.
		/// </returns>
		protected object MemberwiseClone()
		{
			return new Object();
		}

		/// <summary>
		/// Determines whether the specified <see cref="Object"/> instances are the same instance.
		/// </summary>
		/// <param name="objA">The first <see cref="Object"/> to compare.</param>
		/// <param name="objB">The second <see cref="Object"/> to compare.</param>
		/// <returns>
		/// true if objA is the same instance as objB or if both are null references;
		/// otherwise, false.
		/// </returns>
		public static bool ReferenceEquals(object objA, object objB)
		{
			return (objA == objB);
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