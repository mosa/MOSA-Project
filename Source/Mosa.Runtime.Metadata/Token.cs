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

	public struct Token
	{
		readonly uint token;

		public int RID
		{
			get { return (int)(token & 0x00ffffff); }
		}

		public TableTypes Table
		{
			get { return (TableTypes)(token & 0xff000000); }
		}

		public static readonly Token Zero = new Token((uint)0);

		public Token(uint token)
		{
			this.token = token;
		}

		public Token(TableTypes type)
			: this(type, 0)
		{
		}

		public Token(TableTypes type, uint rid)
		{
			token = (uint)type | rid;
		}

		public Token(TableTypes type, int rid)
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
			if (obj is Token)
			{
				var other = (Token)obj;
				return other.token == token;
			}

			return false;
		}

		public static bool operator ==(Token one, Token other)
		{
			return one.token == other.token;
		}

		public static bool operator !=(Token one, Token other)
		{
			return one.token != other.token;
		}

		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", Table, RID.ToString("x4"));
		}

		public Token NextRow
		{
			get
			{
				return new Token(token + 1);
			}
		}

		public Token PreviousRow
		{
			get
			{
				if (RID != 0)
					return new Token(token - 1);
				else
					return new Token(token);
			}
		}

		public System.Collections.Generic.IEnumerable<Token> Upto(Token last)
		{
			if (RID > last.RID)
				yield break;

			Token token = this;

			while (token != last)
			{
				yield return token;
				token = token.NextRow;
			}

			yield return token;
		}

	}
}
