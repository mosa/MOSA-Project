// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaArrayInfo : IEquatable<MosaArrayInfo>
	{
		public static readonly MosaArrayInfo Vector = new MosaArrayInfo(new List<int>(), 1, new List<uint>());

		[NotNull]
		public IList<int>? LowerBounds { get; }

		public uint Rank { get; }

		[NotNull]
		public IList<uint>? Sizes { get; }

		public MosaArrayInfo(IList<int> lowerBounds, uint rank, IList<uint> sizes)
		{
			if (rank == 0)   // Should not be zero
				return;
			LowerBounds = new List<int>(lowerBounds).AsReadOnly();
			Rank = rank;
			Sizes = new List<uint>(sizes).AsReadOnly();
		}

		public bool Equals(MosaArrayInfo? other)
		{
			if (this == Vector && other == Vector)
				return true;

			return Rank == other?.Rank &&
				   LowerBounds.SequenceEquals(other.LowerBounds) &&
				   Sizes.SequenceEqual(other.Sizes);
		}

		public override string ToString()
		{
			if (this == Vector)
				return "[]";

			var builder = new StringBuilder();
			builder.Append('[');

			for (var i = 0; i < Rank; i++)
			{
				if (i != 0) builder.Append(',');
				if (!(i < LowerBounds?.Count)) continue;

				builder.Append(LowerBounds[i]);
				builder.Append("..");

				if (i < Sizes?.Count)
				{
					builder.Append(LowerBounds[i]);
					builder.Append(Sizes[i]);
				}
				else builder.Append('.');
			}

			builder.Append(']');
			return builder.ToString();
		}
	}
}
