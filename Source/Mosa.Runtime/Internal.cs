// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public static class Internal
	{
		#region Allocation

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer AllocateObject(RuntimeTypeHandle handle, uint classSize)
		{
			// An object has the following memory layout:
			//   - Pointer TypeDef
			//   - Pointer SyncBlock
			//   - 0 .. n object data fields

			var memory = GC.AllocateObject((2 * (uint)(Pointer.Size)) + classSize);

			memory.StorePointer(0, new Pointer(handle.Value));

			if (Pointer.Size == 4)
			{
				memory.Store32(Pointer.Size, 0);
			}
			else
			{
				memory.Store64(Pointer.Size, 0);
			}

			return memory;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer AllocateObject(RuntimeTypeHandle handle, int classSize)
		{
			return AllocateObject(handle, (uint)classSize);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer AllocateArray(RuntimeTypeHandle handle, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - Pointer TypeDef
			//   - Pointer SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//   - Padding

			uint allocationSize = ((uint)(Pointer.Size) * 3) + (elements * elementSize);
			allocationSize = (allocationSize + 3) & ~3u;    // Align to 4-bytes boundary

			var memory = GC.AllocateObject(allocationSize);

			memory.StorePointer(0, new Pointer(handle.Value));

			if (Pointer.Size == 4)
			{
				memory.Store32(Pointer.Size, 0);
				memory.Store32(Pointer.Size * 2, elements);
			}
			else
			{
				memory.Store64(Pointer.Size, 0);
				memory.Store64(Pointer.Size * 2, elements);
			}

			return memory;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer AllocateString(RuntimeTypeHandle handle, uint length)
		{
			return AllocateArray(handle, sizeof(char), length);
		}

		#endregion Allocation

		#region (Un)Boxing

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box8(RuntimeTypeHandle handle, byte value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.Store8(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box16(RuntimeTypeHandle handle, ushort value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.Store16(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box32(RuntimeTypeHandle handle, uint value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.Store32(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer Box64(RuntimeTypeHandle handle, ulong value)
		{
			var memory = AllocateObject(handle, Pointer.Size * 2);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.Store64(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer BoxR4(RuntimeTypeHandle handle, float value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.StoreR4(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer BoxR8(RuntimeTypeHandle handle, double value)
		{
			var memory = AllocateObject(handle, Pointer.Size * 2);

			memory.StorePointer(0, new Pointer(handle.Value));
			memory.StoreR8(Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box(RuntimeTypeHandle handle, Pointer value, uint size)
		{
			var memory = AllocateObject(handle, size);

			MemoryCopy(memory + (Pointer.Size * 2), value, size);

			return memory;
		}

		public static Pointer Unbox(Pointer box)
		{
			return box + (Pointer.Size * 2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer UnboxAny(Pointer box, Pointer vt, uint size)
		{
			MemoryCopy(vt, box + (Pointer.Size * 2), size);

			return vt;
		}

		#endregion (Un)Boxing

		#region Memory Manipulation

		public static void MemoryCopy(Pointer dest, Pointer src, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				byte value = src.Load8(i);
				dest.Store8(i, value);
			}
		}

		public static void MemorySet(Pointer dest, byte value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				dest.Store8(i, value);
			}
		}

		public static void MemorySet(Pointer dest, ushort value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i += 2)
			{
				dest.Store16(i, value);
			}
		}

		public static void MemorySet(Pointer dest, uint value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i += 4)
			{
				dest.Store32(i, value);
			}
		}

		public static void MemoryClear(Pointer dest, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				dest.Store8(i, 0);
			}
		}

		#endregion Memory Manipulation

		#region Virtual Machine

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

		public static Pointer IsInstanceOfType(RuntimeTypeHandle handle, object obj)
		{
			if (obj == null)
				return Pointer.Zero;

			var o = Intrinsic.GetObjectAddress(obj);
			var objTypeDefinition = new TypeDefinition(o.LoadPointer());
			var typeDefinition = new TypeDefinition(new Pointer(handle.Value));

			if (IsTypeInInheritanceChain(typeDefinition, objTypeDefinition))
				return o;

			return Pointer.Zero;
		}

		public static object IsInstanceOfInterfaceType(int interfaceSlot, object obj)
		{
			if (obj == null)
				return null;

			var o = Intrinsic.GetObjectAddress(obj);
			var objTypeDefinition = new TypeDefinition(o.LoadPointer());

			var bitmap = objTypeDefinition.Bitmap;

			if (bitmap.IsNull)
				return null;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;

			uint value = bitmap.Load32(index * 4);
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return null;

			return obj;
		}

		#endregion Virtual Machine

		#region Metadata

		public static MethodDefinition GetMethodDefinition(Pointer address)
		{
			var table = Intrinsic.GetMethodLookupTable();
			uint entries = table.Load32();

			table += Pointer.Size; // skip count

			while (entries > 0)
			{
				var addr = table.LoadPointer();
				uint size = table.Load32(Pointer.Size);

				if (address >= addr && address < (addr + size))
				{
					return new MethodDefinition(table.LoadPointer(Pointer.Size * 2));
				}

				table += Pointer.Size * 3;

				entries--;
			}

			return new MethodDefinition(Pointer.Zero);
		}

		public static MethodDefinition GetMethodDefinitionViaMethodExceptionLookup(Pointer address)
		{
			var table = Intrinsic.GetMethodExceptionLookupTable();

			if (table.IsNull)
			{
				return new MethodDefinition(Pointer.Zero);
			}

			uint entries = table.Load32();

			table += Pointer.Size;

			while (entries > 0)
			{
				var addr = table.LoadPointer();
				uint size = table.Load32(Pointer.Size);

				if (address >= addr && address < (addr + size))
				{
					return new MethodDefinition(table.LoadPointer(Pointer.Size * 2));
				}

				table += Pointer.Size * 3;

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

		#endregion Metadata

		#region Internal

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetPreviousStackFrame(Pointer stackFrame)
		{
			if (stackFrame < new Pointer(0x1000))
			{
				return Pointer.Zero;
			}

			return stackFrame.LoadPointer();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetStackFrame(uint depth)
		{
			return GetStackFrame(depth, Pointer.Zero);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer GetStackFrame(uint depth, Pointer stackFrame)
		{
			if (stackFrame.IsNull)
			{
				stackFrame = Intrinsic.GetStackFrame();
			}

			while (depth > 0)
			{
				depth--;

				stackFrame = GetPreviousStackFrame(stackFrame);

				if (stackFrame.IsNull)
				{
					return Pointer.Zero;
				}
			}

			return stackFrame;
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
		public static MethodDefinition GetMethodDefinitionFromStackFrameDepth(uint depth, Pointer stackFrame)
		{
			if (stackFrame.IsNull)
			{
				stackFrame = Intrinsic.GetStackFrame();
			}

			stackFrame = GetStackFrame(depth + 0, stackFrame);

			var address = GetReturnAddressFromStackFrame(stackFrame);

			return Runtime.Internal.GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, Pointer stackFrame, Pointer eip)
		{
			var entry = new SimpleStackTraceEntry();

			Pointer address;

			if (depth == 0 && !eip.IsNull)
			{
				address = eip;
			}
			else
			{
				if (stackFrame.IsNull)
				{
					stackFrame = Intrinsic.GetStackFrame();
				}

				if (!eip.IsNull)
				{
					depth--;
				}

				stackFrame = GetStackFrame(depth, stackFrame);

				address = GetReturnAddressFromStackFrame(stackFrame);
			}

			var methodDef = Runtime.Internal.GetMethodDefinition(address);

			if (methodDef.IsNull)
				return entry;

			entry.MethodDefinition = methodDef;
			entry.Offset = (uint)methodDef.Method.GetOffset(address);
			return entry;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void xxExceptionHandler()
		{
			// capture this register immediately
			var exceptionObject = Intrinsic.GetExceptionRegister();

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

				var methodDef = GetMethodDefinitionViaMethodExceptionLookup(returnAddress);

				if (!methodDef.IsNull)
				{
					var protectedRegion = GetProtectedRegionEntryByAddress(returnAddress - 1, exceptionType, methodDef);

					if (!protectedRegion.IsNull)
					{
						// found handler for current method, call it

						var methodStart = methodDef.Method;
						uint handlerOffset = protectedRegion.HandlerOffset;
						var jumpTarget = methodStart + handlerOffset;

						uint stackSize = methodDef.StackSize & 0xFFFF; // lower 16-bits only
						var previousFrame = GetPreviousStackFrame(stackFrame);
						var newStack = previousFrame - stackSize;

						//Native.FrameJump(jumpTarget, newStack, previousFrame, exceptionObject.ToInt32());
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}

		#endregion Internal

		public static void Fault(uint code, uint extra = 0)
		{
			Debug.Fail("Fault: " + ((int)code).ToString("hex") + " , Extra: " + ((int)extra).ToString("hex"));
		}

		public static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}
	}
}
