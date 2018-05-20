// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public unsafe static class Internal
	{
		#region Allocation

		public static UIntPtr AllocateMemory(uint size)
		{
			return GC.AllocateObject(size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr AllocateObject(RuntimeTypeHandle handle, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = (2 * (uint)(UIntPtr.Size)) + classSize;
			var memory = AllocateMemory(allocationSize);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store(memory, UIntPtr.Size, 0);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr AllocateObject(RuntimeTypeHandle handle, int classSize)
		{
			return AllocateObject(handle, (uint)classSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr AllocateArray(RuntimeTypeHandle handle, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//   - Padding

			uint allocationSize = ((uint)(UIntPtr.Size) * 3) + elements * elementSize;
			allocationSize = (allocationSize + 3) & ~3u;    // Align to 4-bytes boundary

			var memory = AllocateMemory(allocationSize);

			Intrinsic.Store32(memory, 0, handle.Value.ToInt32());
			Intrinsic.Store32(memory, UIntPtr.Size, 0);
			Intrinsic.Store32(memory, UIntPtr.Size * 2, elements);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr AllocateString(RuntimeTypeHandle handle, uint length)
		{
			return AllocateArray(handle, sizeof(char), length);
		}

		#endregion Allocation

		#region (Un)Boxing

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr Box8(RuntimeTypeHandle handle, byte value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store8(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		public static UIntPtr Box16(RuntimeTypeHandle handle, ushort value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store16(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		public static UIntPtr Box32(RuntimeTypeHandle handle, uint value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store32(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		public static UIntPtr Box64(RuntimeTypeHandle handle, ulong value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size * 2);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store64(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		public static void* BoxR4(RuntimeTypeHandle handle, float value)
		{
			byte* memory = (byte*)AllocateObject(handle, UIntPtr.Size);
			*(float*)(memory + ((uint)(UIntPtr.Size) * 2)) = value;
			return memory;
		}

		public static void* BoxR8(RuntimeTypeHandle handle, double value)
		{
			byte* memory = (byte*)AllocateObject(handle, UIntPtr.Size * 2);
			*(double*)(memory + ((uint)(UIntPtr.Size) * 2)) = value;
			return memory;
		}

		public static void* Box(RuntimeTypeHandle handle, void* value, uint size)
		{
			byte* memory = (byte*)AllocateObject(handle, size);

			byte* dest = memory + (uint)(UIntPtr.Size * 2);
			byte* src = (byte*)value;

			for (int i = 0; i < size; i++)
			{
				dest[i] = src[i];
			}

			return memory;
		}

		public static byte Unbox8(void* box)
		{
			return *((byte*)box + (uint)(UIntPtr.Size) * 2);
		}

		public static ushort Unbox16(void* box)
		{
			return *(ushort*)((byte*)box + (uint)(UIntPtr.Size) * 2);
		}

		public static uint* Unbox32(void* box)
		{
			return (uint*)((byte*)box + (uint)(UIntPtr.Size) * 2);
		}

		public static ulong* Unbox64(void* box)
		{
			return (ulong*)((byte*)box + (uint)(UIntPtr.Size) * 2);
		}

		public static void* Unbox(void* box, void* vt, uint size)
		{
			//MemoryCopy(vt, (byte*)box + (uint)(UIntPtr.Size) * 2, size);

			byte* dest = (byte*)vt;
			byte* src = (byte*)box + (uint)(UIntPtr.Size * 2);

			for (int i = 0; i < size; i++)
			{
				dest[i] = src[i];
			}

			return vt;
		}

		#endregion (Un)Boxing

		#region Memory Manipulation

		public static void MemoryCopy(UIntPtr dest, UIntPtr src, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		public static void MemorySet(UIntPtr dest, byte value, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		public static void MemoryClear(UIntPtr dest, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		#endregion Memory Manipulation

		#region Metadata

		internal static LinkedList<RuntimeAssembly> Assemblies = null;

		public static void Setup()
		{
			Assemblies = new LinkedList<RuntimeAssembly>();

			// Get AssemblyListTable and Assembly count
			var assemblyListTable = Intrinsic.GetAssemblyListTable();
			uint assemblyCount = Intrinsic.Load32(assemblyListTable);

			// Loop through and populate the array
			for (int i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				var ptr = Intrinsic.Load32(assemblyListTable, UIntPtr.Size + (UIntPtr.Size * i));

				Assemblies.AddLast(new RuntimeAssembly((MDAssemblyDefinition*)ptr));
			}
		}

		#endregion Metadata

		public static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}
	}
}
