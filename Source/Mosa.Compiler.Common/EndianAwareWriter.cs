/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

namespace Mosa.Compiler.Common
{

	public class EndianAwareWriter : BinaryWriter
	{

		private bool swap = false;

		public EndianAwareWriter(Stream input, bool isLittleEndian)
			: base(input)
		{
			swap = (isLittleEndian != Endian.NativeIsLittleEndian);
		}

		public override void Write(ushort value)
		{
			value = swap ? Endian.Swap(value) : value;
			base.Write(value);
		}

		public override void Write(uint value)
		{
			value = swap ? Endian.Swap(value) : value;
			base.Write(value);
		}
	}

}
