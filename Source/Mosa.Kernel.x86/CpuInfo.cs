/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{
	public class CpuInfo
	{
	
		public ulong NumberOfCores
		{
			get
			{
				return (ulong)((Native.CpuIdEax(4) >> 26) + 1);
			}
		}

		public ulong Type
		{
			get
			{
				return (ulong)((Native.CpuIdEax(1) & 0x3000) >> 12);
			}
		}

		public ulong Stepping
		{
			get
			{
				return (ulong)(Native.CpuIdEax(1) & 0xF);
			}
		}

		public ulong Model
		{
			get
			{
				return (ulong)((Native.CpuIdEax(1) & 0xF0) >> 4);
			}
		}

		public ulong Family
		{
			get
			{
				return (ulong)((Native.CpuIdEax(1) & 0xF00) >> 8);
			}
		}

		public bool SupportsExtendedCpuid
		{
			get
			{
				uint identifier = (uint)Native.CpuIdEax(0x80000000);
				return (identifier & 0x80000000) != 0;
			}
		}

		public bool SupportsBrandString
		{
			get
			{
				uint identifier = (uint)Native.CpuIdEax(0x80000000);
				return identifier >= 0x80000004U;
			}
		}

		public void PrintVendorString(ConsoleSession console)
		{
			int identifier = Native.CpuIdEbx(0);
			for (int i = 0; i < 4; ++i)
				console.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Native.CpuIdEdx(0);
			for (int i = 0; i < 4; ++i)
				console.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Native.CpuIdEcx(0);
			for (int i = 0; i < 4; ++i)
				console.Write((char)((identifier >> (i * 8)) & 0xFF));
		}

		public void PrintBrandString(ConsoleSession console)
		{
			if (SupportsBrandString)
			{
				PrintBrand(console,(uint)0x80000002);
				PrintBrand(console, (uint)0x80000003);
				PrintBrand(console, (uint)0x80000004);
				return;
			}

			console.Write(@"Unknown (Generic x86)");
		}

		private void PrintBrand(ConsoleSession console, uint param)
		{
			int identifier = Native.CpuIdEax(param);
			bool whitespace = true;
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(console, identifier, i, ref whitespace);

			identifier = Native.CpuIdEbx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(console, identifier, i, ref whitespace);

			identifier = Native.CpuIdEcx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(console, identifier, i, ref whitespace);

			identifier = Native.CpuIdEdx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(console, identifier, i, ref whitespace);
		}

		private void PrintBrandPart(ConsoleSession console, int identifier, int i, ref bool whitespace)
		{
			char character = (char)((identifier >> (i * 8)) & 0xFF);

			if (whitespace && character == ' ')
				return;
			if (whitespace && character != ' ')
			{
				console.Write(character);
				whitespace = false;
				return;
			}
			if (character == ' ')
				whitespace = true;
			console.Write(character);
		}
	}
}