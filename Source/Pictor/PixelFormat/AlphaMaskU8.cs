/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor.PixelFormat
{
	///<summary>
	///</summary>
	public interface IAlphaMask
	{
		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<returns></returns>
		byte Pixel(int x, int y);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="val"></param>
		///<returns></returns>
		byte CombinePixel(int x, int y, byte val);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="dst"></param>
		///<param name="numPixel"></param>
		unsafe void FillHorizontalSpan(int x, int y, byte* dst, int numPixel);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="dst"></param>
		///<param name="numPixel"></param>
		unsafe void FillVerticalSpan(int x, int y, byte* dst, int numPixel);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="dst"></param>
		///<param name="numPixel"></param>
		unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int numPixel);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="dst"></param>
		///<param name="numPixel"></param>
		unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int numPixel);

		///<summary>
		///</summary>
		///<param name="x"></param>
		///<param name="y"></param>
		///<param name="dst"></param>
		///<param name="numPixel"></param>
		unsafe void CombineVerticalSpan(int x, int y, byte* dst, int numPixel);
	};

	///<summary>
	///</summary>
	public sealed class AlphaMaskByteUnclipped : IAlphaMask
	{
		private RasterBuffer _rasterBuffer;
		private readonly uint _step;
		private readonly uint _offset;

		///<summary>
		///</summary>
		public static int CoverShift = 8;

		///<summary>
		///</summary>
		public static int CoverNone = 0;

		///<summary>
		///</summary>
		public static int CoverFull = 255;

		///<summary>
		///</summary>
		///<param name="rbuf"></param>
		///<param name="step"></param>
		///<param name="offset"></param>
		public AlphaMaskByteUnclipped(RasterBuffer rbuf, uint step, uint offset)
		{
			_step = step;
			_offset = offset;
			_rasterBuffer = rbuf;
		}

		///<summary>
		///</summary>
		///<param name="rbuf"></param>
		public void Attach(RasterBuffer rbuf)
		{
			_rasterBuffer = rbuf;
		}

		public byte Pixel(int x, int y)
		{
			unsafe
			{
				return *(_rasterBuffer.GetPixelPointer(y) + x * _step + _offset);
			}
		}

		public byte CombinePixel(int x, int y, byte val)
		{
			unsafe
			{
				return (byte)((CoverFull + val * *(_rasterBuffer.GetPixelPointer(y) + x * _step + _offset)) >> CoverShift);
			}
		}

		public unsafe void FillHorizontalSpan(int x, int y, byte* dst, int numPixel)
		{
			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*dst++ = *mask;
				mask += _step;
			}
			while (--numPixel != 0);
		}

		public unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int numPixel)
		{
			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*dst = *mask;
				++dst;
				mask += _step;
			}
			while (--numPixel != 0);
		}

		public unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int numPixel)
		{
			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*dst = (byte)((CoverFull + (*dst) * (*mask)) >> CoverShift);
				++dst;
				mask += _step;
			}
			while (--numPixel != 0);
		}

		public unsafe void FillVerticalSpan(int x, int y, byte* dst, int numPixel)
		{
			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*dst++ = *mask;
				mask += _rasterBuffer.StrideInBytes();
			}
			while (--numPixel != 0);
		}

		public unsafe void CombineVerticalSpan(int x, int y, byte* dst, int numPixel)
		{
			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*dst = (byte)((CoverFull + (*dst) * (*mask)) >> CoverShift);
				++dst;
				mask += _rasterBuffer.StrideInBytes();
			}
			while (--numPixel != 0);
		}
	};

	///<summary>
	///</summary>
	public sealed class AlphaMaskByteClipped : IAlphaMask
	{
		private RasterBuffer _rasterBuffer;
		private readonly uint _step;
		private readonly uint _offset;

		///<summary>
		///</summary>
		public static int CoverShift = 8;

		///<summary>
		///</summary>
		public static int CoverNone = 0;

		///<summary>
		///</summary>
		public static int CoverFull = 255;

		///<summary>
		///</summary>
		///<param name="rbuf"></param>
		///<param name="step"></param>
		///<param name="offset"></param>
		public AlphaMaskByteClipped(RasterBuffer rbuf, uint step, uint offset)
		{
			_step = step;
			_offset = offset;
			_rasterBuffer = rbuf;
		}

		///<summary>
		///</summary>
		///<param name="rbuf"></param>
		public void Attach(RasterBuffer rbuf)
		{
			_rasterBuffer = rbuf;
		}

		public byte Pixel(int x, int y)
		{
			if (x >= 0 && y >= 0 &&
				x < (int)_rasterBuffer.Width() &&
				y < (int)_rasterBuffer.Height())
			{
				unsafe
				{
					return *(_rasterBuffer.GetPixelPointer(y) + x * _step + _offset);
				}
			}
			return 0;
		}

		public byte CombinePixel(int x, int y, byte val)
		{
			if (x >= 0 && y >= 0 &&
				x < (int)_rasterBuffer.Width() &&
				y < (int)_rasterBuffer.Height())
			{
				unsafe
				{
					return (byte)((CoverFull + val * *(_rasterBuffer.GetPixelPointer(y) + x * _step + _offset)) >> CoverShift);
				}
			}
			return 0;
		}

		public unsafe void FillHorizontalSpan(int x, int y, byte* dst, int numPixel)
		{
			int xmax = (int)_rasterBuffer.Width() - 1;
			int ymax = (int)_rasterBuffer.Height() - 1;

			int count = numPixel;
			byte* covers = dst;

			if (y < 0 || y > ymax)
			{
				Basics.MemClear(dst, numPixel);
				return;
			}

			if (x < 0)
			{
				count += x;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers, -x);
				covers -= x;
				x = 0;
			}

			if (x + count > xmax)
			{
				int rest = x + count - xmax - 1;
				count -= rest;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers + count, rest);
			}

			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*covers++ = *(mask);
				mask += _step;
			}
			while (--count != 0);
		}

		public unsafe void CombineHorizontalSpanFullCover(int x, int y, byte* dst, int numPixel)
		{
			int xmax = (int)_rasterBuffer.Width() - 1;
			int ymax = (int)_rasterBuffer.Height() - 1;

			int count = numPixel;
			byte* covers = dst;

			if (y < 0 || y > ymax)
			{
				Basics.MemClear(dst, numPixel);
				return;
			}

			if (x < 0)
			{
				count += x;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers, -x);
				covers -= x;
				x = 0;
			}

			if (x + count > xmax)
			{
				int rest = x + count - xmax - 1;
				count -= rest;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers + count, rest);
			}

			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*covers = *mask;
				++covers;
				mask += _step;
			}
			while (--count != 0);
		}

		public unsafe void CombineHorizontalSpan(int x, int y, byte* dst, int numPixel)
		{
			int xmax = (int)_rasterBuffer.Width() - 1;
			int ymax = (int)_rasterBuffer.Height() - 1;

			int count = numPixel;
			byte* covers = dst;

			if (y < 0 || y > ymax)
			{
				Basics.MemClear(dst, numPixel);
				return;
			}

			if (x < 0)
			{
				count += x;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers, -x);
				covers -= x;
				x = 0;
			}

			if (x + count > xmax)
			{
				int rest = x + count - xmax - 1;
				count -= rest;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers + count, rest);
			}

			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*covers = (byte)((CoverFull + (*covers) *
											   *mask) >>
								 CoverShift);
				++covers;
				mask += _step;
			}
			while (--count != 0);
		}

		public unsafe void FillVerticalSpan(int x, int y, byte* dst, int numPixel)
		{
			int xmax = (int)_rasterBuffer.Width() - 1;
			int ymax = (int)_rasterBuffer.Height() - 1;

			int count = numPixel;
			byte* covers = dst;

			if (x < 0 || x > xmax)
			{
				Basics.MemClear(dst, numPixel);
				return;
			}

			if (y < 0)
			{
				count += y;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers, -y);
				covers -= y;
				y = 0;
			}

			if (y + count > ymax)
			{
				int rest = y + count - ymax - 1;
				count -= rest;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers + count, rest);
			}

			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*covers++ = *mask;
				mask += _rasterBuffer.StrideInBytes();
			}
			while (--count != 0);
		}

		public unsafe void CombineVerticalSpan(int x, int y, byte* dst, int numPixel)
		{
			int xmax = (int)_rasterBuffer.Width() - 1;
			int ymax = (int)_rasterBuffer.Height() - 1;

			int count = numPixel;
			byte* covers = dst;

			if (x < 0 || x > xmax)
			{
				Basics.MemClear(dst, numPixel);
				return;
			}

			if (y < 0)
			{
				count += y;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers, -y);
				covers -= y;
				y = 0;
			}

			if (y + count > ymax)
			{
				int rest = y + count - ymax - 1;
				count -= rest;
				if (count <= 0)
				{
					Basics.MemClear(dst, numPixel);
					return;
				}
				Basics.MemClear(covers + count, rest);
			}

			byte* mask = _rasterBuffer.GetPixelPointer(y) + x * _step + _offset;
			do
			{
				*covers = (byte)((CoverFull + (*covers) * (*mask)) >> CoverShift);
				++covers;
				mask += _rasterBuffer.StrideInBytes();
			}
			while (--count != 0);
		}
	};
}