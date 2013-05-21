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
	public class Object
	{
		public Object()
		{
		}

		public virtual bool Equals(object obj)
		{
			return false;
		}

		public static bool Equals(object objA, object objB)
		{
			return false;
		}

		public virtual int GetHashCode()
		{
			return 0;
		}

		protected object MemberwiseClone()
		{
			return null;
		}

		public static bool ReferenceEquals(object objA, object objB)
		{
			return false;
		}

		public virtual string ToString()
		{
			return null;
		}
	}
}