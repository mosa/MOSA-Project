/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
	public abstract class BaseSimDevice
	{
		protected SimCPU simCPU;

		public BaseSimDevice(SimCPU simCPU)
		{
			this.simCPU = simCPU;
		}

		public abstract void Initialize();

		public abstract void Reset();

		public abstract void MemoryWrite(ulong address, byte size);

		public abstract void PortWrite(uint port, byte value);

		public virtual byte PortRead(uint port)
		{
			return 0;
		}

		public virtual ushort[] GetPortList()
		{
			return null;
		}

		static protected ushort[] GetPortList(ushort start, ushort count)
		{
			ushort[] list = new ushort[count];

			for (int i = 0; i < count; i++)
				list[i] = (ushort)(start + i);

			return list;
		}
	}
}