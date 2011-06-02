/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Mosa.Test.System.Numbers
{
	public static class C
	{
		private static IList<char> series = null;

		public static IEnumerable<char> Series
		{
			get
			{
				if (series == null) series = GetSeries();

				foreach (char value in series)
					yield return value;
			}
		}

		public static IList<char> GetSeries()
		{
			List<char> list = new List<char>();

			list.Add((char)1);
			list.Add((char)2);
			list.Add((char)32);
			list.Add((char)127);
			list.Add((char)255);
			list.Add((char)256);
			list.Add(char.MinValue);
			list.Add(char.MaxValue);
			list.Add('0');
			list.Add('9');
			list.Add('A');
			list.Add('Z');
			list.Add('a');
			list.Add('z');
			list.Add(' ');
			list.Add('\n');
			list.Add('\t');

			list.Sort();

			return list;
		}

	}
}
