// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.AddressMode
{
	/// <summary>
	/// Shld32
	/// </summary>
	public sealed class Shld32 : BaseTransform
	{
		public Shld32() : base(X64.Shld32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !X64TransformHelper.IsAddressMode(context);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X64TransformHelper.AddressModeConversion(context, X64.Mov32);
		}
	}
}
