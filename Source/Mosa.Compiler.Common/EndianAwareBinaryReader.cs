/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Compiler.Common
{

	public class EndianAwareBinaryReader : BinaryReader
	{

		private bool swap = false;

		public EndianAwareBinaryReader(Stream input, bool isLittleEndian)
			: base(input)
		{
			swap = (isLittleEndian != Endian.NativeIsLittleEndian);
		}

		public override ushort ReadUInt16()
		{
			ushort value = base.ReadUInt16();
			return swap ? Endian.Swap(value) : value;
		}

		public override uint ReadUInt32()
		{
			uint value = base.ReadUInt32();
			return swap ? Endian.Swap(value) : value;
		}
	}

}
