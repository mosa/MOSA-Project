// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.System.x86
{
	internal static class Math
	{
		//[Plug("System.Math::Abs")]
		//internal static double Abs(double value)
		//{
		//	return 0; // TODO
		//}

		//[Plug("System.Math::Abs")]
		//internal static float Abs(float value)
		//{
		//	return 0; // TODO
		//}

		[Plug("System.Math::Sqrt")]
		internal static double Sqrt(double d)
		{
			return Native.Sqrtsd(d);
		}

		[Plug("System.Math::Ceiling")]
		internal static double Ceiling(double d)
		{
			return Native.Roundsd2Positive(d);
		}

		[Plug("System.Math::Floor")]
		internal static double Floor(double d)
		{
			return Native.Roundsd2Negative(d);
		}

		//[Plug("System.Math::Log")]
		//internal static double Log(double d)
		//{
		//	return 0; // TODO
		//}

		//[Plug("System.Math::FMod")]
		//internal static double FMod(double x, double y)
		//{
		//	return 0; // TODO
		//}

		//[Plug("System.Math::ModF")]
		//internal static unsafe double ModF(double x, double* intptr)
		//{
		//	return 0; // TODO
		//}
	}
}
