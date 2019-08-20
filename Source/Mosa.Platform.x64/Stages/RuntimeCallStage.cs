// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class RuntimeCallStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.RemFloatR4, RemFloatR4);
			AddVisitation(IRInstruction.RemFloatR8, RemFloatR8);
		}

		#region Visitation Methods

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

		#endregion Visitation Methods

		private void ReplaceWithCall(Context context, string namespaceName, string typeName, string methodName)
		{
			var method = GetMethod(namespaceName, typeName, methodName);

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			// FUTURE: throw compiler exception

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
