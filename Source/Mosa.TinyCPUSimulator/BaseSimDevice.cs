// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public abstract class BaseSimDevice
	{
		protected SimCPU simCPU;

		public bool IsMemoryMonitor { get; protected set; }

		public BaseSimDevice(SimCPU simCPU)
		{
			this.simCPU = simCPU;
			IsMemoryMonitor = false;
		}

		public virtual void Initialize()
		{
		}

		public virtual void Reset()
		{
		}

		public virtual void MemoryWrite(ulong address, byte size)
		{
		}

		public virtual void PortWrite(uint port, byte value)
		{
		}

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
