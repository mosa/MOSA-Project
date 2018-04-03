// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicsHelper
	/// </summary>
	public static class IntrinsicsHelper
	{
		/// <summary>
		/// Allows quick internal call replacements
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <param name="internalMethod">The internal method to replace with.</param>
		/// <param name="internalClass">The internal class that has the internal method.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void MapToRunTime(Context context, MethodCompiler methodCompiler, string internalMethod, string internalClass = "Internal")
		{
			if (context == null || methodCompiler == null || internalMethod == null || internalClass == null)
				throw new ArgumentNullException();

			var type = methodCompiler.TypeSystem.GetTypeByName("Mosa.Runtime", internalClass);
			Debug.Assert(type != null, "Cannot find Mosa.Runtime." + internalClass);

			var method = type.FindMethodByName(internalMethod);
			Debug.Assert(method != null, "Cannot find " + internalMethod + " in " + type.Name);

			var symbol = Operand.CreateSymbolFromMethod(method, methodCompiler.TypeSystem);
			var result = context.Result;
			var operands = new List<Operand>(context.Operands);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
		}
	}
}
