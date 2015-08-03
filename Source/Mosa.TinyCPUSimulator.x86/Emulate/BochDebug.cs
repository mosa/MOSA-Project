// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	/// <summary>
	/// Represents an emulated Boch debug port
	/// </summary>
	public class BochDebug : BaseSimDevice
	{
		public const ushort StandardIOBase = 0xE9;

		protected readonly TextWriter debug;

		/// <summary>
		/// Initializes a new instance of the <see cref="BochDebug"/> class.
		/// </summary>
		/// <param name="simCPU">The sim cpu.</param>
		public BochDebug(SimCPU simCPU)
			: base(simCPU)
		{
			debug = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BochDebug"/> class.
		/// </summary>
		/// <param name="simCPU">The sim cpu.</param>
		/// <param name="output">The output.</param>
		public BochDebug(SimCPU simCPU, TextWriter output)
			: base(simCPU)
		{
			debug = output;
		}

		public override ushort[] GetPortList()
		{
			return GetPortList(StandardIOBase, 1);
		}

		public override void MemoryWrite(ulong address, byte size)
		{
			return;
		}

		public override void PortWrite(uint port, byte value)
		{
			if (debug != null)
			{
				debug.WriteLine("0x" + value.ToString("X2") + " " + value.ToString());
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("0x" + value.ToString("X2") + " " + value.ToString());
			}
		}
	}
}