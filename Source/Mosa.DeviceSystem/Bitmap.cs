// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public static class Bitmap
{
	public static FrameBuffer32 CreateImage(byte[] data)
	{
		if (data[0] != (byte)'B' || data[1] != (byte)'M')
			return null;

		var bytesPerPixel = (uint)(data[0x1C] / 8);

		if (bytesPerPixel != 3 && bytesPerPixel != 4)
			return null;

		var stream = new DataBlock(data);

		var width = stream.GetUInt32(0x12);
		var height = stream.GetUInt32(0x16);
		var dataSectionOffset = stream.GetUInt32(0xA);

		var buffer = new FrameBuffer32(HAL.AllocateMemory(width * height * bytesPerPixel, 0), width, height, (x, y) => (y * width + x) * 4);

		do
		{
			height--;

			for (uint x = 0; x < width; x++)
			{
				var color = bytesPerPixel == 4 ? stream.GetUInt32(dataSectionOffset) : stream.GetUInt24(dataSectionOffset) | 0xFF000000;
				buffer.SetPixel(color, x, height);

				dataSectionOffset += bytesPerPixel;
			}

			if (bytesPerPixel == 3 && width * bytesPerPixel % 4 > 0)
				dataSectionOffset += 4 - width * bytesPerPixel % 4;

		} while (height > 0);

		return buffer;
	}
}
