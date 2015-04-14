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

using Mosa.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class Runtime
	{
		internal const uint NativeIntSize = 4;

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

			uint allocationSize = (2 * NativeIntSize) + classSize;
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

			uint allocationSize = (NativeIntSize * 3) + (uint)(elements * elementSize);
			allocationSize = (allocationSize + 3) & ~3u;	// Align to 4-bytes boundary
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

		#region Metadata

		internal static LinkedList<RuntimeAssembly> Assemblies;

		public static string InitializeMetadataString(uint* ptr)
		{
			return (string)Intrinsic.GetObjectFromAddress(ptr);
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
		}

		#endregion Metadata

		public static bool IsTypeInInheritanceChain(MetadataTypeStruct* typeDefinition, MetadataTypeStruct* chain)
		{
			while (chain != null)
			{
				if (chain == typeDefinition)
					return true;

				chain = chain->ParentType;
			}

			return false;
		}

		public static void* IsInstanceOfType(RuntimeTypeHandle handle, void* obj)
		{
			if (obj == null)
				return null;

			MetadataTypeStruct* typeDefinition = (MetadataTypeStruct*)((uint**)&handle)[0];

			MetadataTypeStruct* objTypeDefinition = (MetadataTypeStruct*)((uint*)obj)[0];

			if (IsTypeInInheritanceChain(typeDefinition, objTypeDefinition))
				return (void*)obj;

			return null;
		}

		public static void* IsInstanceOfInterfaceType(int interfaceSlot, void* obj)
		{
			MetadataTypeStruct* objTypeDefinition = (MetadataTypeStruct*)((uint*)obj)[0];

			if (objTypeDefinition == null)
				return null;

			uint* bitmap = objTypeDefinition->Bitmap;

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

		public static MetadataMethodStruct* GetMethodDefinition(uint address)
		{
			uint table = Native.GetMethodLookupTable();
			uint entries = Intrinsic.Load32(table);

			table = table + 4;

			while (entries > 0)
			{
				uint addr = Intrinsic.Load32(table);
				uint size = Intrinsic.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return (MetadataMethodStruct*)Intrinsic.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return null;
		}

		public static MetadataMethodStruct* GetMethodDefinitionViaMethodExceptionLookup(uint address)
		{
			uint table = Native.GetMethodExceptionLookupTable();

			if (table == 0)
				return null;

			uint entries = Intrinsic.Load32(table);

			table = table + NativeIntSize;

			while (entries > 0)
			{
				uint addr = Intrinsic.Load32(table);
				uint size = Intrinsic.Load32(table, NativeIntSize);

				if (address >= addr && address < addr + size)
				{
					return (MetadataMethodStruct*)Intrinsic.Load32(table, NativeIntSize * 2);
				}

				table = table + (NativeIntSize * 3);

				entries--;
			}

			return null;
		}

		public static MetadataPRDefinitionStruct* GetProtectedRegionEntryByAddress(uint address, MetadataTypeStruct* exceptionType, MetadataMethodStruct* methodDef)
		{
			var protectedRegionTable = methodDef->ProtectedRegionTable;

			if (protectedRegionTable == null)
				return null;

			uint method = (uint)methodDef->Method;

			if (method == 0)
				return null;

			uint offset = address - method;
			int entries = protectedRegionTable->NumberOfRegions;

			int entry = 0;
			MetadataPRDefinitionStruct* protectedRegionDef = null;
			uint currentStart = uint.MinValue;
			uint currentEnd = uint.MaxValue;
			while (entry < entries)
			{
				var prDef = MetadataPRTableStruct.GetProtecteRegionDefinitionAddress(protectedRegionTable, (uint)entry);

				uint start = prDef->StartOffset;
				uint end = prDef->EndOffset;

				if ((offset >= start) && (offset < end) && (start >= currentStart) && (end < currentEnd))
				{
					var handlerType = prDef->HandlerType;

					// If the handler is a Finally clause, accept without testing
					if (handlerType == ExceptionHandlerType.Finally)
					{
						protectedRegionDef = prDef;
						currentStart = start;
						currentEnd = end;
						entry++;
						continue;
					}

					var exType = prDef->ExceptionType;

					// If the handler is a Exception clause, accept if the exception Type
					// is in the is within the inhertiance chain of the exception object
					if (handlerType == ExceptionHandlerType.Exception && IsTypeInInheritanceChain(exType, exceptionType))
					{
						protectedRegionDef = prDef;
						currentStart = start;
						currentEnd = end;
						entry++;
						continue;
					}
				}

				entry++;
			}

			return protectedRegionDef;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetPreviousStackFrame(uint ebp)
		{
			if (ebp < 0x1000)
				return 0;
			return Intrinsic.Load32(ebp);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetStackFrame(uint depth)
		{
			return GetStackFrame(depth, 0);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetStackFrame(uint depth, uint ebp)
		{
			if (ebp == 0)
				ebp = Native.GetEBP();

			while (depth > 0)
			{
				depth--;

				ebp = GetPreviousStackFrame(ebp);

				if (ebp == 0)
					return 0;
			}

			return ebp;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static uint GetReturnAddressFromStackFrame(uint stackframe)
		{
			if (stackframe < 0x1000)
				return 0;
			return Intrinsic.Load32(stackframe, NativeIntSize);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SetReturnAddressForStackFrame(uint stackframe, uint value)
		{
			Native.Set32(stackframe + NativeIntSize, value);
		}

		public static string GetMethodDefinitionName(MetadataMethodStruct* methodDef)
		{
			return InitializeMetadataString(methodDef->Name);
		}

		public static MetadataMethodStruct* GetMethodDefinitionFromStackFrameDepth(uint depth)
		{
			return GetMethodDefinitionFromStackFrameDepth(depth, 0);
		}

		public static MetadataMethodStruct* GetMethodDefinitionFromStackFrameDepth(uint depth, uint ebp)
		{
			if (ebp == 0)
				ebp = Native.GetEBP();

			ebp = GetStackFrame(depth + 0, ebp);

			uint address = GetReturnAddressFromStackFrame(ebp);
			return GetMethodDefinition(address);
		}

		public static SimpleStackTraceEntry GetStackTraceEntry(uint depth, uint ebp, uint eip)
		{
			var entry = new SimpleStackTraceEntry();

			uint address;
			if (depth == 0 && eip != 0)
				address = eip;
			else
			{
				if (ebp == 0)
					ebp = Native.GetEBP();

				if (eip != 0)
					depth--;
				//else
				//	depth += 3;

				ebp = GetStackFrame(depth, ebp);

				address = GetReturnAddressFromStackFrame(ebp);
			}

			var methodDef = GetMethodDefinition(address);

			if (methodDef == null)
				return entry;

			entry.MethodDefinition = methodDef;
			entry.Offset = address - (uint)(methodDef->Method);

			return entry;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
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

				var exceptionType = (MetadataTypeStruct*)Intrinsic.Load32(exceptionObject);

				var methodDef = GetMethodDefinitionViaMethodExceptionLookup(returnAdddress);

				if (methodDef != null)
				{
					var protectedRegion = GetProtectedRegionEntryByAddress(returnAdddress - 1, exceptionType, methodDef);

					//DebugOutput(((uint)protectedRegion));

					if (protectedRegion != null)
					{
						// found handler for current method, call it

						uint methodStart = (uint)methodDef->Method;
						uint handlerOffset = protectedRegion->HandlerOffset;
						uint jumpTarget = methodStart + handlerOffset;

						uint stackSize = methodDef->StackSize & 0xFFFF; // lower 16-bits only
						uint previousFrame = GetPreviousStackFrame(stackFrame);
						uint newStack = previousFrame - stackSize;

						//DebugOutput("5x:");
						//DebugOutput(jumpTarget);
						//DebugOutput(stackSize);
						//DebugOutput(newStack);
						//DebugOutput(previousFrame);

						//var method = GetMethodDefinition(jumpTarget);

						//DebugOutput(jumpTarget - methodStart);

						//string caller = Runtime.GetMethodDefinitionName(method);

						//DebugOutput(caller);

						Native.FrameJump(jumpTarget, newStack, previousFrame);
					}
				}

				// no handler in method, go up the stack
				stackFrame = GetPreviousStackFrame(stackFrame);
			}
		}
	}

	/// <summary>
	/// Holds information about a single stacktrace entry
	/// </summary>
	public struct SimpleStackTraceEntry
	{
		private string methodName;
		public unsafe MetadataMethodStruct* MethodDefinition;
		public uint Offset;

		unsafe public string MethodName
		{
			get
			{
				if (MethodDefinition == null)
					return null;
				if (methodName == null)
					methodName = Runtime.GetMethodDefinitionName(MethodDefinition);
				return methodName;
			}
		}

		/// <summary>
		/// Returns a human readable text of this entry
		/// </summary>
		/// <returns></returns>
		unsafe public StringBuffer ToStringBuffer()
		{
			var buf = new StringBuffer();

			buf.Append("0x");
			buf.Append((uint)MethodDefinition->Method, "X");
			buf.Append("+0x");
			buf.Append(Offset, "X");
			buf.Append(" ");

			var idx = MethodName.IndexOf(' ') + 1; //Skip return type
			buf.Append(MethodName, idx);
			return buf;
		}

		/// <summary>
		/// Skip defines, if this entry should be displayed, or not.
		/// </summary>
		public bool Skip
		{
			get
			{
				{
					if (!Valid) return true;
					if (MethodName == null)
						return true;
					return MethodName.IndexOf("System.Void Mosa.Kernel.x86.Panic::") >= 0;
				}
			}
		}

		public bool Valid
		{
			get { return MethodName != null; }
		}
	}
}
