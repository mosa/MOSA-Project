// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.x64
{
	public unsafe static class Internal
	{
		public static void Fault(uint code, uint extra = 0)
		{
			System.Diagnostics.Debug.Fail("Fault: " + ((int)code).ToString("hex") + " , Extra: " + ((int)extra).ToString("hex"));
		}

		public static MethodDefinition GetMethodDefinition(Pointer address)
		{
			var table = Native.GetMethodLookupTable();
			uint entries = Intrinsic.Load32(table);

			table += Pointer.Size; // skip count

			while (entries > 0)
			{
				var addr = Intrinsic.LoadPointer(table);
				uint size = Intrinsic.Load32(table, Pointer.Size);

				if (address >= addr && address < (addr + size))

				//if (address.ToInt64() >= addr.ToInt64() && (address.ToInt64() < (addr.ToInt64() + size)))
				{
					return new MethodDefinition(Intrinsic.LoadPointer(table, Pointer.Size * 2));
				}

				table += (Pointer.Size * 3);

				entries--;
			}

			return new MethodDefinition(Pointer.Zero);
		}

		public static MethodDefinition GetMethodDefinitionViaMethodExceptionLookup(Pointer address)
		{
			var table = Native.GetMethodExceptionLookupTable();

			if (table.IsNull)
			{
				return new MethodDefinition(Pointer.Zero);
			}

			uint entries = Intrinsic.Load32(table);

			table += Pointer.Size;

			while (entries > 0)
			{
				var addr = Intrinsic.LoadPointer(table);
				uint size = Intrinsic.Load32(table, Pointer.Size);

				if (address >= addr && address < (addr + size))

				//if (address.ToInt64() >= addr.ToInt64() && address.ToInt64() < addr.ToInt64() + size)
				{
					return new MethodDefinition(Intrinsic.LoadPointer(table, Pointer.Size * 2));
				}

				table += (Pointer.Size * 3);

				entries--;
			}

			return new MethodDefinition(Pointer.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ProtectedRegionDefinition GetProtectedRegionEntryByAddress(Pointer address, TypeDefinition exceptionType, MethodDefinition methodDef)
		{
			var protectedRegionTable = methodDef.ProtectedRegionTable;

			if (protectedRegionTable.IsNull)
			{
				return new ProtectedRegionDefinition(Pointer.Zero);
			}

			var method = methodDef.Method;

			if (method.IsNull)
			{
				return new ProtectedRegionDefinition(Pointer.Zero);
			}

			uint offset = (uint)method.GetOffset(address);
			uint entries = protectedRegionTable.NumberOfRegions;

			var protectedRegionDefinition = new ProtectedRegionDefinition(Pointer.Zero);
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
						(handlerType == ExceptionHandlerType.Exception && Runtime.Internal.IsTypeInInheritanceChain(exType, exceptionType)))
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
		public static Pointer GetPreviousStackFrame(Pointer ebp)
		{
			if (ebp < new Pointer(0x1000))
			{
				return Pointer.Zero;
			}

			return Intrinsic.LoadPointer(ebp);
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

			return Intrinsic.LoadPointer(stackframe, Pointer.Size);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetReturnAddressForStackFrame(Pointer stackframe, uint value)
		{
			Intrinsic.Store(stackframe, Pointer.Size, value);
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
			return GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, Pointer ebp, Pointer eip)
		{
			var entry = new SimpleStackTraceEntry();

			Pointer address;

			if (depth == 0 && eip != Pointer.Zero)
			{
				address = eip;
			}
			else
			{
				if (ebp.IsNull)
				{
					ebp = Native.GetEBP();
				}

				if (eip != Pointer.Zero)
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

						Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject.ToInt32());
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}
	}
}
