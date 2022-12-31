
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Lzcnt32
	/// </summary>
	//public sealed class Lzcnt32 : BaseTransform
	//{
	//	public Lzcnt32() : base(X64.Lzcnt32, TransformType.Manual | TransformType.Transform)
	//	{
	//	}

	//	public override bool Match(Context context, TransformContext transform)
	//	{
	//		return true;
	//	}

	//	public override void Transform(Context context, TransformContext transform)
	//	{
	//		transform.MoveOperand1ToVirtualRegister(context, X64.Mov32);
	//	}
	//}
}
