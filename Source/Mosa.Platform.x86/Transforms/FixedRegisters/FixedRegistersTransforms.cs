// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FixedRegisters
{
	/// <summary>
	/// Fixed Registers Transformation List
	/// </summary>
	public static class FixedRegistersTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform>
		{
			new Cdq32(),
			new Div32(),
			new IDiv32(),
			new IMul32(),
			new In16(),
			new In32(),
			new In8(),
			new MovStoreSeg32(),
			new Mul32(),
			new Out8(),
			new Out16(),
			new Out32(),
			new Rcr32(),
			new RdMSR(),

			new Sar32(),
			new Shl32(),
			new Shld32(),
			new Shr32(),
			new Shrd32(),
			new WrMSR(),
		};
	}
}
