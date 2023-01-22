// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// Mulss
	/// </summary>
	public sealed class Mulss : BaseTransform
	{
		public Mulss() : base(X86.Mulss, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X86TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X86TransformHelper.AddressModeConversionCummulative(context, X86.Movss);
		}
	}
}
