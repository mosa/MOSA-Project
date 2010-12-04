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

namespace Test.Mosa.Runtime.CompilerFramework.Numbers
{
	public class U2
	{
		private static IList<ushort> samples = null;
		public static IList<ushort> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<ushort> Samples
		{
			get
			{
				foreach (ushort value in SampleData)
					yield return value;
			}
		}

		public static IList<ushort> GetSamples()
		{
			List<ushort> list = new List<ushort>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(byte.MinValue);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue + 1);
			list.Add(byte.MaxValue - 1);
			list.Add(ushort.MinValue);
			list.Add(ushort.MaxValue);
			list.Add(ushort.MinValue + 1);
			list.Add(ushort.MaxValue - 1);

			list.Sort();

			return list;
		}

	}
}
