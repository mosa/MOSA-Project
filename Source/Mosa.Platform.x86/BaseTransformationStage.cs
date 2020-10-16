// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Base Transformation Stage
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "x86"; } }

		protected Operand Constant_1;
		protected Operand Constant_4;
		protected Operand Constant_16;
		protected Operand Constant_24;
		protected Operand Constant_31;
		protected Operand Constant_32;
		protected Operand Constant_64;

		protected Operand Constant_1F;

		protected override void Setup()
		{
			Constant_1 = CreateConstant(1);
			Constant_4 = CreateConstant(4);
			Constant_16 = CreateConstant(16);
			Constant_24 = CreateConstant(16);
			Constant_31 = CreateConstant(31);
			Constant_32 = CreateConstant(32);
			Constant_64 = CreateConstant(64);
			Constant_1F = Constant_31;
		}
	}
}
