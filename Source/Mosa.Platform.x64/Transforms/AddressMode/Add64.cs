// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.AddressMode
{
	/// <summary>
	/// Add64
	/// </summary>
	public sealed class Add64 : BaseTransform
	{
		public Add64() : base(X64.Add64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X64TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X64TransformHelper.AddressModeConversionCummulative(context, X64.Mov64);
		}
	}
}
