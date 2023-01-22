// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// Adc32
	/// </summary>
	public sealed class Dec32 : BaseTransform
	{
		public Dec32() : base(X86.Dec32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X86TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X86TransformHelper.AddressModeConversion(context, X86.Mov32);
		}
	}
}
