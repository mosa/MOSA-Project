/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Collections;

namespace System
{
	/// <summary>
	///
	/// </summary>
	public abstract class Array : ICloneable, IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable
	{
		private int length;

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		/// <summary>
		/// Gets the rank (number of dimensions) of the Array. For example, a one-dimensional array returns 1, a two-dimensional array returns 2, and so on.
		/// </summary>
		public int Rank
		{
			// TODO
			get { return 0; }
		}

		/// <summary>
		///
		/// </summary>
		public void SetValue(object value, int index)
		{
			// TODO
		}

		/// <summary>
		///
		/// </summary>
		public object GetValue(int index)
		{
			// TODO
			return null;
		}

		/// <summary>
		///
		/// </summary>
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			for (int s = 0, d = destinationIndex; s < length; s++, d++)
			{
				sourceArray.SetValue(destinationArray.GetValue(d), s + sourceIndex);
			}
		}

		object ICloneable.Clone()
		{
			//TODO
			return null;
		}

		bool IList.IsFixedSize
		{
			//TODO
			get { return false; }
		}

		bool IList.IsReadOnly
		{
			//TODO
			get { return false; }
		}

		object IList.this[int index]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		int IList.Add(object value)
		{
			throw new NotImplementedException();
		}

		void IList.Clear()
		{
			throw new NotImplementedException();
		}

		bool IList.Contains(object value)
		{
			throw new NotImplementedException();
		}

		int IList.IndexOf(object value)
		{
			throw new NotImplementedException();
		}

		void IList.Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		void IList.Remove(object value)
		{
			throw new NotImplementedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		int ICollection.Count
		{
			get
			{
				return this.length;
			}
		}

		bool ICollection.IsSynchronized
		{
			//TODO
			get
			{
				return true;
			}
		}

		object ICollection.SyncRoot
		{
			//TODO
			get
			{
				return this;
			}
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			throw new NotImplementedException();
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			throw new NotImplementedException();
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			throw new NotImplementedException();
		}
	}
}