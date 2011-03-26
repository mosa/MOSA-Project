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

		public uint RID
		{
			get { return token & 0x00ffffff; }
		}

		public TableTypes Table
		{
			get { return (TableTypes)(token & 0xff000000); }
		}

		public static readonly MetadataToken Zero = new MetadataToken((uint)0);

		public MetadataToken(uint token)
		{
			this.token = token;
		}

		public MetadataToken(TableTypes type)
			: this(type, 0)
		{
		}

		public MetadataToken(TableTypes type, uint rid)
		{
			token = (uint)type | rid;
		}

		public MetadataToken(TableTypes type, int rid)
		{
			token = (uint)type | (uint)rid;
		}

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
	}
}
