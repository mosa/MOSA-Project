// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	public class Counter
	{
		public readonly string Name;
		public int Count = 0;

		public Counter(string name)
		{
			Name = name;
		}

		public void Increment()
		{
			Count++;
		}

		public void Reset()
		{
			Count = 0;
		}

		public void Set(int count)
		{
			Count = count;
		}

		public static Counter operator ++(Counter counter)
		{
			counter.Count++;
			return counter;
		}
	}
}
