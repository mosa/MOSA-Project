// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime.ARM32.Math;

internal static class FloatingPoint
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double FloatToDouble(float f)
	{
		// TODO
		return 0.0f;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float DoubleToFloat(double d)
	{
		// TODO
		return 0.0f;
	}

	public static uint BitCopyFloatR4ToInt32(float f)
	{
		// TODO
		return 0;
	}

	public static uint BitCopyInt32ToFloatR4(float f)
	{
		// TODO
		return 0;
	}
}
