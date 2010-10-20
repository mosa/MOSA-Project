using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Permutation
	{

		public static IList<string[]> GetPermutation(IList<string>[] lists)
		{
			// This is hard... so fake it!

			if (lists.Length == 1)
				return GetPermutation(lists[0]);
			else if (lists.Length == 2)
				return GetPermutation(lists[0], lists[1]);
			else if (lists.Length == 3)
				return GetPermutation(lists[0], lists[1], lists[2]);
			else if (lists.Length == 4)
				return GetPermutation(lists[0], lists[1], lists[2], lists[3]);
			else if (lists.Length == 5)
				return GetPermutation(lists[0], lists[1], lists[2], lists[3], lists[4]);
			else
				throw new Exception("No go!");
		}

		public static IList<string[]> GetPermutation(IList<string> alist)
		{
			IList<string[]> results = new List<string[]>();

			foreach (string a in alist)
				results.Add(new string[1] { a });

			return results;
		}

		public static IList<string[]> GetPermutation(IList<string> alist, IList<string> blist)
		{
			IList<string[]> results = new List<string[]>();

			foreach (string a in alist)
				foreach (string b in blist)
					results.Add(new string[2] { a, b });

			return results;
		}

		public static IList<string[]> GetPermutation(IList<string> alist, IList<string> blist, IList<string> clist)
		{
			IList<string[]> results = new List<string[]>();

			foreach (string a in alist)
				foreach (string b in blist)
					foreach (string c in clist)
						results.Add(new string[3] { a, b, c });

			return results;
		}

		public static IList<string[]> GetPermutation(IList<string> alist, IList<string> blist, IList<string> clist, IList<string> dlist)
		{
			IList<string[]> results = new List<string[]>();

			foreach (string a in alist)
				foreach (string b in blist)
					foreach (string c in clist)
						foreach (string d in dlist)
							results.Add(new string[4] { a, b, c, d });

			return results;
		}

		public static IList<string[]> GetPermutation(IList<string> alist, IList<string> blist, IList<string> clist, IList<string> dlist, IList<string> elist)
		{
			IList<string[]> results = new List<string[]>();

			foreach (string a in alist)
				foreach (string b in blist)
					foreach (string c in clist)
						foreach (string d in dlist)
							foreach (string e in elist)
								results.Add(new string[5] { a, b, c, d, e });

			return results;
		}

		public static void Dump(IList<string[]> p)
		{

			foreach (string[] valuelist in p)
			{
				foreach (string value in valuelist)
				{
					Console.Write(value);
					Console.Write(' ');
				}

				Console.WriteLine();
			}
		}
	}
}
