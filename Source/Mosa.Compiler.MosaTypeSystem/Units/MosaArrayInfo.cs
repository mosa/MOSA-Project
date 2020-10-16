// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaArrayInfo : IEquatable<MosaArrayInfo>
	{
		public static readonly MosaArrayInfo Vector = new MosaArrayInfo(new List<int>(), 1, new List<uint>());

		public IList<int> LowerBounds { get; }

		public uint Rank { get; }

		public IList<uint> Sizes { get; }

		public MosaArrayInfo(IList<int> lowerBounds, uint rank, IList<uint> sizes)
		{
			if (rank == 0)   // Should not be zero
				return;
			LowerBounds = new List<int>(lowerBounds).AsReadOnly();
			Rank = rank;
			Sizes = new List<uint>(sizes).AsReadOnly();
		}

		public bool Equals(MosaArrayInfo other)
		{
			if (this == Vector && other == Vector)
				return true;

			return Rank == other.Rank &&
				   LowerBounds.SequenceEquals(other.LowerBounds) &&
				   Sizes.SequenceEqual(other.Sizes);
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
