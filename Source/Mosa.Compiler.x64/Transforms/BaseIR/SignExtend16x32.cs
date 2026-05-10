// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// SignExtend16x32
/// </summary>
public sealed class SignExtend16x32 : BaseIRTransform
{
	public static readonly SignExtend16x32 Instance = new();

	private SignExtend16x32() : base(IR.SignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx16To32);
	}
}
