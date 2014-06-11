/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class Runtime
	{
		private const uint nativeIntSize = 4;

		// This method will be plugged by "Mosa.Kernel.x86.KernelMemory.AllocateMemory"
		private static uint AllocateMemory(uint size)
		{
			return 0;
		}

		public static void* AllocateObject(void* typeDefinition, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = (2 * nativeIntSize) + classSize;
			void* memory = (void*)AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = (uint)typeDefinition;
			destination[1] = 0; // No sync block initially

			return memory;
		}

		public static void* AllocateArray(void* typeDefinition, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//   - Padding

			uint allocationSize = (nativeIntSize * 3) + (uint)(elements * elementSize);
			allocationSize = (allocationSize + 3) & ~3u;	// Align to 4-bytes boundary
			void* memory = (void*)AllocateMemory(allocationSize);

			uint* destination = (uint*)memory;
			destination[0] = (uint)typeDefinition;
			destination[1] = 0; // No sync block initially
			destination[2] = elements;

			return memory;
		}

		public static void* AllocateString(void* typeDefinition, uint length)
		{
			return AllocateArray(typeDefinition, 2, length);
		}

		public static void* GetTypeHandle(void* obj)
		{
			// TypeDefinition is located at the beginning of object (i.e. *obj )
			return (void*)((uint*)obj)[0];
		}

		public static void InitializeArray(uint* array, uint* fieldHandle)
		{
			byte* arrayElements = (byte*)(array + 3);
			// See FieldDefinition for format of field handle
			byte* fieldData = (byte*)*(fieldHandle + 1);
			uint dataLength = *(fieldHandle + 2);
			while (dataLength > 0)
			{
				*arrayElements = *fieldData;
				arrayElements++;
				fieldData++;
				dataLength--;
			}
		}

		public static uint IsInstanceOfType(uint typeDefinition, uint obj)
		{
			if (obj == 0)
				return 0;

			uint objTypeDefinition = ((uint*)obj)[0];

			while (objTypeDefinition != 0)
			{
				if (objTypeDefinition == typeDefinition)
					return obj;

				objTypeDefinition = ((uint*)objTypeDefinition)[5];
			}

			return 0;
		}

		public static uint IsInstanceOfInterfaceType(int interfaceSlot, uint obj)
		{
			if (obj == 0)
				return 0;

			uint objTypeDefinition = ((uint*)obj)[0];

			// Check parent definitions as well to see if they implement the interface
			while (objTypeDefinition != 0)
			{
				uint bitmap = ((uint*)(objTypeDefinition))[8];

				if (bitmap == 0)
				{
					objTypeDefinition = ((uint*)objTypeDefinition)[5];
					continue;
				}

				int index = interfaceSlot / 32;
				int bit = interfaceSlot % 32;
				uint value = ((uint*)bitmap)[index];
				uint result = value & (uint)(1 << bit);

				if (result != 0)
					return obj;

				objTypeDefinition = ((uint*)objTypeDefinition)[5];
			}

			return 0;
		}

		public static uint Castclass(uint typeDefinition, uint obj)
		{
			//TODO: Fake result
			return obj;
		}

		// TODO: efficiency?
		public static void Memcpy(void* dest, void* src, uint count)
		{
			uint* _dest = (uint*)dest;
			uint* _src = (uint*)src;
			uint c = count & 3;
			count >>= 2;

			while (count > 0)
			{
				*_dest = *_src;
				_dest++;
				_src++;
				count--;
			}

			byte* __dest = (byte*)_dest;
			byte* __src = (byte*)_src;
			while (c > 0)
			{
				*__dest = *__src;
				__dest++;
				__src++;
				c--;
			}
		}

		public static void* Box8(void* typeDefinition, byte value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);	// 4 for alignment
			*(byte*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box16(void* typeDefinition, ushort value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);	// 4 for alignment
			*(ushort*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box32(void* typeDefinition, uint value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);
			*(uint*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box64(void* typeDefinition, ulong value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 8);
			*(ulong*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box(void* typeDefinition, void* value, uint size)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, size);
			Memcpy(memory + nativeIntSize * 2, value, size);
			return memory;
		}

		public static byte Unbox8(void* box)
		{
			return *(byte*)((byte*)box + nativeIntSize * 2);
		}

		public static ushort Unbox16(void* box)
		{
			return *(ushort*)((byte*)box + nativeIntSize * 2);
		}

		public static uint* Unbox32(void* box)
		{
			return (uint*)((byte*)box + nativeIntSize * 2);
		}

		public static ulong* Unbox64(void* box)
		{
			return (ulong*)((byte*)box + nativeIntSize * 2);
		}

		public static void* Unbox(void* box, void* vt, uint size)
		{
			Memcpy(vt, (byte*)box + nativeIntSize * 2, size);
			return vt;
		}

		public static void Throw(uint something)
		{
		}

		public static uint GetSizeOfObject(void* obj)
		{
			void* typeDefinition = (void*)((uint*)obj)[0];

			return GetSizeOfType((void*)typeDefinition);
		}

		public static uint GetSizeOfType(void* typeDefinition)
		{
			uint sizeOf = ((uint*)typeDefinition)[3];

			return sizeOf;
		}
	}
}