using System;

namespace Mosa.UnitTests.Mosa.Collections.Generic
{
	public class TClass: IComparable
	{
		public uint ID = 0;
		public uint Magic = 0;

		public TClass()
		{
			this.ID = 0;
			this.Magic = 0;
		}

		public TClass(uint ID, uint Magic)
		{
			this.ID = ID;
			this.Magic = Magic;
		}

		public int CompareTo(object obj)
		{
			if (obj != null)
			{
				if (this.GetType() == obj.GetType() && this is TClass && obj is TClass)
				{
					int Result = 0;

					TClass LHS = this;
					TClass RHS = (TClass)obj;

					if (LHS.Magic < RHS.Magic) Result = -1;
					if (LHS.Magic == RHS.Magic) Result = 0;
					if (LHS.Magic > RHS.Magic) Result = 1;

					return Result;
				}
				else
				{
					throw new ArgumentException("Object is not a 'TClass' type object!");
				}
			}
			else
			{
				throw new ArgumentNullException("obj", "Object cannot be NULL!");
			}
		}

		public static bool operator ==(TClass lhs, TClass rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(TClass lhs, TClass rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object obj)
		{
			bool Result = false;

			// In order to have "equality", we must have same type of objects
			if (this.GetType() == obj.GetType() && this is TClass && obj is TClass)
			{
				// Check whether we have same instance
				if (Object.ReferenceEquals(this, obj))
				{
					Result = true;
				}

				// Check whether both objects are null
				if (Object.ReferenceEquals(this, null) && Object.ReferenceEquals(obj, null))
				{
					Result = true;
				}

				// Check whether the "contents" are the same
				if (!Object.ReferenceEquals(this, null) && !Object.ReferenceEquals(obj, null))
				{
					TClass LHS = this;
					TClass RHS = (TClass)obj;

					Result = (LHS.Magic == RHS.Magic);
				}
			}

			return Result;
		}

		public override int GetHashCode()
		{
			return (int)this.Magic;
		}

		public override string ToString()
		{
			return "[ID:" + this.ID + ", MAGIC:" + this.Magic + "]";
		}
	}
}
