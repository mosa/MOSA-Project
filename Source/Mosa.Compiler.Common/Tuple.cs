// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common
{
	public class Tuple<T1, T2>
	{
		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public T1 Item1 { get; private set; }

		public T2 Item2 { get; private set; }

		public override bool Equals(object obj)
		{
			Tuple<T1, T2> other = obj as Tuple<T1, T2>;
			if (other == null) return false;
			return object.Equals(Item1, other.Item1) && object.Equals(Item2, other.Item2);
		}

		public override int GetHashCode()
		{
			int hash1 = EqualityComparer<T1>.Default.GetHashCode(Item1);
			int hash2 = EqualityComparer<T2>.Default.GetHashCode(Item2);
			return hash1 * 7 + hash2;
		}

		public override string ToString()
		{
			return string.Format("{{{0}, {1}}}", Item1, Item2);
		}
	}

	public class Tuple<T1, T2, T3>
	{
		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		public T1 Item1 { get; private set; }

		public T2 Item2 { get; private set; }

		public T3 Item3 { get; private set; }

		public override bool Equals(object obj)
		{
			Tuple<T1, T2, T3> other = obj as Tuple<T1, T2, T3>;
			if (other == null) return false;
			return object.Equals(Item1, other.Item1) && object.Equals(Item2, other.Item2) && object.Equals(Item3, other.Item3);
		}

		public override int GetHashCode()
		{
			int hash1 = EqualityComparer<T1>.Default.GetHashCode(Item1);
			int hash2 = EqualityComparer<T2>.Default.GetHashCode(Item2);
			int hash3 = EqualityComparer<T3>.Default.GetHashCode(Item3);
			return (((hash1 * 7) ^ hash2) * 7) ^ hash3;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2})", Item1, Item2, Item3);
		}
	}

	public class Tuple<T1, T2, T3, T4>
	{
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}

		public T1 Item1 { get; private set; }

		public T2 Item2 { get; private set; }

		public T3 Item3 { get; private set; }

		public T4 Item4 { get; private set; }

		public override bool Equals(object obj)
		{
			Tuple<T1, T2, T3, T4> other = obj as Tuple<T1, T2, T3, T4>;
			if (other == null) return false;
			return object.Equals(Item1, other.Item1) && object.Equals(Item2, other.Item2) && object.Equals(Item3, other.Item3) && object.Equals(Item4, other.Item4);
		}

		public override int GetHashCode()
		{
			int hash1 = EqualityComparer<T1>.Default.GetHashCode(Item1);
			int hash2 = EqualityComparer<T2>.Default.GetHashCode(Item2);
			int hash3 = EqualityComparer<T3>.Default.GetHashCode(Item3);
			int hash4 = EqualityComparer<T4>.Default.GetHashCode(Item4);
			return (((((hash1 * 7) ^ hash2) * 7) ^ hash3) * 7) ^ hash4;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3})", Item1, Item2, Item3, Item4);
		}
	}

	public static class Tuple
	{
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}
	}
}
