// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
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
			AddVisitation(IRInstruction.RemFloatR4, RemFloatR4);
			AddVisitation(IRInstruction.RemFloatR8, RemFloatR8);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64);     // smod64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
		}

		#region Visitation Methods

		private void DivSigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "sdiv64", context.Result, context.Operand1, context.Operand2);
		}

		private void DivUnsigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "udiv64", context.Result, context.Operand1, context.Operand2);
		}

		private void RemFloatR4(Context context)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			ReplaceWithPlatformDivisionCall(context, "RemR4", context.Result, context.Operand1, context.Operand2);
		}

		private void RemFloatR8(Context context)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			ReplaceWithPlatformDivisionCall(context, "RemR8", context.Result, context.Operand1, context.Operand2);
		}

		private void RemSigned64(Context context)
		{
			ReplaceWithDivisionCall(context, "smod64", context.Result, context.Operand1, context.Operand2);
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
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.Math.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.x86.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
