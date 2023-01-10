// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// Address Mode Transformation List
	/// </summary>
	public static class AddressModeTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform>
		{
			new Adc32(),
			new Add32(),
			new Addsd(),
			new Addss(),
			new And32(),
			new Btr32(),
			new Bts32(),
			new Dec32(),
			new Divsd(),
			new Divss(),
			new Mulsd(),
			new Mulss(),
			new Neg32(),
			new Not32(),
			new Or32(),
			new Rcr32(),
			new Roundsd(),
			new Roundss(),
			new Sar32(),
			new Sbb32(),
			new Shl32(),
			new Shld32(),
			new Shr32(),
			new Shrd32(),
			new Sub32(),
			new Subsd(),
			new Subss(),
			new Xor32(),
		};
	}
}
