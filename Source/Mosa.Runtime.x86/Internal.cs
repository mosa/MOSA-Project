// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.x86
{
	public unsafe static class Internal
	{
		internal const uint NativeIntSize = 4;

		public static bool IsTypeInInheritanceChain(MDTypeDefinition* typeDefinition, MDTypeDefinition* chain)
		{
			while (chain != null)
			{
				if (chain == typeDefinition)
					return true;

				chain = chain->ParentType;
			}

			return false;
		}

		public static void* IsInstanceOfType(RuntimeTypeHandle handle, void* obj)
		{
			if (obj == null)
				return null;

			MDTypeDefinition* typeDefinition = (MDTypeDefinition*)((uint**)&handle)[0];

			MDTypeDefinition* objTypeDefinition = (MDTypeDefinition*)((uint*)obj)[0];

			if (IsTypeInInheritanceChain(typeDefinition, objTypeDefinition))
				return obj;

			return null;
		}

		public static void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			MDTypeDefinition* objTypeDefinition = (MDTypeDefinition*)((uint*)obj)[0];

			if (objTypeDefinition == null)
				return null;

			uint* bitmap = objTypeDefinition->Bitmap;

			if (bitmap == null)
				return null;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;
			uint value = bitmap[index];
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return null;

			return obj;
		}

		[Method("Mosa.Runtime.Internal.MemoryCopy")]
		public static void MemoryCopy(void* dest, void* src, uint count)
		{
			ulong* _dest = (ulong*)dest;
			ulong* _src = (ulong*)src;
			uint byteCount = count & 7;
			count >>= 3;

			for (; count >= 4; count -= 4, _dest += 4, _src += 4)
				Native.Memcpy256(_dest, _src);

			for (uint index = 0; index < count; index++)
				_dest[index] = _src[index];

			_dest += count;
			_src += count;

			byte* __dest = (byte*)_dest;
			byte* __src = (byte*)_src;
			for (uint index = 0; index < byteCount; index++)
				__dest[index] = __src[index];
		}

		[Method("Mosa.Runtime.Internal.MemorySet")]
		public static void MemorySet(void* dest, byte value, uint count)
		{
			// TEMP: assigning the method parameters into local variables forces the compiler to load the values
			// into virtual registers, which unlocks the optimizer to generate much better code quality.
			uint dst = (uint)dest;
			uint cnt = count;

			uint e3 = dst + cnt;
			byte val = value;

			// write 1 byte increments until 32-bit alignment
			for (; (dst & 0x3) != 0; dst++)
			{
				Native.Set8(dst, val);
			}

			uint e2 = e3 & 0xFFFFFFFC;
			uint value4 = (uint)((val << 24) | (val << 16) | (val << 8) | val);

			// write in 32-bit increments
			for (; dst < e2; dst = dst + 4)
			{
				Native.Set32(dst, value4);
			}

			// write remaining in 1 byte increments
			for (; dst < e3; dst++)
			{
				Native.Set8(dst, val);
			}
		}

		[Method("Mosa.Runtime.Internal.MemoryClear")]
		public static void MemoryClear(void* dest, uint count)
		{
			// TEMP: assigning the method parameters into local variables forces the compiler to load the values
			// into virtual registers, which unlocks the optimizer to generate much better code quality.
			uint dst = (uint)dest;
			uint cnt = count;

			uint e3 = dst + cnt;

			// write 1 byte increments until 32-bit alignment
			for (; (dst & 0x3) != 0; dst++)
			{
				Native.Set8(dst, 0);
			}

			uint e2 = e3 & 0xFFFFFFFC;

			// write in 32-bit increments
			for (; dst < e2; dst = dst + 4)
			{
				Native.Set32(dst, 0);
			}

			// write remaining in 1 byte increments
			for (; dst < e3; dst++)
			{
				Native.Set8(dst, 0);
			}
		}

		public static void DebugOutput(byte code)
		{
			Native.Out8(0xEA, code);
		}

		public static void DebugOutput(uint code)
		{
			Native.Out8(0xEB, (byte)((code >> 24) & 0xFF));
			Native.Out8(0xEB, (byte)((code >> 16) & 0xFF));
			Native.Out8(0xEB, (byte)((code >> 8) & 0xFF));
			Native.Out8(0xEB, (byte)(code & 0xFF));
		}

		public static void DebugOutput(string msg)
		{
			for (int i = 0; i < msg.Length; i++)
			{
				var c = msg[i];
				Native.Out8(0xEC, (byte)c);
			}

			Native.Out8(0xEC, 0);
		}

		public static void DebugOutput(string msg, uint code)
		{
			DebugOutput(msg);
			DebugOutput(code);
		}

		public static void Fault(uint code)
		{
			DebugOutput(code);
			System.Diagnostics.Debug.Fail("Fault: " + ((int)code).ToString("hex"));
		}

		public static MDMethodDefinition* GetMethodDefinition(uint address)
		{
			uint table = Native.GetMethodLookupTable();
			uint entries = Intrinsic.Load32(table);

			table = table + 4;

			while (entries > 0)
			{
				uint addr = Intrinsic.Load32(table);
				uint size = Intrinsic.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return (MDMethodDefinition*)Intrinsic.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return null;
		}

		public static MDMethodDefinition* GetMethodDefinitionViaMethodExceptionLookup(uint address)
		{
			uint table = Native.GetMethodExceptionLookupTable();

			if (table == 0)
				return null;

			uint entries = Intrinsic.Load32(table);

			table = table + NativeIntSize;

			while (entries > 0)
			{
				uint addr = Intrinsic.Load32(table);
				uint size = Intrinsic.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return (MDMethodDefinition*)Intrinsic.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return null;
		}

		public static MDProtectedRegionDefinition* GetProtectedRegionEntryByAddress(uint address, MDTypeDefinition* exceptionType, MDMethodDefinition* methodDef)
		{
			var protectedRegionTable = methodDef->ProtectedRegionTable;

			if (protectedRegionTable == null)
				return null;

			uint method = (uint)methodDef->Method;

			if (method == 0)
				return null;

			uint offset = address - method;
			uint entries = protectedRegionTable->NumberOfRegions;

			uint entry = 0;
			MDProtectedRegionDefinition* protectedRegionDef = null;
			uint currentStart = uint.MinValue;
			uint currentEnd = uint.MaxValue;

			while (entry < entries)
			{
				var prDef = protectedRegionTable->GetProtectedRegionDefinition(entry);

				uint start = prDef->StartOffset;
				uint end = prDef->EndOffset;

				if ((offset >= start) && (offset < end) && (start >= currentStart) && (end < currentEnd))
				{
					var handlerType = prDef->HandlerType;
					var exType = prDef->ExceptionType;

					// If the handler is a finally clause, accept without testing
					// If the handler is a exception clause, accept if the exception type is in the is within the inheritance chain of the exception object
					if ((handlerType == ExceptionHandlerType.Finally) ||
						(handlerType == ExceptionHandlerType.Exception && IsTypeInInheritanceChain(exType, exceptionType)))
					{
						protectedRegionDef = prDef;
						currentStart = start;
						currentEnd = end;
					}
				}

				entry++;
			}

			return protectedRegionDef;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetPreviousStackFrame(uint ebp)
		{
			if (ebp < 0x1000)
				return 0;
			return Intrinsic.Load32(ebp);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetStackFrame(uint depth)
		{
			return GetStackFrame(depth, 0);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetStackFrame(uint depth, uint ebp)
		{
			if (ebp == 0)
				ebp = Native.GetEBP();

			while (depth > 0)
			{
				depth--;

				ebp = GetPreviousStackFrame(ebp);

				if (ebp == 0)
					return 0;
			}

			return ebp;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetReturnAddressFromStackFrame(uint stackframe)
		{
			if (stackframe < 0x1000)
				return 0;
			return Intrinsic.Load32(stackframe, NativeIntSize);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetReturnAddressForStackFrame(uint stackframe, uint value)
		{
			Native.Set32(stackframe + NativeIntSize, value);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MDMethodDefinition* GetMethodDefinitionFromStackFrameDepth(uint depth)
		{
			return GetMethodDefinitionFromStackFrameDepth(depth, 0);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MDMethodDefinition* GetMethodDefinitionFromStackFrameDepth(uint depth, uint ebp)
		{
			if (ebp == 0)
				ebp = Native.GetEBP();

			ebp = GetStackFrame(depth + 0, ebp);

			uint address = GetReturnAddressFromStackFrame(ebp);
			return GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, uint ebp, uint eip)
		{
			var entry = new SimpleStackTraceEntry();

			uint address;
			if (depth == 0 && eip != 0)
				address = eip;
			else
			{
				if (ebp == 0)
					ebp = Native.GetEBP();

				if (eip != 0)
					depth--;

				ebp = GetStackFrame(depth, ebp);

				address = GetReturnAddressFromStackFrame(ebp);
			}

			var methodDef = GetMethodDefinition(address);

			if (methodDef == null)
				return entry;

			entry.MethodDefinition = methodDef;
			entry.Offset = address - (uint)(methodDef->Method);

			return entry;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ExceptionHandler()
		{
			// capture this register immediately
			uint exceptionObject = Native.GetExceptionRegister();

			uint stackFrame = GetStackFrame(1);

			for (;;)
			{
				uint returnAddress = GetReturnAddressFromStackFrame(stackFrame);

				if (returnAddress == 0)
				{
					// hit the top of stack!
					Fault(0XBAD00002);
				}

				var exceptionType = (MDTypeDefinition*)Intrinsic.Load32(exceptionObject);

				var methodDef = GetMethodDefinitionViaMethodExceptionLookup(returnAddress);

				if (methodDef != null)
				{
					var protectedRegion = GetProtectedRegionEntryByAddress(returnAddress - 1, exceptionType, methodDef);

					if (protectedRegion != null)
					{
						// found handler for current method, call it

						uint methodStart = (uint)methodDef->Method;
						uint handlerOffset = protectedRegion->HandlerOffset;
						uint jumpTarget = methodStart + handlerOffset;

						uint stackSize = methodDef->StackSize & 0xFFFF; // lower 16-bits only
						uint previousFrame = GetPreviousStackFrame(stackFrame);
						uint newStack = previousFrame - stackSize;

						Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject);
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}
	}
}
