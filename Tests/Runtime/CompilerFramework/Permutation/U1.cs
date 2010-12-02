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

namespace Test.Mosa.Runtime.CompilerFramework.Permutation
{
	public class U1
	{
		private static IList<byte> samples = null;
		public static IList<byte> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<byte> Samples
		{
			get
			{
				foreach (byte value in SampleData)
					yield return value;
			}
		}

		public static IList<byte> GetSamples()
		{
			List<byte> list = new List<byte>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(byte.MinValue);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue + 1);
			list.Add(byte.MaxValue - 1);

			//list.Sort();

			return list;
		}

	}
}
