using Mosa.DeviceSystem;
using Mosa.HardwareSystem;
using Mosa.Kernel.x86;

namespace Mosa.VBEWorld.x86
{
	public static class VBE
	{
		private static IMemory _lfb;

		public static IFrameBuffer Framebuffer { get; set;  }

		public static bool InitVBE(IHardwareAbstraction hal)
		{
			if (!Multiboot.IsMultibootEnabled)
				return false;

			if (!Multiboot.VBEPresent)
				return false;

			VBEMode vbeInfo = Multiboot.VBEModeInfoStructure;

			uint memorySize = (uint)(vbeInfo.ScreenWidth * vbeInfo.ScreenHeight * (vbeInfo.BitsPerPixel / 8));
			_lfb = hal.RequestPhysicalMemory(vbeInfo.MemoryPhysicalLocation, memorySize);

			switch(vbeInfo.BitsPerPixel)
			{
				case 8: Framebuffer = new FrameBuffer8bpp(_lfb, vbeInfo.ScreenWidth, vbeInfo.ScreenHeight, 0, vbeInfo.Pitch); break;
				case 16: Framebuffer = new FrameBuffer16bpp(_lfb, vbeInfo.ScreenWidth, vbeInfo.ScreenHeight, 0, vbeInfo.Pitch); break;
				case 24: Framebuffer = new FrameBuffer24bpp(_lfb, vbeInfo.ScreenWidth, vbeInfo.ScreenHeight, 0, vbeInfo.Pitch); break;
				case 32: Framebuffer = new FrameBuffer32bpp(_lfb, vbeInfo.ScreenWidth, vbeInfo.ScreenHeight, 0, vbeInfo.Pitch); break;

				default:
					return false;
			}

			return true;
		}
	}
}
