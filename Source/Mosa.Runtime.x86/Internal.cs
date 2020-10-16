// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.x86
{
	public unsafe static class Internal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ExceptionHandler()
		{
			// capture this register immediately
			var exceptionObject = Intrinsic.GetExceptionRegister();

			var stackFrame = Runtime.Internal.GetStackFrame(1);

			for (uint i = 0; ; i++)
			{
				var returnAddress = Runtime.Internal.GetReturnAddressFromStackFrame(stackFrame);

				if (returnAddress.IsNull)
				{
					// hit the top of stack!
					Runtime.Internal.Fault(0XBAD00002, i);
				}

				var exceptionType = new TypeDefinition(exceptionObject.LoadPointer());

				var methodDef = Runtime.Internal.GetMethodDefinitionViaMethodExceptionLookup(returnAddress);

				if (!methodDef.IsNull)
				{
					var protectedRegion = Runtime.Internal.GetProtectedRegionEntryByAddress(returnAddress - 1, exceptionType, methodDef);

					if (!protectedRegion.IsNull)
					{
						// found handler for current method, call it

						var methodStart = methodDef.Method;
						uint handlerOffset = protectedRegion.HandlerOffset;
						var jumpTarget = methodStart + handlerOffset;

						uint stackSize = methodDef.StackSize & 0xFFFF; // lower 16-bits only
						var previousFrame = Mosa.Runtime.Internal.GetPreviousStackFrame(stackFrame);
						var newStack = previousFrame - stackSize;

						Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject.ToInt32());
					}
				}

				// no handler in method, go up the stack
				stackFrame = Mosa.Runtime.Internal.GetPreviousStackFrame(stackFrame);
			}
		}
	}
}
