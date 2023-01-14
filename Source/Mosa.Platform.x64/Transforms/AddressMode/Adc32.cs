// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode
{
	/// <summary>
	/// Adc32
	/// </summary>
	public sealed class Adc32 : BaseTransform
	{
		public Adc32() : base(X64.Adc32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X64TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X64TransformHelper.AddressModeConversionCummulative(context, X64.Mov32);
		}
	}
}
