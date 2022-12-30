
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Compare32x64
	/// </summary>
	public sealed class Compare32x64 : BaseTransformation
	{
		public Compare32x64() : base(IRInstruction.Compare32x64, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			// FIXME!
			//Compare32x64(context);
		}
	}
}
