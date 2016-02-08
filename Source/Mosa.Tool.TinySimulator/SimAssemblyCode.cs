// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using Mosa.TinyCPUSimulator;

namespace Mosa.Tool.TinySimulator
{
	public class SimAssemblyCode : IAssemblyCode
	{
		public SimCPU simCPU;
		public ulong offset;    // hack until next version of SharpDism

		public SimAssemblyCode(SimCPU simCPU, ulong offset = 0)
		{
			this.simCPU = simCPU;
			this.offset = offset;
		}

		byte IAssemblyCode.this[int index]
		{
			get { return simCPU.DirectRead8((ulong)index + offset); }
		}

		int IAssemblyCode.Length
		{
			get { return int.MaxValue; }
		}
	}
}
