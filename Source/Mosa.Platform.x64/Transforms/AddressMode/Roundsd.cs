// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode
{
	/// <summary>
	/// Roundsd
	/// </summary>
	public sealed class Roundsd : BaseTransform
	{
		public Roundsd() : base(X64.Roundsd, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X64TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X64TransformHelper.AddressModeConversion(context, X64.Movsd);
		}
	}
}
