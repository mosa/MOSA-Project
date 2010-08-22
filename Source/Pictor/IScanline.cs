/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

namespace Pictor
{
	public struct ScanlineSpan
	{
		public int x;
		public int len;
		public uint cover_index;
	};

	public interface IScanline
	{
		void Finalize(int y);
		void Reset(int min_x, int max_x);
		void ResetSpans();
		uint NumberOfSpans
		{
			get;
		}

		ScanlineSpan Begin
		{
			get;
		}
		ScanlineSpan GetNextScanlineSpan();
		int y();
		byte[] GetCovers();
		void AddCell(int x, uint cover);
		void AddSpan(int x, int len, uint cover);
	};
}
