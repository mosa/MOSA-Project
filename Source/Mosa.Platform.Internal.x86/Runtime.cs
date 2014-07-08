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
using System.Runtime.InteropServices;
using System.Reflection;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class Runtime
	{
		private const uint nativeIntSize = 4;

		#region Allocation

		// This method will be plugged by "Mosa.Kernel.x86.KernelMemory.AllocateMemory"
		private static uint AllocateMemory(uint size)
		{
			return 0;
		}

		public static void* AllocateObject(RuntimeTypeHandle* typeDefinition, uint classSize)
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

		public static void* AllocateArray(RuntimeTypeHandle* typeDefinition, uint elementSize, uint elements)
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

		public static void* AllocateString(RuntimeTypeHandle* typeDefinition, uint length)
		{
			return AllocateArray(typeDefinition, 2, length);
		}

		#endregion Allocation

		#region Metadata Lookup

		[StructLayout(LayoutKind.Sequential)]
		private struct MetadataVector
		{
			public uint* Pointer;
			public string Name;
		}

		private static MetadataVector[] Assemblies;
		private static MetadataVector[] Types;

		public static string Metadata_InitializeString(uint* ptr)
		{
			int length = (int)(ptr[0]);
			return new string((sbyte*)++ptr, 0, length);
		}

		public static void Metadata_InitializeLookup()
		{
			// Get AssemblyListTable and Assembly count
			uint* assemblyListTable = Native.GetAssemblyListTable();
			uint assemblyCount = assemblyListTable[0];

			// Create new MetadataVector array for assemblies using count
			Assemblies = new MetadataVector[assemblyCount];

			// Type count will be solved during population of assemblies MetadataVector array
			uint typeCount = 0;

			// Loop through and populate the array
			for (uint i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				uint* ptr = (uint*)(assemblyListTable[1 + i]);

				// Populate the MetadataVector
				Assemblies[i].Pointer = ptr;
				Assemblies[i].Name = Metadata_InitializeString((uint*)(ptr[0]));

				// Increment the type count
				typeCount += ptr[3];
			}

			// Create new MetadataVector array for types using count
			Types = new MetadataVector[typeCount];

			// Reset the type count for use as offset
			typeCount = 0;

			// Iterate through the assemblies to get types
			for (uint i = 0; i < assemblyCount; i++)
			{
				uint* assemblyPtr = Assemblies[i].Pointer;
				uint assemblyTypeCount = assemblyPtr[3];

				// Loop through and populate the types MetadataVector array
				for (uint j = 0; j < assemblyTypeCount; j++)
				{
					// Get the pointer to the Type Metadata
					uint* typePtr = (uint*)(assemblyPtr[4 + j]);

					// Populate the MetadataVector
					Types[typeCount + j].Pointer = typePtr;
					Types[typeCount + j].Name = Metadata_InitializeString((uint*)(typePtr[0]));
				}

				// Increment the type count
				typeCount += assemblyTypeCount;
			}
		}

		#endregion Metadata Lookup

		#region Metadata - Type

		public unsafe static class Type
		{
			public static RuntimeTypeHandle* GetHandleFromObject(void* obj)
			{
				// TypeDefinition is located at the beginning of object (i.e. *obj )
				return (RuntimeTypeHandle*)((uint*)obj)[0];
			}

			public static RuntimeTypeHandle* GetHandleByName(string typeName, bool ignoreCase)
			{
				// If we are ignoring casing then lower the casing
				if (ignoreCase)
					typeName = typeName.ToLower();

				// Loop through all the types and check to see if we have a match
				for (uint i = 0; i < Types.Length; i++)
				{
					// Get the name
					// FIXME: for some reason using the name in the MetadataVector doesn't work
					//string name = Types[i].Name;
					string name = Metadata_InitializeString((uint*)Types[i].Pointer[0]);

					// Compare the length, if not a match then skip
					if (typeName.Length != name.Length)
						continue;

					// If we are ignoring casing then lower the casing
					if (ignoreCase)
						name = name.ToLower();

					// Compare name with desired name, if not a match then continue
					if (typeName != name)
						continue;

					// Once we have a match return result
					return (RuntimeTypeHandle*)Types[i].Pointer;
				}

				// If we didn't find anything then return a null pointer
				return (RuntimeTypeHandle*)0;
			}

			public static string GetFullName(RuntimeTypeHandle* typeDefinition)
			{
				// Name pointer located at the beginning of the TypeDefinition
				uint* ptr = (uint*)typeDefinition;
				return Metadata_InitializeString((uint*)ptr[0]);
			}

			public static TypeAttributes GetAttributes(RuntimeTypeHandle* typeDefinition)
			{
				// Type attributes located at 3rd position of TypeDefinition
				return (TypeAttributes)((uint*)typeDefinition)[2];
			}
		}

		#endregion Metadata - Type

		public static void InitializeArray(uint* array, RuntimeFieldHandle* fieldHandle)
		{
			uint* fieldHandlePtr = (uint*)fieldHandle;
			byte* arrayElements = (byte*)(array + 3);

			// See FieldDefinition for format of field handle
			byte* fieldData = (byte*)*(fieldHandlePtr + 4);
			uint dataLength = *(fieldHandlePtr + 5);
			while (dataLength > 0)
			{
				*arrayElements = *fieldData;
				arrayElements++;
				fieldData++;
				dataLength--;
			}
		}

		public static void* IsInstanceOfType(RuntimeTypeHandle* typeDefinition, void* obj)
		{
			if (obj == null)
				return null;

			RuntimeTypeHandle* objTypeDefinition = (RuntimeTypeHandle*)((uint*)obj)[0];

			while (objTypeDefinition != null)
			{
				if (objTypeDefinition == typeDefinition)
					return (void*)obj;

				objTypeDefinition = (RuntimeTypeHandle*)((uint*)objTypeDefinition)[5];
			}

			return null;
		}

		public static void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			RuntimeTypeHandle* objTypeDefinition = (RuntimeTypeHandle*)((uint*)obj)[0];

			if (objTypeDefinition == null)
				return null;

			uint bitmap = ((uint*)(objTypeDefinition))[9];

			if (bitmap == 0)
				return null;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;
			uint value = ((uint*)bitmap)[index];
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return null;

			return obj;
		}

		public static void* Castclass(RuntimeTypeHandle* typeDefinition, void* obj)
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

		#region (Un)Boxing

		public static void* Box8(RuntimeTypeHandle* typeDefinition, byte value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);	// 4 for alignment
			*(byte*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box16(RuntimeTypeHandle* typeDefinition, ushort value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);	// 4 for alignment
			*(ushort*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box32(RuntimeTypeHandle* typeDefinition, uint value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 4);
			*(uint*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box64(RuntimeTypeHandle* typeDefinition, ulong value)
		{
			byte* memory = (byte*)AllocateObject(typeDefinition, 8);
			*(ulong*)(memory + (nativeIntSize * 2)) = value;
			return memory;
		}

		public static void* Box(RuntimeTypeHandle* typeDefinition, void* value, uint size)
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

		#endregion (Un)Boxing

		public static void Throw(uint something)
		{
		}

		public static uint GetSizeOfObject(void* obj)
		{
			RuntimeTypeHandle* typeDefinition = Runtime.Type.GetHandleFromObject(obj);

			return GetSizeOfType(typeDefinition);
		}

		public static uint GetSizeOfType(RuntimeTypeHandle* typeDefinition)
		{
			uint sizeOf = ((uint*)typeDefinition)[3];

			return sizeOf;
		}
	}
}