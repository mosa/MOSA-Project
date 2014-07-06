/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaArrayInfo : IEquatable<MosaArrayInfo>
	{
		public static readonly MosaArrayInfo Vector = new MosaArrayInfo(null, 0, null);

		public IList<int> LowerBounds { get; private set; }

		public uint Rank { get; private set; }

		public IList<uint> Sizes { get; private set; }

		public MosaArrayInfo(IList<int> lowerBounds, uint rank, IList<uint> sizes)
		{
			if (rank == 0)   // Should not be zero
				return;
			LowerBounds = new List<int>(lowerBounds).AsReadOnly();
			Rank = rank;
			Sizes = new List<uint>(sizes).AsReadOnly();
		}

		public bool Equals(MosaArrayInfo info)
		{
			if (this == Vector && info == Vector)
				return true;

			return Rank == info.Rank &&
				   this.LowerBounds.SequenceEquals(info.LowerBounds) &&
				   this.Sizes.SequenceEqual(info.Sizes);
		}

		private string sig;

		public override string ToString()
		{
			if (this == Vector)
				return "[]";
			else if (sig == null)
			{
				StringBuilder result = new StringBuilder();
				result.Append("[");
				for (int i = 0; i < Rank; i++)
				{
					if (i != 0)
						result.Append(",");
					if (i < LowerBounds.Count)
					{
						result.Append(LowerBounds[i]);
						result.Append("..");
						if (i < Sizes.Count)
							result.Append(LowerBounds[i] + Sizes[i]);
						else
							result.Append(".");
					}
				}
				result.Append("]");
				sig = result.ToString();
			}

			return sig;
		}
	}
}