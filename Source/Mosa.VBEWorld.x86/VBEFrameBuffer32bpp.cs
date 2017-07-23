using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime;

namespace Mosa.VBEWorld.x86
{
	public unsafe class VBEFrameBuffer32bpp : IFrameBuffer
	{
		public uint Width { get; private set; }
		public uint Height { get; private set; }

		private int _pixelsPerLine;

		private uint* _memory;
		private uint _memorySize;

		public VBEFrameBuffer32bpp(VBEMode mode)
		{
			Width = mode.ScreenWidth;
			Height = mode.ScreenHeight;

			_pixelsPerLine = mode.Pitch / (mode.BitsPerPixel / 8);

			_memory = (uint*)mode.MemoryPhysicalLocation;

			//Map the LFB
			uint lfbLocation = mode.MemoryPhysicalLocation;
			_memorySize = (uint)(mode.ScreenWidth * mode.ScreenHeight * (mode.BitsPerPixel / 8));

			uint pageSize = 0x1000; //???

			uint pages = _memorySize / pageSize;
			for (uint x = 0; x < pages; x++)
			{
				PageTable.MapVirtualAddressToPhysical((uint)(lfbLocation + pageSize * x), (uint)(lfbLocation + pageSize * x), true);
			}
		}

		public uint GetPixel(uint x, uint y)
		{
			return _memory[x + _pixelsPerLine * y];
		}

		public void SetPixel(uint color, uint x, uint y)
		{
			_memory[x + _pixelsPerLine * y] = color;
		}

		//Non IFrameBuffer functions
		public void Clear()
		{
			Internal.MemorySet((void*)_memory, 0x00, _memorySize);
		}

		public void FillRectangle(uint color, uint x, uint y, uint w, uint h)
		{
			uint startAddress = (uint)(x + _pixelsPerLine * y);

			for (uint offsetY = 0; offsetY < h; offsetY++)
			{
				for (uint offsetX = 0; offsetX < w; offsetX++)
				{
					_memory[startAddress + offsetX] = color;
				}

				startAddress += (uint)_pixelsPerLine;
			}
		}
	}
}
