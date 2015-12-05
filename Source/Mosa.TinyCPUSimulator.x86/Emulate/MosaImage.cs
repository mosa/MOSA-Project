// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	public class MosaImage : BaseSimDevice
	{
		public static readonly uint BaseAddress = 0x00400000;
		public static readonly uint ImageSize = 0x00400000;

		public MosaImage(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override void Initialize()
		{
			var x86 = simCPU as CPUx86;

			simCPU.AddMemory(BaseAddress, ImageSize, 1);
		}

		public override BaseSimDevice Clone(SimCPU simCPU)
		{
			return new MosaImage(simCPU);
		}
	}
}
