// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public  static class Internal
	{
		#region Allocation

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr Box16(RuntimeTypeHandle handle, ushort value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store16(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr Box32(RuntimeTypeHandle handle, uint value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store32(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr Box64(RuntimeTypeHandle handle, ulong value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size * 2);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store64(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr BoxR4(RuntimeTypeHandle handle, float value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.StoreR4(memory, UIntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static UIntPtr BoxR8(RuntimeTypeHandle handle, double value)
		{
			var memory = AllocateObject(handle, UIntPtr.Size * 2);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.StoreR8(memory, UIntPtr.Size * 2, value);

			return memory;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		public static UIntPtr Box(RuntimeTypeHandle handle, UIntPtr value, uint size)
		{
			var memory = AllocateObject(handle, size);

			MemoryCopy(memory + (UIntPtr.Size * 2), value, size);

			return memory;
		}

		public static UIntPtr Unbox8(UIntPtr box)
		{
			return box + (UIntPtr.Size * 2);
		}

		public static UIntPtr Unbox16(UIntPtr box)
		{
			return box + (UIntPtr.Size * 2);
		}

		public static UIntPtr Unbox32(UIntPtr box)
		{
			return box + (UIntPtr.Size * 2);
		}

		public static UIntPtr Unbox64(UIntPtr box)
		{
			return box + (UIntPtr.Size * 2);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		public static UIntPtr Unbox(UIntPtr box, UIntPtr vt, uint size)
		{
			MemoryCopy(vt, box + (UIntPtr.Size * 2), size);

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

		public unsafe static void Setup()
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
