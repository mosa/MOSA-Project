// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Runtime
{
	public abstract class BaseRuntimeTransform : BaseTransform
	{
		public BaseRuntimeTransform(BaseInstruction instruction, TransformType type, int priority = 50, bool log = false)
			: base(instruction, type, priority, log)
		{ }

		public override bool Match(Context context, Transform transform)
		{
			return true;
		}

		#region Helpers

		public static MosaMethod GetVMCallMethod(Transform transform, string vmcall)
		{
			var method = transform.Compiler.InternalRuntimeType.FindMethodByName(vmcall)
				?? transform.Compiler.PlatformInternalRuntimeType.FindMethodByName(vmcall);

			Debug.Assert(method != null, $"Cannot find method: {vmcall}");

			transform.MethodScanner.MethodInvoked(method, transform.Method);

			return method;
		}

		public static void SetVMCall(Transform transform, Context context, string vmcall, Operand result, List<Operand> operands)
		{
			var method = GetVMCallMethod(transform, vmcall);
			var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

			context.SetInstruction(IR.CallStatic, result, symbol, operands);
		}

		#endregion Helpers
	}
}
