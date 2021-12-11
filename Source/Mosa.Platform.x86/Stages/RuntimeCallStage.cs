// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class RuntimeCallStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.DivSigned64, DivSigned64);     // sdiv64
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64); // udiv64
			AddVisitation(IRInstruction.MulCarryOut64, MulCarryOut64);
			AddVisitation(IRInstruction.MulOverflowOut64, MulOverflowOut64);
			AddVisitation(IRInstruction.RemR4, RemFloatR4);
			AddVisitation(IRInstruction.RemR8, RemFloatR8);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);     // smod64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
		}

		#region Visitation Methods

		private void DivSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "sdiv64");
		}

		private void DivUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "udiv64");
		}

		private void MulCarryOut64(Context context)
		{
			var methodName = "mul64carry";
			var method = GetMethod("Mosa.Runtime.Math", "Multiplication", methodName);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;
			var v1 = MethodCompiler.AddStackLocal(result2.Type);
			var v2 = AllocateVirtualRegisterManagedPointer();

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.AddressOf, v2, v1);
			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2, v2);
			context.AppendInstruction(IRInstruction.Load32, result2, v2, ConstantZero32);

			MethodScanner.MethodInvoked(method, Method);
		}

		private void MulOverflowOut64(Context context)
		{
			var methodName = "mul64overflow";
			var method = GetMethod("Mosa.Runtime.Math", "Multiplication", methodName);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;
			var v1 = MethodCompiler.AddStackLocal(result2.Type);
			var v2 = AllocateVirtualRegisterManagedPointer();

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.AddressOf, v2, v1);
			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2, v2);
			context.AppendInstruction(IRInstruction.Load32, result2, v2, ConstantZero32);

			MethodScanner.MethodInvoked(method, Method);
		}

		private void RemFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR4");
		}

		private void RemFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR8");
		}

		private void RemSigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "smod64");
		}

		private void RemUnsigned64(Context context)
		{
			ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "umod64");
		}

		#endregion Visitation Methods
	}
}
