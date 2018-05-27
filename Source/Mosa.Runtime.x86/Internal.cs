// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Plug;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.x86
{
	public unsafe static class Internal
	{
		internal const uint NativeIntSize = 4;

		public static bool IsTypeInInheritanceChain(TypeDefinition typeDefinition, TypeDefinition chain)
		{
			while (!chain.IsNull)
			{
				if (chain.Handle == typeDefinition.Handle)
					return true;

				chain = chain.ParentType;
			}

			return false;
		}

		public static void* IsInstanceOfType(RuntimeTypeHandle handle, void* obj)
		{
			if (obj == null)
				return null;

			MDTypeDefinition* typeDefinition = (MDTypeDefinition*)((uint**)&handle)[0];

			MDTypeDefinition* objTypeDefinition = (MDTypeDefinition*)((uint*)obj)[0];

			if (IsTypeInInheritanceChain(new TypeDefinition(new UIntPtr(typeDefinition)), new TypeDefinition(new UIntPtr(objTypeDefinition))))
				return obj;

			return null;
		}

		public static void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			MDTypeDefinition* objTypeDefinition = (MDTypeDefinition*)((uint*)obj)[0];

			if (objTypeDefinition == null)
				return null;

			UIntPtr bitmap = objTypeDefinition->Bitmap;

			if (bitmap.ToPointer() == null)
				return null;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;

			//uint value = bitmap[index];
			uint value = Intrinsic.Load32(bitmap, index * UIntPtr.Size);
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return null;

			return obj;
		}

		[Method("Mosa.Runtime.Internal.MemoryCopy")]
		public static void MemoryCopy(UIntPtr dest, UIntPtr src, uint count)
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
		public static void MemorySet(UIntPtr dest, byte value, uint count)
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
				Intrinsic.Store8(dst, val);
			}

			uint e2 = e3 & 0xFFFFFFFC;
			uint value4 = (uint)((val << 24) | (val << 16) | (val << 8) | val);

			// write in 32-bit increments
			for (; dst < e2; dst += 4)
			{
				Intrinsic.Store32(dst, value4);
			}

			// write remaining in 1 byte increments
			for (; dst < e3; dst++)
			{
				Intrinsic.Store8(dst, val);
			}
		}

		[Method("Mosa.Runtime.Internal.MemoryClear")]
		public static void MemoryClear(UIntPtr dest, uint count)
		{
			// TEMP: assigning the method parameters into local variables forces the compiler to load the values
			// into virtual registers, which unlocks the optimizer to generate much better code quality.
			uint dst = (uint)dest;
			uint cnt = count;

			uint e3 = dst + cnt;

			// write 1 byte increments until 32-bit alignment
			for (; (dst & 0x3) != 0; dst++)
			{
				Intrinsic.Store8(dst, 0);
			}

			uint e2 = e3 & 0xFFFFFFFC;

			// write in 32-bit increments
			for (; dst < e2; dst += 4)
			{
				Intrinsic.Store32(dst, 0);
			}

			// write remaining in 1 byte increments
			for (; dst < e3; dst++)
			{
				Intrinsic.Store8(dst, 0);
			}
		}

		public static void Fault(uint code, uint extra = 0)
		{
			System.Diagnostics.Debug.Fail("Fault: " + ((int)code).ToString("hex") + " , Extra: " + ((int)extra).ToString("hex"));
		}

		public static MethodDefinition GetMethodDefinition(UIntPtr address)
		{
			var table = Native.GetMethodLookupTable();
			uint entries = Intrinsic.Load32(table);

			table += UIntPtr.Size; // skip count

			while (entries > 0)
			{
				var addr = Intrinsic.LoadPointer(table);
				uint size = Intrinsic.Load32(table, UIntPtr.Size);

				if (address.ToUInt64() >= addr.ToUInt64() && (address.ToUInt64() < (addr.ToUInt64() + size)))
				{
					return new MethodDefinition(Intrinsic.LoadPointer(table, UIntPtr.Size * 2));
				}

				table += (UIntPtr.Size * 3);

				entries--;
			}

			return new MethodDefinition(UIntPtr.Zero);
		}

		public static MethodDefinition GetMethodDefinitionViaMethodExceptionLookup(UIntPtr address)
		{
			var table = Native.GetMethodExceptionLookupTable();

			if (table == UIntPtr.Zero)
				return new MethodDefinition(UIntPtr.Zero);

			uint entries = Intrinsic.Load32(table);

			table += UIntPtr.Size;

			while (entries > 0)
			{
				var addr = Intrinsic.LoadPointer(table);
				uint size = Intrinsic.Load32(table, NativeIntSize);

				if (address.ToUInt64() >= addr.ToUInt64() && address.ToUInt64() < addr.ToUInt64() + size)
				{
					return new MethodDefinition(Intrinsic.LoadPointer(table, UIntPtr.Size * 2));
				}

				table += (UIntPtr.Size * 3);

				entries--;
			}

			return new MethodDefinition(UIntPtr.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ProtectedRegionDefinition GetProtectedRegionEntryByAddress(UIntPtr address, TypeDefinition exceptionType, MethodDefinition methodDef)
		{
			var protectedRegionTable = methodDef.ProtectedRegionTable;

			if (protectedRegionTable.IsNull)
				return new ProtectedRegionDefinition(UIntPtr.Zero);

			var method = methodDef.Method;

			if (method == UIntPtr.Zero)
				return new ProtectedRegionDefinition(UIntPtr.Zero);

			uint offset = (uint)(address.ToUInt64() - method.ToUInt64());
			uint entries = protectedRegionTable.NumberOfRegions;

			var protectedRegionDefinition = new ProtectedRegionDefinition(UIntPtr.Zero);
			uint currentStart = uint.MinValue;
			uint currentEnd = uint.MaxValue;
			uint entry = 0;

			while (entry < entries)
			{
				var prDef = protectedRegionTable.GetProtectedRegionDefinition(entry);

				uint start = prDef.StartOffset;
				uint end = prDef.EndOffset;

				if ((offset >= start) && (offset < end) && (start >= currentStart) && (end < currentEnd))
				{
					var handlerType = prDef.HandlerType;
					var exType = prDef.ExceptionType;

					// If the handler is a finally clause, accept without testing
					// If the handler is a exception clause, accept if the exception type is in the is within the inheritance chain of the exception object
					if ((handlerType == ExceptionHandlerType.Finally) ||
						(handlerType == ExceptionHandlerType.Exception && IsTypeInInheritanceChain(exType, exceptionType)))
					{
						protectedRegionDefinition = prDef;
						currentStart = start;
						currentEnd = end;
					}
				}

				entry++;
			}

			return protectedRegionDefinition;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr GetPreviousStackFrame(UIntPtr ebp)
		{
			if (ebp.ToUInt64() < 0x1000)
			{
				return UIntPtr.Zero;
			}

			return Intrinsic.LoadPointer(ebp);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr GetStackFrame(uint depth)
		{
			return GetStackFrame(depth, UIntPtr.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr GetStackFrame(uint depth, UIntPtr ebp)
		{
			if (ebp == UIntPtr.Zero)
			{
				ebp = Native.GetEBP();
			}

			while (depth > 0)
			{
				depth--;

				ebp = GetPreviousStackFrame(ebp);

				if (ebp == UIntPtr.Zero)
				{
					return UIntPtr.Zero;
				}
			}

			return ebp;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static UIntPtr GetReturnAddressFromStackFrame(UIntPtr stackframe)
		{
			if (stackframe.ToUInt64() < 0x1000)
			{
				return UIntPtr.Zero;
			}

			return Intrinsic.LoadPointer(stackframe, UIntPtr.Size);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetReturnAddressForStackFrame(UIntPtr stackframe, uint value)
		{
			Intrinsic.Store(stackframe, UIntPtr.Size, value);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodDefinition GetMethodDefinitionFromStackFrameDepth(uint depth)
		{
			return GetMethodDefinitionFromStackFrameDepth(depth, UIntPtr.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodDefinition GetMethodDefinitionFromStackFrameDepth(uint depth, UIntPtr ebp)
		{
			if (ebp == UIntPtr.Zero)
			{
				ebp = Native.GetEBP();
			}

			ebp = GetStackFrame(depth + 0, ebp);

			var address = GetReturnAddressFromStackFrame(ebp);
			return GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, UIntPtr ebp, UIntPtr eip)
		{
			var entry = new SimpleStackTraceEntry();

			UIntPtr address;

			if (depth == 0 && eip != UIntPtr.Zero)
			{
				address = eip;
			}
			else
			{
				if (ebp == UIntPtr.Zero)
				{
					ebp = Native.GetEBP();
				}

				if (eip != UIntPtr.Zero)
				{
					depth--;
				}

				ebp = GetStackFrame(depth, ebp);

				address = GetReturnAddressFromStackFrame(ebp);
			}

			var methodDef = GetMethodDefinition(address);

			if (methodDef.IsNull)
				return entry;

			entry.MethodDefinition = methodDef;
			entry.Offset = (uint)(address.ToUInt64() - methodDef.Method.ToUInt64());

			return entry;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ExceptionHandler()
		{
			// capture this register immediately
			var exceptionObject = new UIntPtr(Native.GetExceptionRegister());

			var stackFrame = GetStackFrame(1);

			for (uint i = 0; ; i++)
			{
				var returnAddress = GetReturnAddressFromStackFrame(stackFrame);

				if (returnAddress == UIntPtr.Zero)
				{
					// hit the top of stack!
					Fault(0XBAD00002, i);
				}

				var exceptionType = new TypeDefinition(Intrinsic.LoadPointer(exceptionObject));

				var methodDef = GetMethodDefinitionViaMethodExceptionLookup(returnAddress);

				if (!methodDef.IsNull)
				{
					var protectedRegion = GetProtectedRegionEntryByAddress(returnAddress - 1, exceptionType, methodDef);

					if (!protectedRegion.IsNull)
					{
						// found handler for current method, call it

						var methodStart = methodDef.Method;
						uint handlerOffset = protectedRegion.HandlerOffset;
						var jumpTarget = methodStart + (int)handlerOffset;

						uint stackSize = methodDef.StackSize & 0xFFFF; // lower 16-bits only
						var previousFrame = GetPreviousStackFrame(stackFrame);
						var newStack = previousFrame - (int)stackSize;

						Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject.ToUInt32());
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}
	}
}
