// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// MulCarryOut64
	/// </summary>
	public sealed class MulCarryOut64 : BaseTransformation
	{
		public MulCarryOut64() : base(IRInstruction.MulCarryOut64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var methodName = "mul64carry";
			var method = transformContext.GetMethod("Mosa.Runtime.Math", "Multiplication", methodName);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;

			var v1 = transformContext.MethodCompiler.AddStackLocal(result2.Type);   // REVIEW
			var v2 = transformContext.AllocateVirtualRegister32();

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			var symbol = Operand.CreateSymbolFromMethod(method, transformContext.TypeSystem);

			context.SetInstruction(IRInstruction.AddressOf, v2, v1);
			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2, v2);
			context.AppendInstruction(IRInstruction.Load32, result2, v2, transformContext.ConstantZero32);

			transformContext.MethodCompiler.MethodScanner.MethodInvoked(method, transformContext.MethodCompiler.Method);
		}
	}
}
