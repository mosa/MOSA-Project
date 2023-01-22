// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// And32
	/// </summary>
	public sealed class And32 : BaseTransform
	{
		public And32() : base(X86.And32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X86TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X86TransformHelper.AddressModeConversionCummulative(context, X86.Mov32);
		}
	}
}
