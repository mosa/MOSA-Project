/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.TinyCPUSimulator
{
	[Serializable]
	public class PageFaultException : SimCPUException
	{
		public ulong Address { get; private set; }

		public PageFaultException(ulong address)
		{
			this.Address = address;
		}

		public override string ToString()
		{
			return "Invalid Memory Access at 0x" + Address.ToString("X");
		}
	}
}