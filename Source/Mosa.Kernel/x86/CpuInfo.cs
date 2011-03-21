/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.x86.Intrinsic;
using Mosa.Kernel.x86;

namespace Mosa.HelloWorld
{
	public class CpuInfo
	{
		public CpuInfo()
		{
			Setup();
		}

		public void Setup()
		{

		}

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

		public void PrintVendorString()
		{
			int identifier = Native.CpuIdEbx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Native.CpuIdEdx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Native.CpuIdEcx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));
		}

		public void PrintBrandString()
		{
			if (SupportsBrandString)
			{
				PrintBrand((uint)0x80000002);
				PrintBrand((uint)0x80000003);
				PrintBrand((uint)0x80000004);
				return;
			}

			Screen.Write(@"Unknown (Generic x86)");
		}

		private void PrintBrand(uint param)
		{
			int identifier = Native.CpuIdEax(param);
			bool whitespace = true;
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(identifier, i, ref whitespace);


			identifier = Native.CpuIdEbx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(identifier, i, ref whitespace);

			identifier = Native.CpuIdEcx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(identifier, i, ref whitespace);

			identifier = Native.CpuIdEdx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					PrintBrandPart(identifier, i, ref whitespace);
		}

		private void PrintBrandPart(int identifier, int i, ref bool whitespace)
		{
			char character = (char)((identifier >> (i * 8)) & 0xFF);

			if (whitespace && character == ' ')
				return;
			if (whitespace && character != ' ')
			{
				Screen.Write(character);
				whitespace = false;
				return;
			}
			if (character == ' ')
				whitespace = true;
			Screen.Write(character);
		}
	}
}