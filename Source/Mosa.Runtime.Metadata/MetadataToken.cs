/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Metadata
{

	public struct MetadataToken
	{
		readonly uint token;

		public int RID
		{
			get { return (int)(token & 0x00ffffff); }
		}

		public MetadataTable Table
		{
			get { return (MetadataTable)(token & 0xff000000); }
		}

		public static readonly MetadataToken Zero = new MetadataToken((uint)0);

		public MetadataToken(uint token)
		{
			this.token = token;
		}

		public MetadataToken(MetadataTable type)
			: this(type, 0)
		{
		}

		public MetadataToken(MetadataTable type, uint rid)
		{
			token = (uint)type | rid;
		}

		public MetadataToken(MetadataTable type, int rid)
		{
			token = (uint)type | (uint)rid;
		}

		//public MetadataToken(IndexType index, int value)
		//{
		//    int bits = IndexBits[(int)index];
		//    int mask = 1;

		//    for (int i = 1; i < bits; i++) mask = (mask << 1) | 1;

		//    // Get the table
		//    int table = (int)value & mask;

		//    // Correct the value
		//    value = ((int)value >> bits);

		//    token = (uint)IndexTables2[(int)index][table] | (uint)value;
		//}

		public int ToInt32()
		{
			return (int)token;
		}

		public uint ToUInt32()
		{
			return token;
		}

		public override int GetHashCode()
		{
			return (int)token;
		}

		public override bool Equals(object obj)
		{
			if (obj is MetadataToken)
			{
				var other = (MetadataToken)obj;
				return other.token == token;
			}

			return false;
		}

		public static bool operator ==(MetadataToken one, MetadataToken other)
		{
			return one.token == other.token;
		}

		public static bool operator !=(MetadataToken one, MetadataToken other)
		{
			return one.token != other.token;
		}

		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", Table, RID.ToString("x4"));
		}

		public MetadataToken NextRow
		{
			get
			{
				return new MetadataToken(token + 1);
			}
		}

		public MetadataToken PreviousRow
		{
			get
			{
				if (RID != 0)
					return new MetadataToken(token - 1);
				else
					return new MetadataToken(token);
			}
		}

		public System.Collections.Generic.IEnumerable<MetadataToken> Upto(MetadataToken last)
		{
			if (RID > last.RID)
				yield break;

			MetadataToken token = this;

			while (token != last)
			{
				yield return token;
				token = token.NextRow;
			}

			yield return token;
		}

	}
}
