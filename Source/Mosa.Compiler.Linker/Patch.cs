﻿/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Specifies the patch
	/// </summary>
	public struct Patch
	{
		public readonly int Shift;
		public readonly ulong Mask;

		public Patch(int start, int length, int shift)
		{
			this.Mask = (~(ulong)0) >> (64 - length);
			this.Mask = this.Mask << start;
			this.Shift = shift - start;
		}

		public ulong GetResult(ulong value)
		{
			ulong final = value & Mask;

			if (Shift != 0)
			{
				if (Shift > 0)
				{
					final = final << Shift;
				}
				else
				{
					final = final >> (-Shift);
				}
			}

			return final;
		}

		public static ulong GetResult(Patch[] patches, ulong value)
		{
			ulong final = 0;

			foreach (var patch in patches)
			{
				final = final | patch.GetResult(value);
			}

			return final;
		}

		public static ulong GetFinalMask(Patch[] patches)
		{
			ulong final = 0;

			foreach (var patch in patches)
			{
				final = final | patch.GetResult(~(ulong)0);
			}

			return final;
		}
	}
}