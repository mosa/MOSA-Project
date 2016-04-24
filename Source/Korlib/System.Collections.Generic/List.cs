// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Collections.Generic
{
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class List<T> : IList<T>
	{
		private const int _defaultCapacity = 4;

		private static readonly T[] _emptyArray = new T[0];

		private T[] _items;
		private int _size;

		public List()
		{
			_items = _emptyArray;
			_size = 0;
		}

		public List(int capacity)
		{
			if (capacity == 0)
				_items = _emptyArray;
			else
				_items = new T[capacity];
		}

		public int Count
		{
			get { return _size; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		private void EnsureCapacity(int size)
		{
			if (_items.Length < size)
			{
				if (size == 0)
				{
					_items = new T[_defaultCapacity];
				}
				else
				{
					int newsize = _items.Length * 2;

					if (newsize < size)
						newsize = size;

					Resize(newsize);
				}
			}
		}

		private void Resize(int newsize)
		{
			var previous = _items;

			_items = new T[newsize];

			for (int i = 0; i < _size; i++)
			{
				_items[i] = previous[i];
			}
		}

		public void Add(T item)
		{
			EnsureCapacity(_size + 1);

			_items[_size] = item;
			_size++;
		}

		public void Clear()
		{
			_size = 0;
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < _size; i++)
			{
				if (_items[i].Equals(item))
					return true;
			}
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException("index");

			for (int i = 0; i < _size; i++)
			{
				array[arrayIndex++] = _items[i];
			}
		}

		public bool Remove(T item)
		{
			int at = IndexOf(item);

			if (at < 0)
				return false;

			RemoveAt(at);

			return true;
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i < _size; i++)
			{
				if (_items[i].Equals(item))
					return i;
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			EnsureCapacity(_size + 1);

			_size++;
			for (int i = index; i < _size; i++)
			{
				_items[i] = _items[i + 1];
			}

			_items[index] = item;
		}

		/// <summary>
		/// Removes at.
		/// </summary>
		/// <param name="index">The index.</param>
		public void RemoveAt(int index)
		{
			_size--;

			for (int i = index; i < _size; i++)
			{
				_items[i] = _items[i + 1];
			}

			_items[_size] = default(T);
		}

		/// <summary>
		/// Gets or sets the T at the specified index.
		/// </summary>
		/// <value></value>
		public T this[int index]
		{
			get { return _items[index]; }
			set { _items[index] = value; }
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
		{
			private List<T> list;
			private int index;
			private T current;

			internal Enumerator(List<T> list)
			{
				this.list = list;
				index = 0;
				current = default(T);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				List<T> localList = list;

				if (((uint)index < (uint)localList._size))
				{
					current = localList._items[index];
					index++;
					return true;
				}
				return MoveNextRare();
			}

			private bool MoveNextRare()
			{
				index = list._size + 1;
				current = default(T);
				return false;
			}

			public T Current
			{
				get
				{
					return current;
				}
			}

			Object System.Collections.IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			void System.Collections.IEnumerator.Reset()
			{
				index = 0;
				current = default(T);
			}
		}
	}
}
