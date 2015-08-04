// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.TinyCPUSimulator
{
	[Serializable]
	public class InvalidMemoryAccess : SimCPUException
	{
		public ulong Address { get; private set; }

		public InvalidMemoryAccess(ulong address)
		{
			this.Address = address;
		}

		public override string ToString()
		{
			return "Invalid Memory Access at 0x" + Address.ToString("X");
		}
	}
}
