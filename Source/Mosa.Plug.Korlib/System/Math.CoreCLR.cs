// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System
{
	internal static class Math
	{
		[Plug("System.Math::Abs")]
		internal static double Abs(double value)
		{
			return 0; // TODO
		}

		[Plug("System.Math::Abs")]
		internal static float Abs(float value)
		{
			return 0; // TODO
		}

		[Plug("System.Math::Ceiling")]
		internal static double Ceiling(double a)
		{
			return 0; // TODO
		}

		[Plug("System.Math::Log")]
		internal static double Log(double d)
		{
			return 0; // TODO
		}

		[Plug("System.Math::FMod")]
		internal static double FMod(double x, double y)
		{
			return 0; // TODO
		}

		[Plug("System.Math::ModF")]
		internal static unsafe double ModF(double x, double* intptr)
		{
			return 0; // TODO
		}
	}
}
