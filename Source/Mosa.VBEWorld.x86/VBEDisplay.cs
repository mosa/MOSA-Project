// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.VBEWorld.x86
{
	public static class VBEDisplay
	{
		private static ConstrainedPointer _lfb;

		public static IFrameBuffer Framebuffer { get; set; }

		public static bool InitVBE(BaseHardwareAbstraction hal)
		{
			if (!VBE.IsVBEAvailable)
				return false;

			uint memorySize = (uint)(VBE.ScreenWidth * VBE.ScreenHeight * (VBE.BitsPerPixel / 8));
			_lfb = hal.GetPhysicalMemory(VBE.MemoryPhysicalLocation, memorySize);

			switch (VBE.BitsPerPixel)
			{
				case 8: Framebuffer = new FrameBuffer8bpp(_lfb, VBE.ScreenWidth, VBE.ScreenHeight, 0, VBE.Pitch); break;
				case 16: Framebuffer = new FrameBuffer16bpp(_lfb, VBE.ScreenWidth, VBE.ScreenHeight, 0, VBE.Pitch); break;
				case 24: Framebuffer = new FrameBuffer24bpp(_lfb, VBE.ScreenWidth, VBE.ScreenHeight, 0, VBE.Pitch); break;
				case 32: Framebuffer = new FrameBuffer32bpp(_lfb, VBE.ScreenWidth, VBE.ScreenHeight, 0, VBE.Pitch); break;

				default:
					return false;
			}

			return true;
		}
	}
}
