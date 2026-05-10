// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// Fixed Registers Transformation List
/// </summary>
public static class FixedRegistersTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Cdq32.Instance,
		Div32.Instance,
		IDiv32.Instance,
		MovStoreSeg32.Instance,
		Mul32.Instance,
		Rcr32.Instance,
		Sar32.Instance,
		Shl32.Instance,
		Shld32.Instance,
		Shr32.Instance,
		Shrd32.Instance,

		RdMSR.Instance,
		WrMSR.Instance,
		In8.Instance,
		In16.Instance,
		In32.Instance,
		Out8.Instance,
		Out16.Instance,
		Out32.Instance,

		Cdq64.Instance,
		Div64.Instance,
		IDiv64.Instance,
		MovStoreSeg64.Instance,
		Mul64.Instance,
		Rcr64.Instance,
		Sar64.Instance,
		Shl64.Instance,
		Shld64.Instance,
		Shr64.Instance,
		Shrd64.Instance,

		IMul32Constant.Instance,
		IMul64Constant.Instance,
	};
}
