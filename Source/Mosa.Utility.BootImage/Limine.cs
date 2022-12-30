// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Linq;
using Mosa.DeviceSystem;
using Mosa.Utility.BootImage.Properties;

namespace Mosa.Utility.BootImage
{
	//https://github.com/limine-bootloader/limine/blob/v3.0-binary/limine-deploy.c
	public static class Limine
	{
		enum CacheState { Clean, Dirty }

		private static FileStream Stream;
		private static DataBlock DataCache;
		private static ulong CachedBlock;
		private static CacheState State;

		private const int BlockSize = 512;

		// TODO: Fix for VDI
		public static void Deploy(string path, bool vdi)
		{
			Stream = File.Open(path, FileMode.Open);
			DataCache = new DataBlock(new byte[BlockSize]);
			CachedBlock = 0;
			State = CacheState.Clean;

			Stream.Read(DataCache.Data);

			// To not interfere with VDI header
			var mbrLoc = vdi ? 512 : 0;

			// Skip detecting if the device has a valid partition table, because we know it does

			var bootloader_img = Resources.bootloader;
			var stage2Size = bootloader_img.Length - 512;
			var stage2Sectors = DivRoundUp(stage2Size, 512);
			var stage2SizeB = (ushort)(stage2Sectors / 2 * 512);
			var stage2SizeA = (ushort)(stage2SizeB + ((stage2Sectors % 2 != 0) ? 512 : 0));

			// Default split of stage 2 for MBR (consecutive in post MBR gap)
			ulong stage2LocA = 512;
			var stage2LocB = stage2LocA + stage2SizeA;

			var origMBR = new byte[70];
			var timestamp = new byte[6];

			// Save original timestamp
			Read(timestamp, 218, 6);

			// Save the original partition table of the device
			Read(origMBR, 440, 70);

			// Write the bootsector from the bootloader to the device
			Write(bootloader_img, 0, 512);

			// Write the rest of stage 2 to the device
			Write(bootloader_img.Skip(512).ToArray(), stage2LocA, stage2SizeA);
			Write(bootloader_img.Skip(512 + stage2SizeA).ToArray(), stage2LocB, (uint)(stage2Size - stage2SizeA));

			// Hardcode in the bootsector the location of stage 2 halves
			WriteUShort(stage2SizeA, 0x1a4);
			WriteUShort(stage2SizeB, 0x1a4 + 2);
			WriteULong(stage2LocA, 0x1a4 + 4);
			WriteULong(stage2LocB, 0x1a4 + 12);

			// Write back timestamp
			Write(timestamp, 218, 6);

			// Write back the saved partition table to the device
			Write(origMBR, 440, 70);

			FlushCache();
			Stream.Flush();
			Stream.Close();
		}

		private static void Read(byte[] buffer, uint loc, uint count)
		{
			uint progress = 0;
			while (progress < count)
			{
				var prog = loc + progress;
				var block = prog / BlockSize;

				CacheBlock(block);

				var chunk = count - progress;
				var offset = prog % BlockSize;

				if (chunk > BlockSize - offset)
					chunk = BlockSize - offset;

				for (uint i = 0; i < chunk; i++)
					buffer[progress + i] = DataCache.GetByte(offset + i);

				progress += chunk;
			}
		}

		private static void Write(byte[] buffer, ulong loc, ulong count)
		{
			ulong progress = 0;
			while (progress < count)
			{
				var prog = loc + progress;
				var block = prog / BlockSize;

				CacheBlock(block);

				var chunk = count - progress;
				var offset = prog % BlockSize;

				if (chunk > BlockSize - offset)
					chunk = BlockSize - offset;

				for (ulong i = 0; i < chunk; i++)
					DataCache.SetByte((uint)(offset + i), buffer[progress + i]);

				State = CacheState.Dirty;
				progress += chunk;
			}
		}

		private static void WriteUShort(ushort num, ulong loc)
		{
			ulong progress = 0, count = sizeof(ushort);
			while (progress < count)
			{
				var prog = loc + progress;
				var block = prog / BlockSize;

				CacheBlock(block);

				var chunk = count - progress;
				var offset = prog % BlockSize;

				if (chunk > BlockSize - offset)
					chunk = BlockSize - offset;

				DataCache.SetUShort((uint)offset, num);

				State = CacheState.Dirty;
				progress += chunk;
			}
		}

		private static void WriteULong(ulong num, ulong loc)
		{
			ulong progress = 0, count = sizeof(ulong);
			while (progress < count)
			{
				var prog = loc + progress;
				var block = prog / BlockSize;

				CacheBlock(block);

				var chunk = count - progress;
				var offset = prog % BlockSize;

				if (chunk > BlockSize - offset)
					chunk = BlockSize - offset;

				DataCache.SetULong((uint)offset, num);

				State = CacheState.Dirty;
				progress += chunk;
			}
		}

		private static void CacheBlock(ulong block)
		{
			if (CachedBlock == block)
				return;

			if (State == CacheState.Dirty)
				FlushCache();

			Stream.Seek((long)(block * BlockSize), SeekOrigin.Begin);
			Stream.Read(DataCache.Data);

			CachedBlock = block;
		}

		private static void FlushCache()
		{
			if (State == CacheState.Clean)
				return;

			Stream.Seek((long)(CachedBlock * BlockSize), SeekOrigin.Begin);
			Stream.Write(DataCache.Data);

			State = CacheState.Clean;
		}

		private static int DivRoundUp(int a, int b)
		{
			return (a + (b - 1)) / b;
		}
	}
}
