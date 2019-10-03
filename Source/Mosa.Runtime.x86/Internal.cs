// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.x86
{
	public unsafe static class Internal
	{
		public static void Fault(uint code, uint extra = 0)
		{
			Debug.Fail("Fault: " + ((int)code).ToString("hex") + " , Extra: " + ((int)extra).ToString("hex"));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetPreviousStackFrame(Pointer ebp)
		{
			if (ebp < new Pointer(0x1000))
			{
				return Pointer.Zero;
			}

			return ebp.LoadPointer();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetStackFrame(uint depth)
		{
			return GetStackFrame(depth, Pointer.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetStackFrame(uint depth, Pointer ebp)
		{
			if (ebp.IsNull)
			{
				ebp = Native.GetEBP();
			}

			while (depth > 0)
			{
				depth--;

				ebp = GetPreviousStackFrame(ebp);

				if (ebp.IsNull)
				{
					return Pointer.Zero;
				}
			}

			return ebp;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetReturnAddressFromStackFrame(Pointer stackframe)
		{
			if (stackframe < new Pointer(0x1000))
			{
				return Pointer.Zero;
			}

			return stackframe.LoadPointer(Pointer.Size);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetReturnAddressForStackFrame(Pointer stackframe, uint value)
		{
			stackframe.Store32(Pointer.Size, value);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodDefinition GetMethodDefinitionFromStackFrameDepth(uint depth)
		{
			return GetMethodDefinitionFromStackFrameDepth(depth, Pointer.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodDefinition GetMethodDefinitionFromStackFrameDepth(uint depth, Pointer ebp)
		{
			if (ebp.IsNull)
			{
				ebp = Native.GetEBP();
			}

			ebp = GetStackFrame(depth + 0, ebp);

			var address = GetReturnAddressFromStackFrame(ebp);

			return Runtime.Internal.GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, Pointer ebp, Pointer eip)
		{
			var entry = new SimpleStackTraceEntry();

			Pointer address;

			if (depth == 0 && !eip.IsNull)
			{
				address = eip;
			}
			else
			{
				if (ebp.IsNull)
				{
					ebp = Native.GetEBP();
				}

				if (!eip.IsNull)
				{
					depth--;
				}

				ebp = GetStackFrame(depth, ebp);

				address = GetReturnAddressFromStackFrame(ebp);
			}

			var methodDef = Runtime.Internal.GetMethodDefinition(address);

			if (methodDef.IsNull)
				return entry;

			entry.MethodDefinition = methodDef;
			entry.Offset = (uint)methodDef.Method.GetOffset(address);
			return entry;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ExceptionHandler()
		{
			// capture this register immediately
			var exceptionObject = new Pointer(Native.GetExceptionRegister());

			var stackFrame = GetStackFrame(1);

			for (uint i = 0; ; i++)
			{
				var returnAddress = GetReturnAddressFromStackFrame(stackFrame);

				if (returnAddress.IsNull)
				{
					// hit the top of stack!
					Fault(0XBAD00002, i);
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
						var previousFrame = GetPreviousStackFrame(stackFrame);
						var newStack = previousFrame - stackSize;

						Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject.ToInt32());
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}
	}
}
