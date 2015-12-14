// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
			// TODO: support multidimensional arrays
			get { return 1; }
		}

		/// <summary>
		///
		/// </summary>
		public void SetValue(object value, params int[] indices)
		{
			if (indices == null)
				throw new ArgumentNullException("indices");
			if (this.Rank != indices.Length)
				throw new ArgumentException("The number of dimensions in the current Array is not equal to the number of elements in indices.", "indices");

			// TODO
		}

		/// <summary>
		///
		/// </summary>
		public object GetValue(params int[] indices)
		{
			if (indices == null)
				throw new ArgumentNullException("indices");
			if (this.Rank != indices.Length)
				throw new ArgumentException("The number of dimensions in the current Array is not equal to the number of elements in indices.", "indices");

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

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int GetLength(int dimension);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern int GetLowerBound(int dimension);

		public int GetUpperBound(int dimension)
		{
			return GetLowerBound(dimension) + GetLength(dimension) - 1;
		}

		object ICloneable.Clone()
		{
			//TODO
			return null;
		}

		bool IList.IsFixedSize
		{
			get { return true; }
		}

		bool IList.IsReadOnly
		{
			get { return true; }
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
			// NOT SUPPORTED
			throw new NotSupportedException();
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
			// NOT SUPPORTED
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			// NOT SUPPORTED
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			// NOT SUPPORTED
			throw new NotSupportedException();
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

		public IEnumerator GetEnumerator()
		{
			return new SZArrayEnumerator(this);
		}

		public static T[] Empty<T>()
		{
			return EmptyArray<T>.Value;
		}

		private static class EmptyArray<T>
		{
			internal static readonly T[] Value = new T[0];
		}

		// TODO Support multidimensional arrays
		[Serializable]
		private sealed class SZArrayEnumerator : IEnumerator, IDisposable
		{
			private Array array;
			private int currentPosition;
			private int length;

			public SZArrayEnumerator(Array array)
			{
				if (array.Rank != 1 || array.GetLowerBound(0) != 0)
					throw new InvalidOperationException("SZArrayEnumerator only works on single dimension arrays with a lower bound of zero.");
				this.array = array;
				this.currentPosition = -1;
				this.length = array.Length;
			}

			public object Current
			{
				get
				{
					if (currentPosition < 0)
						throw new InvalidOperationException("Enumeration has not started.");
					if (currentPosition >= length)
						throw new InvalidOperationException("Enumeration has already ended.");
					return array.GetValue(currentPosition);
				}
			}

			public bool MoveNext()
			{
				if (currentPosition < length)
					currentPosition++;
				if (currentPosition < length)
					return true;
				else
					return false;
			}

			public void Reset()
			{
				currentPosition = -1;
			}

			public void Dispose()
			{
			}
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

		// ----------- STATIC METHODS ----------- //

		public static int IndexOf(Array array, object value)
		{
			if (array == null)
				throw new ArgumentNullException("array");

			// TODO
			throw new NotImplementedException();
		}

		// ---------------------------------------------- //
		// -------------------SZARRAYS------------------- //
		// ---------------------------------------------- //
		// The "this" in methods inside this class is not an instance of SZArrayHelper.
		// It is actually an array. The generic type parameter is filled in by the compiler.
		// This only occurs for SZ arrays. The methods are attached and the generic interfaces are added.
		private sealed class SZArrayHelper<T>
		{
			private SZArrayHelper()
			{
				throw new InvalidOperationException("Cannot instantiate this class!!!");
			}

			// -----------------------------------------------------------
			// ------- Implement IEnumerable<T> interface methods --------
			// -----------------------------------------------------------
			private IEnumerator<T> GetEnumerator()
			{
				return new SZGenericArrayEnumerator(RuntimeHelpers.UnsafeCast<T[]>(this));
			}

			// -----------------------------------------------------------
			// ------- Implement ICollection<T> interface methods --------
			// -----------------------------------------------------------
			private void CopyTo(T[] array, int index)
			{
				if (array != null && array.Rank != 1)
					throw new ArgumentException("Multidimensional arrays are not supported");

				T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
				Array.Copy(_this, 0, array, index, _this.Length);
			}

			private int get_Count()
			{
				T[] @this = RuntimeHelpers.UnsafeCast<T[]>(this);
				return @this.Length;
			}

			// -----------------------------------------------------------
			// ---------- Implement IList<T> interface methods -----------
			// -----------------------------------------------------------
			private T get_Item(int index)
			{
				T[] @this = RuntimeHelpers.UnsafeCast<T[]>(this);
				if ((uint)index >= (uint)@this.Length)
					throw new ArgumentOutOfRangeException("index");

				return @this[index];
			}

			private void set_Item(int index, T value)
			{
				T[] @this = RuntimeHelpers.UnsafeCast<T[]>(this);
				if ((uint)index >= (uint)@this.Length)
					throw new ArgumentOutOfRangeException("index");

				@this[index] = value;
			}

			private void Add(T value)
			{
				// NOT SUPPORTED
				throw new NotSupportedException();
			}

			private bool Contains(T value)
			{
				T[] @this = RuntimeHelpers.UnsafeCast<T[]>(this);
				return Array.IndexOf(@this, value) != -1;
			}

			private bool get_IsReadOnly()
			{
				return true;
			}

			private void Clear()
			{
				// NOT SUPPORTED
				throw new NotSupportedException();
			}

			private int IndexOf(T value)
			{
				T[] @this = RuntimeHelpers.UnsafeCast<T[]>(this);
				return Array.IndexOf(@this, value);
			}

			private void Insert(int index, T value)
			{
				// NOT SUPPORTED
				throw new NotSupportedException();
			}

			private bool Remove(T value)
			{
				// NOT SUPPORTED
				throw new NotSupportedException();
			}

			private void RemoveAt(int index)
			{
				// NOT SUPPORTED
				throw new NotSupportedException();
			}

			// This is a normal generic Enumerator for SZ arrays.
			// It doesn't have any of the "this" stuff that SZArrayHelper does.
			[Serializable]
			private sealed class SZGenericArrayEnumerator : IEnumerator<T>, IDisposable
			{
				private T[] array;
				private int currentPosition;
				private int length;

				public SZGenericArrayEnumerator(T[] array)
				{
					if (array.Rank != 1 || array.GetLowerBound(0) != 0)
						throw new InvalidOperationException("SZGenericArrayEnumerator only works on single dimension arrays with a lower bound of zero.");
					this.array = array;
					this.currentPosition = -1;
					this.length = array.Length;
				}

				public T Current
				{
					get
					{
						if (currentPosition < 0)
							throw new InvalidOperationException("Enumeration has not started.");
						if (currentPosition >= length)
							throw new InvalidOperationException("Enumeration has already ended.");
						return array[currentPosition];
					}
				}

				public bool MoveNext()
				{
					if (currentPosition < length)
						currentPosition++;
					if (currentPosition < length)
						return true;
					else
						return false;
				}

				public void Reset()
				{
					currentPosition = -1;
				}

				public void Dispose()
				{
				}

				object IEnumerator.Current
				{
					get { return this.Current; }
				}

				void IEnumerator.Reset()
				{
					this.currentPosition = -1;
				}
			}
		}
	}
}
