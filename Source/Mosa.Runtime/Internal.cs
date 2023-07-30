// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime;

public static class Internal
{
	#region Data Members

	internal static int objectSequence;

	/// <summary>
	/// An object header the following memory layout:
	///   - Hash Value (32-bit)
	///   - Lock & Status (32-bit)
	///   - MethodTable (native int)
	/// </summary>
	internal static int ObjectHeaderSize
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 4 + 4 + Pointer.Size;
	}

	#endregion Data Members

	#region Allocation

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Pointer AllocateObject(Pointer methodTable, uint classSize)
	{
		var hashvalue = Interlocked.Increment(ref objectSequence);

		// An object has the following memory layout:
		//   - Object Header
		//   - 0 .. n object data fields
		//
		// Object references point to the end of the header
		// so accessing the header requires negative offsets from the object reference

		var allocationSize = ObjectHeaderSize + classSize;

		var memory = GC.AllocateObject((uint)allocationSize);

		// Hash
		memory.Store32(0, hashvalue);

		// Lock & Status
		memory.Store32(4, 0);

		// Set MethodTable
		memory.StorePointer(8, methodTable);

		return memory + ObjectHeaderSize;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer AllocateObject(Pointer methodTable, int classSize)
	{
		return AllocateObject(methodTable, (uint)classSize);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Pointer AllocateArray(Pointer methodTable, uint elementSize, uint elements)
	{
		// An array has the following memory layout:
		//   - Object Header
		//   - Length (native int)
		//   - ElementType[length] elements
		//
		// Object references point to the end of the header
		// so accessing the header requires negative offsets from the object reference

		var memory = AllocateObject(methodTable, (uint)(Pointer.Size + elements * elementSize));

		if (Pointer.Size == 4)
		{
			memory.Store32(0, elements);
		}
		else
		{
			memory.Store64(0u, elements);
		}

		return memory;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Pointer AllocateString(Pointer methodTable, uint length)
	{
		return AllocateArray(methodTable, sizeof(char), length);
	}

	#endregion Allocation

	#region (Un)Boxing

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer Box8(Pointer methodTable, byte value)
	{
		var memory = AllocateObject(methodTable, 8);

		memory.Store8(value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer Box16(Pointer methodTable, ushort value)
	{
		var memory = AllocateObject(methodTable, 4);

		memory.Store16(value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer Box32(Pointer methodTable, uint value)
	{
		var memory = AllocateObject(methodTable, 4);

		memory.Store32(value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Pointer Box64(Pointer methodTable, ulong value)
	{
		var memory = AllocateObject(methodTable, 8);

		memory.Store64(0, value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer BoxR4(Pointer methodTable, float value)
	{
		var memory = AllocateObject(methodTable, 4);

		memory.StoreR4(value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer BoxR8(Pointer methodTable, double value)
	{
		var memory = AllocateObject(methodTable, 8);

		memory.StoreR8(value);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer Box(Pointer methodTable, Pointer value, uint size)
	{
		var memory = AllocateObject(methodTable, size);

		MemoryCopy(memory, value, size);

		return memory;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Pointer UnboxAny(Pointer src, Pointer dest, uint size)
	{
		// TODO: Check if compatible type

		MemoryCopy(dest, src, size);

		return dest;
	}

	#endregion (Un)Boxing

	#region Memory Manipulation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void MemoryCopy(Pointer dest, Pointer src, uint count)
	{
		//MemoryCopy1(dest, src, count);
		var mod = count % 4;

		MemoryCopy1(dest, src, mod);
		MemoryCopy4(dest + mod, src + mod, count - mod);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MemoryCopy1(Pointer dest, Pointer src, uint count)
	{
		if (count == 0)
			return;

		for (var i = 0; i < count; i++)
		{
			var value = src.Load8(i);
			dest.Store8(i, value);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MemoryCopy4(Pointer dest, Pointer src, uint count)
	{
		if (count == 0)
			return;

		for (var i = 0; i < count; i += 4)
		{
			var value = src.Load32(i);
			dest.Store32(i, value);
		}
	}

	public static void MemorySet(Pointer dest, byte value, uint count)
	{
		for (var i = 0; i < count; i++)
		{
			dest.Store8(i, value);
		}
	}

	public static void MemorySet(Pointer dest, ushort value, uint count)
	{
		for (var i = 0; i < count; i += 2)
		{
			dest.Store16(i, value);
		}
	}

	public static void MemorySet(Pointer dest, uint value, uint count)
	{
		for (var i = 0; i < count; i += 4)
		{
			dest.Store32(i, value);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void MemoryClear(Pointer dest, uint count)
	{
		//MemoryClear1(dest, count);
		var mod = count % 4;

		MemoryClear1(dest, mod);
		MemoryClear4(dest + mod, count - mod);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MemoryClear1(Pointer dest, uint count)
	{
		if (count == 0)
			return;

		for (var i = 0; i < count; i++)
		{
			dest.Store8(i, 0);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void MemoryClear4(Pointer dest, uint count)
	{
		if (count == 0)
			return;

		for (var i = 0; i < count; i += 4)
		{
			dest.Store32(i, 0);
		}
	}

	#endregion Memory Manipulation

	#region Virtual Machine

	public static Pointer GetTypeDefinition(object obj)
	{
		var address = Intrinsic.GetObjectAddress(obj);
		return address.LoadPointer(-Pointer.Size);
	}

	public static Pointer GetTypeDefinition(Pointer obj)
	{
		return obj.LoadPointer(-Pointer.Size);
	}

	public static Pointer GetObjectLockAndStatus(object obj)
	{
		var address = Intrinsic.GetObjectAddress(obj);

		return address - Pointer.Size - 4;
	}

	public static Pointer GetObjectHashValue(object obj)
	{
		var address = Intrinsic.GetObjectAddress(obj);

		return address - Pointer.Size - 4 - 4;
	}

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

	public static object IsInstanceOfType(RuntimeTypeHandle handle, object obj)
	{
		if (obj == null)
			return null;

		var objTypeDefinition = new TypeDefinition(GetTypeDefinition(obj));
		var typeDefinition = new TypeDefinition(new Pointer(handle.Value));

		if (IsTypeInInheritanceChain(typeDefinition, objTypeDefinition))
			return obj;

		return null;
	}

	public static object IsInstanceOfInterfaceType(int interfaceSlot, object obj)
	{
		if (obj == null)
			return null;

		var objTypeDefinition = new TypeDefinition(GetTypeDefinition(obj));

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

			if (address >= addr && address < addr + size)
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

			if (address >= addr && address < addr + size)
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

			if (offset >= start && offset < end && start >= currentStart && end < currentEnd)
			{
				var handlerType = prDef.HandlerType;
				var exType = prDef.ExceptionType;

				// If the handler is a finally clause, accept without testing
				// If the handler is a exception clause, accept if the exception type is in the is within the inheritance chain of the exception object
				if (handlerType == ExceptionHandlerType.Finally ||
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
		return GetStackFrame(depth + 1, Pointer.Zero);
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

		return GetMethodDefinition(address);
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

		var methodDef = GetMethodDefinition(address);

		if (methodDef.IsNull)
			return entry;

		entry.MethodDefinition = methodDef;
		entry.Offset = (uint)methodDef.Method.GetOffset(address);
		return entry;
	}

	#endregion Internal

	public static void Fault(uint code, uint extra = 0)
	{
		Debug.Fail("Fault: " + ((int)code).ToString("x") + " , Extra: " + ((int)extra).ToString("x"));
	}

	#region Throw Exceptions

	public static void ThrowIndexOutOfRangeException()
	{
		throw new IndexOutOfRangeException();
	}

	public static void ThrowOverflowException()
	{
		throw new OverflowException();
	}

	public static void ThrowDivideByZeroException()
	{
		throw new DivideByZeroException();
	}

	#endregion Throw Exceptions
}
