// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.BaseTransformationStage" />
	public sealed class RuntimeCallStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.DivSigned32, DivSigned32);     // sdiv32
			AddVisitation(IRInstruction.DivSigned64, DivSigned64);     // sdiv64
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64); // udiv64
			AddVisitation(IRInstruction.DivUnsigned32, DivUnsigned32); // udiv32
			AddVisitation(IRInstruction.RemSigned32, RemSigned32);     // smod32
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);     // smod64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
			AddVisitation(IRInstruction.RemUnsigned32, RemUnsigned32); // umod32
		}

		#region Visitation Methods

		private void DivSigned32(Context context)
		{
			ReplaceWithDivisionCall(context, "sdiv32", context.Result, context.Operand1, context.Operand2);
		}

		private void DivSigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "sdiv64", context.Result, context.Operand1, context.Operand2);
		}

		private void DivUnsigned32(Context context)
		{
			ReplaceWithDivisionCall(context, "udiv32", context.Result, context.Operand1, context.Operand2);
		}

		private void DivUnsigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "udiv64", context.Result, context.Operand1, context.Operand2);
		}

		private void RemSigned32(Context context)
		{
			ReplaceWithDivisionCall(context, "smod32", context.Result, context.Operand1, context.Operand2);
		}

		private void RemSigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "smod64", context.Result, context.Operand1, context.Operand2);
		}

		private void RemUnsigned32(Context context)
		{
			ReplaceWithDivisionCall(context, "umod32", context.Result, context.Operand1, context.Operand2);
		}

		private void RemUnsigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "umod64", context.Result, context.Operand1, context.Operand2);
		}

		#endregion Visitation Methods

		private void ReplaceWithDivisionCall(Context context, string methodName, Operand result, Operand operand1, Operand operand2)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.Math", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.Math.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);

			MethodScanner.MethodInvoked(method, Method);
		}

		private void ReplaceWithPlatformDivisionCall(Context context, string methodName, Operand result, Operand operand1, Operand operand2)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.ARMv8A32.Math", "FloatingPoint");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.ARMv8A32.Math::FloatingPoint type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
