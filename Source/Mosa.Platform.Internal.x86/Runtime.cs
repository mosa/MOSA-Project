/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class Runtime
	{
		public const uint NativeIntSize = 4;

		#region Allocation

		// This method will be plugged by "Mosa.Kernel.x86.KernelMemory.AllocateMemory"
		public static uint AllocateMemory(uint size)
		{
			return 0;
		}

		public static void* AllocateObject(RuntimeTypeHandle handle, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			uint allocationSize = (2 * NativeIntSize) + classSize;
			void* memory = (void*)AllocateMemory(allocationSize);

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

			uint allocationSize = (NativeIntSize * 3) + (uint)(elements * elementSize);
			allocationSize = (allocationSize + 3) & ~3u;	// Align to 4-bytes boundary
			void* memory = (void*)AllocateMemory(allocationSize);

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

		#region Metadata

		internal static _Assembly[] Assemblies;

		public static string InitializeMetadataString(uint* ptr)
		{
			int length = (int)(ptr[0]);
			return new string((sbyte*)++ptr, 0, length);
		}

		public static void Setup()
		{
			// Get AssemblyListTable and Assembly count
			uint* assemblyListTable = (uint*)Native.GetAssemblyListTable();
			uint assemblyCount = assemblyListTable[0];

			// Create new MetadataVector array for assemblies using count
			Assemblies = new _Assembly[assemblyCount];

			// Loop through and populate the array
			for (uint i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				uint* ptr = (uint*)(assemblyListTable[1 + i]);
				Assemblies[i] = new _Assembly(ptr);
			}
		}

		#endregion Metadata

		public static void InitializeArray(uint* array, RuntimeFieldHandle handle)
		{
			uint* fieldDefinition = ((uint**)&handle)[0];
			byte* arrayElements = (byte*)(array + 3);

			// See FieldDefinition for format of field handle
			byte* fieldData = (byte*)*(fieldDefinition + 4);
			uint dataLength = *(fieldDefinition + 5);
			while (dataLength > 0)
			{
				*arrayElements = *fieldData;
				arrayElements++;
				fieldData++;
				dataLength--;
			}
		}

		public static void* IsInstanceOfType(RuntimeTypeHandle handle, void* obj)
		{
			if (obj == null)
				return null;

			MetadataTypeStruct* typeDefinition = (MetadataTypeStruct*)((uint**)&handle)[0];

			MetadataTypeStruct* objTypeDefinition = (MetadataTypeStruct*)((uint*)obj)[0];

			while (objTypeDefinition != null)
			{
				if (objTypeDefinition == typeDefinition)
					return (void*)obj;

				objTypeDefinition = (*objTypeDefinition).ParentType;
			}

			return null;
		}

		public static void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			MetadataTypeStruct* objTypeDefinition = (MetadataTypeStruct*)((uint*)obj)[0];

			if (objTypeDefinition == null)
				return null;

			uint* bitmap = (*objTypeDefinition).Bitmap;

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

		public static void Memset(void* dest, byte value, uint size)
		{
			byte* _dest = (byte*)dest;
			while (size > 0)
			{
				*_dest = value;
				_dest++;
				size--;
			}
		}

		#region (Un)Boxing

		public static void* Box8(RuntimeTypeHandle handle, byte value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);	// 4 for alignment
			*(byte*)(memory + (NativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box16(RuntimeTypeHandle handle, ushort value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);	// 4 for alignment
			*(ushort*)(memory + (NativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box32(RuntimeTypeHandle handle, uint value)
		{
			byte* memory = (byte*)AllocateObject(handle, 4);
			*(uint*)(memory + (NativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box64(RuntimeTypeHandle handle, ulong value)
		{
			byte* memory = (byte*)AllocateObject(handle, 8);
			*(ulong*)(memory + (NativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box(RuntimeTypeHandle handle, void* value, uint size)
		{
			byte* memory = (byte*)AllocateObject(handle, size);
			Memcpy(memory + NativeIntSize * 2, value, size);
			return memory;
		}

		public static byte Unbox8(void* box)
		{
			return *(byte*)((byte*)box + NativeIntSize * 2);
		}

		public static ushort Unbox16(void* box)
		{
			return *(ushort*)((byte*)box + NativeIntSize * 2);
		}

		public static uint* Unbox32(void* box)
		{
			return (uint*)((byte*)box + NativeIntSize * 2);
		}

		public static ulong* Unbox64(void* box)
		{
			return (ulong*)((byte*)box + NativeIntSize * 2);
		}

		public static void* Unbox(void* box, void* vt, uint size)
		{
			Memcpy(vt, (byte*)box + NativeIntSize * 2, size);
			return vt;
		}

		#endregion (Un)Boxing

		public static void Throw(uint something)
		{
		}

		public static void Fault(int code)
		{
			// TODO
		}

		public static void* _GetMethodDefinition(uint address)
		{
			int* table = (int*)Native.GetMethodLookupTable();
			uint entries = (uint)table[0];

			table = table + 4;

			while (entries > 0)
			{
				uint addr = (uint)table[0];
				uint size = (uint)table[1];

				if (addr >= address && addr + size < address)
				{
					return (void*)table[3];
				}

				table = table + 12;

				entries--;
			}

			return null;
		}

		public static void* _GetProtectedRegionEntryByAddress(uint address, uint exceptionType)
		{
			int* methodDefination = (int*)_GetMethodDefinition(address);

			if (methodDefination == null)
				Fault(0x000001);

			int* table = (int*)(methodDefination[7]);

			if (table == null)
				return null;

			uint entries = (uint)table[0];

			table = table + 4;

			while (entries > 0)
			{
				uint addr = (uint)table[1];
				uint size = (uint)table[2];
				uint type = (uint)table[4];

				if (address >= addr && address < addr + size)
				{
					if (type != 0 || type == exceptionType)
					{
						return (void*)addr;
					}
				}

				table = table + 12;

				entries--;
			}

			return null;
		}

		public static uint GetMethodDefinition(uint address)
		{
			uint table = Native.GetMethodLookupTable();
			uint entries = Native.Get32(table);

			table = table + 4;

			while (entries > 0)
			{
				uint addr = Native.Get32(table);
				uint size = Native.Get32(table + 4);

				if (address >= addr && address < addr + size)
				{
					return Native.Get32(table + 8);
				}

				table = table + 12;

				entries--;
			}

			return 0;
		}

		public static uint GetProtectedRegionEntryByAddress(uint address, uint exceptionType)
		{
			uint methodDefination = GetMethodDefinition(address);

			if (methodDefination == 0)
				Fault(0x000001);

			uint table = Native.Get32(methodDefination + (4 * 7));

			if (table == 0)
				return 0;

			uint entries = Native.Get32(table);

			table = table + 4;

			while (entries > 0)
			{
				uint addr = Native.Get32(table + 4);
				uint size = Native.Get32(table + 8);
				uint type = Native.Get32(table + 16);

				if (address >= addr && address < addr + size)
				{
					if (type != 0 || type == exceptionType)
					{
						return addr;
					}
				}

				table = table + 12;

				entries--;
			}

			return 0;
		}

		public static string GetCallerName()
		{
			uint ebp = Native.GetEBP();
			uint caller = Native.Get32(ebp + 4);
			uint methodDef = GetMethodDefinition(caller);

			uint name = Native.Get32(methodDef);

			string method = InitializeMetadataString((uint*)name);

			return method;
		}

		public static void HandlerException()
		{
			uint ebp = Native.GetEBP();
			uint caller = Native.Get32(ebp + 4);
			uint exceptionObject = Native.GetExceptionRegister();
			uint exceptionType = Native.Get32(exceptionObject);


		}
	}
}