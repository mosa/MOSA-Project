// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Runtime
{
	public unsafe static class Internal
	{
		#region Allocation

		public static void* AllocateMemory(uint size)
		{
			return GC.AllocateObject(size);
		}

		public static void* AllocateObject(RuntimeTypeHandle handle, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = (2 * (uint)(sizeof(void*))) + classSize;
			void* memory = AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = ((uint*)&handle)[0];
			destination[1] = 0; // No sync block initially

			return memory;
		}

		public static void* AllocateArray(RuntimeTypeHandle handle, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//   - Padding

			uint allocationSize = ((uint)(sizeof(void*)) * 3) + elements * elementSize;
			allocationSize = (allocationSize + 3) & ~3u;    // Align to 4-bytes boundary
			void* memory = AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = ((uint*)&handle)[0];
			destination[1] = 0; // No sync block initially
			destination[2] = elements;

			return memory;
		}

		public static void* AllocateString(RuntimeTypeHandle handle, uint length)
		{
			return AllocateArray(handle, sizeof(char), length);
		}

		#endregion Allocation

		#region (Un)Boxing

		public static void* Box8(RuntimeTypeHandle handle, byte value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);    // 4 for alignment
			*(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* Box16(RuntimeTypeHandle handle, ushort value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);    // 4 for alignment
			*(ushort*)(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* Box32(RuntimeTypeHandle handle, uint value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);
			*(uint*)(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* Box64(RuntimeTypeHandle handle, ulong value)
		{
			byte* memory = (byte*)AllocateObject(handle, 8);
			*(ulong*)(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* BoxR4(RuntimeTypeHandle handle, float value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);
			*(float*)(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* BoxR8(RuntimeTypeHandle handle, double value)
		{
			byte* memory = (byte*)AllocateObject(handle, 8);
			*(double*)(memory + ((uint)(sizeof(void*)) * 2)) = value;
			return memory;
		}

		public static void* Box(RuntimeTypeHandle handle, void* value, uint size)
		{
			byte* memory = (byte*)AllocateObject(handle, size);

			//MemoryCopy(memory + (uint)(sizeof(void*)) * 2, value, size);

			byte* dest = memory + (uint)(sizeof(void*) * 2);
			byte* src = (byte*)value;

			for (int i = 0; i < size; i++)
			{
				dest[i] = src[i];
			}

			return memory;
		}

		public static byte Unbox8(void* box)
		{
			return *((byte*)box + (uint)(sizeof(void*)) * 2);
		}

		public static ushort Unbox16(void* box)
		{
			return *(ushort*)((byte*)box + (uint)(sizeof(void*)) * 2);
		}

		public static uint* Unbox32(void* box)
		{
			return (uint*)((byte*)box + (uint)(sizeof(void*)) * 2);
		}

		public static ulong* Unbox64(void* box)
		{
			return (ulong*)((byte*)box + (uint)(sizeof(void*)) * 2);
		}

		public static void* Unbox(void* box, void* vt, uint size)
		{
			//MemoryCopy(vt, (byte*)box + (uint)(sizeof(void*)) * 2, size);

			byte* dest = (byte*)vt;
			byte* src = (byte*)box + (uint)(sizeof(void*) * 2);

			for (int i = 0; i < size; i++)
			{
				dest[i] = src[i];
			}

			return vt;
		}

		#endregion (Un)Boxing

		#region Memory Manipulation

		public static void MemoryCopy(void* dest, void* src, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		public static void MemorySet(void* dest, byte value, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		public static void MemoryClear(void* dest, uint count)
		{
			// PLUGGED
			throw new NotImplementedException();
		}

		#endregion Memory Manipulation

		#region Metadata

		internal static LinkedList<RuntimeAssembly> Assemblies = null;

		public static void Setup()
		{
			// Get AssemblyListTable and Assembly count
			Ptr assemblyListTable = Intrinsic.GetAssemblyListTable();
			uint assemblyCount = (uint)assemblyListTable[0];
			assemblyListTable++;

			Assemblies = new LinkedList<RuntimeAssembly>();

			// Loop through and populate the array
			for (uint i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				MDAssemblyDefinition* ptr = (MDAssemblyDefinition*)(assemblyListTable[i]);
				Assemblies.AddLast(new RuntimeAssembly(ptr));
			}
		}

		#endregion Metadata

		public static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}
	}
}
