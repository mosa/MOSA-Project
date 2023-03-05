// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms
{
	public static class VMHelper
	{
		public static MosaMethod GetVMCallMethod(TransformContext transform, VmCall vmcall)
		{
			string methodName = vmcall.ToString();

			var method = transform.Compiler.InternalRuntimeType.FindMethodByName(methodName) ?? transform.Compiler.PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			transform.MethodScanner.MethodInvoked(method, transform.Method);

			return method;
		}
	}
}
