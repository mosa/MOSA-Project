// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.StaticLoad;

/// <summary>
/// Load64
/// </summary>
public sealed class Load64 : BaseTransform
{
	public Load64() : base(IRInstruction.Load64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;

		if (!operand1.IsStaticField || !operand1.Field.IsStatic)
			return false;

		if ((operand1.Field.FieldAttributes & MosaFieldAttributes.InitOnly) == 0)
			return false;

		return (operand1.Field.DeclaringType.IsValueType && operand1.Field.DeclaringType.Name is "System.IntPtr" or "System.UIntPtr" && operand1.Field.Name == "Zero");
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.Move64, context.Result, transform.Constant64_0);
	}
}
