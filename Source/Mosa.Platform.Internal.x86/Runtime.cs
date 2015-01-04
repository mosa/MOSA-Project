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
using System.Collections.Generic;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class Runtime
	{
		internal const uint NativeIntSize = 4;

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

		internal static LinkedList<RuntimeAssembly> Assemblies;

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
			Assemblies = new LinkedList<RuntimeAssembly>();

			// Loop through and populate the array
			for (uint i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				uint* ptr = (uint*)(assemblyListTable[1 + i]);
				Assemblies.AddLast(new RuntimeAssembly(ptr));
			}

			foreach (RuntimeAssembly assembly in Assemblies)
			{
				assembly.Phase2();
			}
		}

		#endregion Metadata

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
			uint* _dest = (uint*)dest;
			uint s = size & 3;
			size >>= 2;
			uint value4 = (uint)((value << 24) + (value << 16) + (value << 8) + value);

			while (size > 0)
			{
				*_dest = value4;
				_dest++;
				size--;
			}

			byte* __dest = (byte*)_dest;
			while (s > 0)
			{
				*__dest = value;
				__dest++;
				s--;
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

		public static void DebugOutput(byte code)
		{
			Native.Out8(0xEA, code);
		}

		public static void DebugOutput(uint code)
		{
			Native.Out8(0xEB, (byte)((code >> 24) & 0xFF));
			Native.Out8(0xEB, (byte)((code >> 16) & 0xFF));
			Native.Out8(0xEB, (byte)((code >> 8) & 0xFF));
			Native.Out8(0xEB, (byte)(code & 0xFF));
		}

		public static void DebugOutput(string msg)
		{
			foreach (var c in msg)
			{
				Native.Out8(0xEC, (byte)c);
			}

			Native.Out8(0xEC, 0);
		}

		public static void DebugOutput(string msg, uint code)
		{
			DebugOutput(msg);
			DebugOutput(code);
		}

		public static void Fault(uint code)
		{
			DebugOutput(code);
		}

		public static uint GetMethodDefinition(uint address)
		{
			uint table = Native.GetMethodLookupTable();
			uint entries = Mosa.Internal.Native.Load32(table);

			table = table + 4;

			while (entries > 0)
			{
				uint addr = Mosa.Internal.Native.Load32(table);
				uint size = Mosa.Internal.Native.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return Mosa.Internal.Native.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return 0;
		}

		public static uint GetMethodDefinitionViaMethodExceptionLookup(uint address)
		{
			uint table = Native.GetMethodExceptionLookupTable();

			if (table == 0)
				return 0;

			uint entries = Mosa.Internal.Native.Load32(table);

			table = table + NativeIntSize;

			while (entries > 0)
			{
				uint addr = Mosa.Internal.Native.Load32(table);
				uint size = Mosa.Internal.Native.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return Mosa.Internal.Native.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return 0;
		}

		public static uint GetProtectedRegionEntryByAddress(uint address, uint exceptionType, uint methodDef)
		{
			//DebugOutput("===GetProtectedRegionEntryByAddress===");

			//DebugOutput("address:");
			//DebugOutput(address);
			//DebugOutput("exceptionType:");
			//DebugOutput(exceptionType);
			//DebugOutput("methodDef:");
			//DebugOutput(methodDef);

			uint table = Mosa.Internal.Native.Load32(methodDef, NativeIntSize * 6);

			if (table == 0)
				return 0;

			uint method = Mosa.Internal.Native.Load32(methodDef, NativeIntSize * 4);

			if (method == 0)
				return 0;

			//DebugOutput("table:");
			//DebugOutput(table);
			//DebugOutput("method:");
			//DebugOutput(method);

			uint offset = address - method;

			//DebugOutput("offset:");
			//DebugOutput(offset);

			uint entries = Mosa.Internal.Native.Load32(table);

			//DebugOutput("entries:");
			//DebugOutput(entries);

			table = table + 4;

			while (entries > 0)
			{
				//DebugOutput("entry:");
				//DebugOutput(entries);

				uint start = Mosa.Internal.Native.Load32(table, NativeIntSize * 0);
				uint end = Mosa.Internal.Native.Load32(table, NativeIntSize * 1);

				//DebugOutput("start:");
				//DebugOutput(start);

				//DebugOutput("start:");
				//DebugOutput(end);

				if ((offset >= start) && (offset < end))
				{
					uint type = Mosa.Internal.Native.Load32(table, NativeIntSize * 3);

					//DebugOutput("type:");
					//DebugOutput(type);

					if (type == 0)
					{
						//DebugOutput("entry found:");
						//DebugOutput(table);

						return table;
					}

					uint exceptiontype = Mosa.Internal.Native.Load32(table, NativeIntSize * 4);

					//DebugOutput("exceptiontype:");
					//DebugOutput(exceptiontype);

					if (exceptiontype == exceptionType)
					{
						//DebugOutput("entry found:");
						//DebugOutput(table);

						return table;
					}
				}

				table = table + (NativeIntSize * 5);

				entries--;
			}

			//DebugOutput("No entry found");

			return 0;
		}

		public static uint GetPreviousStackFrame(uint ebp)
		{
			return Mosa.Internal.Native.Load32(ebp);
		}

		public static uint GetStackFrame(uint depth)
		{
			uint ebp = Native.GetEBP();

			while (depth > 0)
			{
				depth--;

				ebp = GetPreviousStackFrame(ebp);

				if (ebp == 0)
					return 0;
			}

			return ebp;
		}

		public static uint GetReturnAddressFromStackFrame(uint stackframe)
		{
			return Mosa.Internal.Native.Load32(stackframe, NativeIntSize);
		}

		public static void SetReturnAddressForStackFrame(uint stackframe, uint value)
		{
			Native.Set32(stackframe + NativeIntSize, value);
		}

		public static string GetMethodDefinitionName(uint methodDef)
		{
			uint name = Mosa.Internal.Native.Load32(methodDef);

			return InitializeMetadataString((uint*)name);
		}

		public static uint GetMethodDefinitionFromStackFrameDepth(uint depth)
		{
			uint ebp = GetStackFrame(depth + 1);

			uint address = GetReturnAddressFromStackFrame(ebp);

			return GetMethodDefinition(address);
		}

		public static void ExceptionHandler()
		{
			// capture this register immediately
			uint exceptionObject = Native.GetExceptionRegister();

			uint stackFrame = GetStackFrame(1);

			for (; ; )
			{
				uint returnAdddress = GetReturnAddressFromStackFrame(stackFrame);

				if (returnAdddress == 0)
				{
					// hit the top of stack!
					Fault(0XBAD00002);
				}

				uint exceptionType = Mosa.Internal.Native.Load32(exceptionObject);

				uint methodDef = GetMethodDefinitionViaMethodExceptionLookup(returnAdddress);

				if (methodDef != 0)
				{
					uint protectedRegion = GetProtectedRegionEntryByAddress(returnAdddress - 1, exceptionType, methodDef);

					if (protectedRegion != 0)
					{
						// found handler for current method, call it

						uint methodStart = Mosa.Internal.Native.Load32(methodDef, NativeIntSize * 4);
						uint stackSize = Mosa.Internal.Native.Load32(methodDef, NativeIntSize * 3) & 0xFFFF; // lower 16-bits only
						uint handlerOffset = Mosa.Internal.Native.Load32(protectedRegion, NativeIntSize * 2);
						uint previousFrame = GetPreviousStackFrame(stackFrame);

						uint jumpTarget = methodStart + handlerOffset;
						uint newStack = previousFrame - stackSize;

						//DebugOutput(jumpTarget);
						//DebugOutput(stackSize);
						//DebugOutput(newStack);
						//DebugOutput(previousFrame);

						Native.FrameJump(jumpTarget, newStack, previousFrame);
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}

		}
	}
}
