// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.RuntimeTime
{
	public abstract class BaseRuntimeTransform : BaseTransform
	{
		public BaseRuntimeTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

		public static MosaMethod GetVMCallMethod(TransformContext transform, string vmcall)
		{
			var method = transform.Compiler.InternalRuntimeType.FindMethodByName(vmcall)
				?? transform.Compiler.PlatformInternalRuntimeType.FindMethodByName(vmcall);

			Debug.Assert(method != null, $"Cannot find method: {vmcall}");

			transform.MethodScanner.MethodInvoked(method, transform.Method);

			return method;
		}

		public static void SetVMCall(TransformContext transform, Context context, string vmcall, Operand result, List<Operand> operands)
		{
			var method = GetVMCallMethod(transform, vmcall);
			var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
		}

		#endregion Helpers
	}
}
