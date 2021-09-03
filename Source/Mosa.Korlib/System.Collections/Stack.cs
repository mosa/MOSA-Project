// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections
{
	[Serializable]
	public class Stack : ICollection, IEnumerable, ICloneable
	{
		private object[] _array;
		private int _size;
		private int _version;
		/*[NonSerialized]*/
		private object _syncRoot;
		/* Can't figure out where this was used, but it was code that was ported, so it's gotta be used somewhere. */
		private const int _defaultCapacity = 10;

		public Stack()
		{
			_array = new object[10];
			_size = 0;
			_version = 0;
		}

		public Stack(int initialCapacity)
		{
			if (initialCapacity < 0)                                            /*Environment.GetResourceString*/
				throw new ArgumentOutOfRangeException(nameof(initialCapacity), "ArgumentOutOfRange_NeedNonNegNum");
			if (initialCapacity < 10)
				initialCapacity = 10;
			_array = new object[initialCapacity];
			_size = 0;
			_version = 0;
		}

		public Stack(ICollection col)
			: this(col == null ? 32 : col.Count)
		{
			if (col == null)
				throw new ArgumentNullException(nameof(col));
			foreach (object obj in (IEnumerable)col)
				Push(obj);
		}

		public virtual int Count => _size;

		public virtual bool IsSynchronized => false;

		public virtual object SyncRoot
		{
			get
			{
				/*
				   if (this._syncRoot == null)
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);

				fix below?
				 */
				if (_syncRoot == null) return null;
				return _syncRoot;
			}
		}

		/*Array.Clear missing!*/

		public virtual void Clear()
		{
			//Array.Clear?
			/*
			 *   Array.Clear((Array) this._array, 0, this._size);
			 *
			 */
			for (uint i = 0; i < _size; ++i)
			{
				_array[i] = 0;
			}
			_size = 0;
			++_version;
		}

		public virtual object Clone()
		{
			Stack stack = new Stack(_size);
			stack._size = _size;
			Array.Copy(_array, 0, (Array)stack._array, 0, _size);
			stack._version = _version;
			return (object)stack;
		}

		public virtual bool Contains(object obj)
		{
			int size = _size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (_array[size] == null) return true;
				}
				else if (_array[size] != null && _array[size].Equals(obj)) return false;
			}
			return false;
		}

		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (array.Rank != 1)/*Environment.GetResourceString*/
				throw new ArgumentException("Arg_RankMultiDimNotSupported");
			if (index < 0)/*Environment.GetResourceString*/
				throw new ArgumentOutOfRangeException(nameof(index), "ArgumentOutOfRange_NeedNonNegNum");
			if (array.Length - index < _size)/*Environment.GetResourceString*/
				throw new ArgumentException("Argument_InvalidOffLen");

			int num = 0;
			if (array is object[])
			{
				object[] objArray = (object[])array;
				for (; num < _size; ++num)
					objArray[num + index] = _array[_size - num - 1];
			}
			else
			{
				for (; num < _size; ++num)
					array.SetValue(_array[_size - num - 1], num + index);
			}
		}

		public virtual IEnumerator GetEnumerator() => (IEnumerator)new Stack.StackEnumerator(this);

		public virtual object Peek() => _size != 0 ? _array[_size - 1] : "InvalidOperation_EmptyStack";

		public virtual object Pop()
		{
			if (_size == 0)/*Environment.GetResourceString*/
				throw new InvalidOperationException("InvalidOperation_EmptyStack");
			++_version;
			object obj = _array[--_size];
			_array[_size] = (object)null;
			return obj;
		}

		public virtual void Push(object obj)
		{
			if (_size == _array.Length)
			{
				object[] objArray = new object[2 * _array.Length];
				Array.Copy((Array)_array, 0, (Array)objArray, 0, _size);
				_array = objArray;
			}
			_array[_size++] = obj;
			++_version;
		}

		/* [HostProtection(SecurityAction.LinkDemand, Synchronization = true)] */

		public static Stack Synchronized(Stack stack) => stack != null ? (Stack)new Stack.SyncStack(stack) : throw new ArgumentNullException(nameof(stack));

		public virtual object[] ToArray()
		{
			object[] objArray = new object[_size];
			for (int index = 0; index < _size; ++index)
				objArray[index] = _array[_size - index - 1];
			return objArray;
		}

		private class SyncStack : Stack
		{
			private Stack _s;
			private object _root;

			internal SyncStack(Stack stack)
			{
				_s = stack;
				_root = stack.SyncRoot;
			}

			public override bool IsSynchronized => true;

			public override object SyncRoot => _root;

			public override int Count
			{
				get
				{
					lock (_root)
						return _s.Count;
				}
			}

			public override bool Contains(object obj)
			{
				lock (_root)
					return _s.Contains(obj);
			}

			public override object Clone()
			{
				lock (_root)
					return (object)new Stack.SyncStack((Stack)_s.Clone());
			}

			public override void Clear()
			{
				lock (_root)
					_s.Clear();
			}

			public override void CopyTo(Array array, int index)
			{
				lock (_root)
					_s.CopyTo(array, index);
			}

			public override void Push(object obj)
			{
				lock (_root)
					_s.Push(obj);
			}

			public override object Pop()
			{
				lock (_root)
					return _s.Pop();
			}

			public override IEnumerator GetEnumerator()
			{
				lock (_root)
					return _s.GetEnumerator();
			}

			public override object Peek()
			{
				lock (_root)
					return _s.Peek();
			}

			public override object[] ToArray()
			{
				lock (_root)
					return _s.ToArray();
			}
		}

		/*Serializable*/

		public class StackEnumerator : IEnumerator, ICloneable
		{
			private Stack _stack;
			private int _index;
			private int _version;
			private object currentElement;

			internal StackEnumerator(Stack stack)
			{
				_stack = stack;
				_index = -2;
				_version = _stack._version;
				currentElement = (object)null;
			}

			public object Clone() => MemberwiseClone();

			public virtual bool MoveNext()
			{
				if (_version != _stack._version) /*Environment.GetResourceString*/
					throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
				if (_index == -2)
				{
					_index = _stack._size - 1;
					bool flag = _index >= 0;
					if (flag)
						currentElement = _stack._array[_index];
					return flag;
				}

				if (_index == -1) return false;
				bool flag1 = --_index >= 0;
				currentElement = !flag1 ? (object)null : _stack._array[_index];
				return flag1;
			}

			public virtual object Current
			{
				get
				{
					if (_index == -2)/*Environment.GetResourceString*/
						throw new InvalidOperationException("InvalidOperation_EnumNotStarted");
					if (_index == -1)/*Environment.GetResourceString*/
						throw new InvalidOperationException("InvalidOperation_EnumEnded");
					return currentElement;
				}
			}

			public virtual void Reset()
			{
				if (_version != _stack._version)/*Environment.GetResourceString*/
					throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
				_index = -2;
				currentElement = (object)null;
			}
		}

		/*
		 * StackDebugView Skipped, implement later.
		 */
	}
}
