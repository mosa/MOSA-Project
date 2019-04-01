// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.System.x86
{
	internal static class MathF
	{
		[Plug("System.MathF::Ceiling")]
		internal static float Ceiling(float s)
		{
			return Native.Roundss2Positive(s);
		}

		[Plug("System.MathF::Sqrt")]
		internal static float Sqrt(float s)
		{
			return Native.Sqrtss(s);
		}

		[Plug("System.MathF::Floor")]
		internal static float Floor(float s)
		{
			return Native.Roundss2Negative(s);
		}

		//[Plug("System.MathF::Log")]
		//internal static float Log(float s)
		//{
		//	return 0; // TODO
		//}

		//[Plug("System.MathF::FMod")]
		//internal static float FMod(float x, float y)
		//{
		//	return 0; // TODO
		//}

		//[Plug("System.MathF::ModF")]
		//internal static unsafe float ModF(float x, float* intptr)
		//{
		//	return 0; // TODO
		//}
	}
}
