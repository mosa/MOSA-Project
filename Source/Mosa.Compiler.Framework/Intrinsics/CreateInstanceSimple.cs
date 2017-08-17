// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::CreateInstanceSimple")]
	internal class CreateInstanceSimple : IIntrinsicInternalMethod
	{
		private const string InternalMethodName = "CreateInstanceSimple";

		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var ctor = context.Operand1;
			var thisObject = context.Operand2;
			var result = context.Result;
			var method = context.InvokeMethod;

			//var internalMethod = methodCompiler.TypeSystem.DefaultLinkerType.FindMethodByName(InternalMethodName);

			//if (internalMethod == null)
			//{
			//	var param = methodCompiler.TypeSystem.CreateParameter(internalMethod, "this", methodCompiler.TypeSystem.BuiltIn.Object);
			//	var parameters = new List<MosaParameter>(1) { param };
			//	internalMethod = methodCompiler.TypeSystem.CreateLinkerMethod(InternalMethodName, methodCompiler.TypeSystem.BuiltIn.Void, false, parameters);
			//}

			context.SetInstruction(IRInstruction.CallDynamic, null, ctor, thisObject);

			//context.InvokeMethod = internalMethod;

			//var symbol = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, internalMethod);
			//context.SetInstruction(IRInstruction.CallStatic, null, ctor, thisObject);

			context.AppendInstruction(IRInstruction.MoveInteger, result, thisObject);
		}

		#endregion Methods
	}
}
