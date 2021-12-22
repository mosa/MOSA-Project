using System;
using System.Runtime.InteropServices;
using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.PCI.Intel
{
	// Only works on VirtualBox for now!
	//https://github.com/nifanfa/MOSA-Core/blob/master/Mosa/Mosa.External.x86/Driver/Audio/AC97.cs
	public class AC97 : BaseDeviceDriver
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct BufferDescriptor
		{
			public uint Addr;
			public ushort Size;
			public ushort Attr;
		}

		private ConstrainedPointer NAM, NABM, BufferListAddr, Buffer;

		private const ushort ListLength = 32;
		private const ushort BufferLength = 0xFFFE;

		private int Status;

		private bool Ready;

		public override void Initialize()
		{
			NAM = Device.Resources.GetMemory(0);
			NABM = Device.Resources.GetMemory(1);

			NABM.Write32(0x2C, 0x00000002);
			NAM.Write16(0, 54188);

			unsafe { BufferListAddr = HAL.AllocateMemory((uint)(ListLength * sizeof(BufferDescriptor)), 0); }
			Buffer = HAL.AllocateMemory(1024 * 1024, 0);

			NAM.Write16(0x02, 0x0F0F);
			NAM.Write16(0x18, 0x0F0F);
			NAM.Write16(0x2C, 48000);

			Ready = true;
		}

		public override bool OnInterrupt()
		{
			if (!Ready)
				return false;

			Status = NABM.Read16(0x16);
			if (Status != 0)
				NABM.Write16(0x16, (ushort)(Status & 0x1E));

			return true;
		}

		// 48 KHz stereo PCM (dual channel)
		public void Play(ConstrainedPointer data)
		{
			if (!Ready)
				return;

			var k = 0;

			Internal.MemoryCopy(Buffer.Address, data.Address, data.Size);

			var size = Math.Clamp(data.Size, 0, Buffer.Size);
			for (uint i = 0; i < size - size % BufferLength; i += BufferLength * 2)
			{
				unsafe
				{
					var desc = (BufferDescriptor*)(BufferListAddr.Address + sizeof(BufferDescriptor) * k);
					desc->Addr = (uint)(Buffer.Address + i);
					desc->Size = BufferLength;
					desc->Attr = 0b0000_0000_0000_0011;
				}
				k++;
			}

			if (k > 0)
				k--;

			NABM.Write8(0x1B, 0x2);

			NABM.Write32(0x10, (uint)BufferListAddr.Address);

			// Set index
			NABM.Write8(0x15, (byte)(k & 0xFF));

			// Play sound
			NABM.Write8(0x1B, 0x11);
		}
	}
}
