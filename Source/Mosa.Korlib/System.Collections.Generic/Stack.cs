namespace System.Collections.Generic
{
	public class Stack<T> : IReadOnlyCollection<T>
	{
		private List<T> _array;

		public Stack()
		{
			_array = new List<T>();
		}

		public Stack(int capacity)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity.ToString());
			_array = new List<T>(capacity);
		}

		public Stack(IEnumerable<T> collection)
		{
			_array = new List<T>();
			foreach (var item in collection) _array.Add(item);
		}

		public int Count => _array.Count;

		public void Clear()
		{
			_array = new List<T>(_array.Capacity);
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return _array[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Push(T item)
		{
			_array.Add(item);
		}

		public T Peek()
		{
			return _array[_array.Count - 1];
		}

		public T Pop()
		{
			var value = _array[_array.Count - 1];
			_array.RemoveAt(_array.Count - 1);
			return value;
		}

		public bool Contains(T item)
		{
			return _array.Contains(item);
		}

		public T[] ToArray()
		{
			return _array.ToArray();
		}
	}

	public class Stack : IReadOnlyCollection<object>
	{
		private List<object> _array;

		public Stack()
		{
			_array = new List<object>();
		}

		public Stack(int capacity)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity.ToString());
			_array = new List<object>(capacity);
		}

		public Stack(IEnumerable<object> collection)
		{
			_array = new List<object>();
			foreach (var item in collection) _array.Add(item);
		}

		public int Count => _array.Count;

		public void Clear()
		{
			_array = new List<object>(_array.Capacity);
		}

		public IEnumerator<object> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return _array[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Push(object item)
		{
			_array.Add(item);
		}

		public object Peek()
		{
			return _array[_array.Count - 1];
		}

		public object Pop()
		{
			var value = _array[_array.Count - 1];
			_array.RemoveAt(_array.Count - 1);
			return value;
		}

		public bool Contains(object item)
		{
			return _array.Contains(item);
		}

		public object[] ToArray()
		{
			return _array.ToArray();
		}
	}
}
