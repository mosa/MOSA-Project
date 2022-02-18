using Mosa.DeviceSystem;
using System.Runtime.InteropServices;

//https://github.com/nifanfa/Solution1/blob/multiboot/Kernel/Driver/AC97.cs
namespace Mosa.DeviceDriver.PCI.Intel
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct BufferDescriptor
	{
		public uint Address;
		public ushort SampleRate;
		public ushort Attribute;
	}

	public class AC97 : BaseDeviceDriver
	{
		private ConstrainedPointer NAM, NABM;
		private BufferDescriptor[] BufferDescriptors;

		private const int NumDescriptors = 31;

		private byte Index = 0;

		public override void Initialize()
		{
			Device.Name = "AC97";

			NAM = Device.Resources.GetMemory(0);
			NABM = Device.Resources.GetMemory(1);

			NABM.Write8(0x2C, 0x2);

			NAM.Write32(0, 0x6166696E);

			NAM.Write16(0x02, 0x0F0F);
			NAM.Write16(0x018, 0x0F0F);

			BufferDescriptors = new BufferDescriptor[NumDescriptors + 1];
		}

		public override bool OnInterrupt()
		{
			var status = NABM.Read16(0x16);
			if ((status & (1 << 3)) != 0)
			{
				NABM.Write8(0x15, Index++);

				if (Index > NumDescriptors)
					Index = 0;
			}

			return true;
		}

		public unsafe void Play(byte[] data, ushort sampleRate = 48000, bool stereo = true)
		{
			var index = 0;

			fixed (byte* buffer = data)
			{
				for (var i = 0; i < data.Length; i += sampleRate * (stereo ? 2 : 1))
				{
					BufferDescriptors[index].Address = (uint)(buffer + i);
					BufferDescriptors[index].SampleRate = sampleRate;
					BufferDescriptors[index].Attribute = 1 << 15;

					if (i + sampleRate > data.Length || index > NumDescriptors)
					{
						BufferDescriptors[index].Attribute |= 1 << 14;
						break;
					}

					index++;
				}
			}

			NABM.Write8(0x1B, 0x02);

			fixed (BufferDescriptor* ptr = BufferDescriptors)
				NABM.Write32(0x10, (uint)ptr);

			NABM.Write8(0x15, 0);
			NABM.Write8(0x1B, 0x01);
		}
	}
}
