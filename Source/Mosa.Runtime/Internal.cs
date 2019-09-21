// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System;
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

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));

			if (Pointer.Size == 4)
			{
				Intrinsic.Store32(memory, Pointer.Size, 0);
			}
			else
			{
				Intrinsic.Store64(memory, Pointer.Size, 0);
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

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));

			if (Pointer.Size == 4)
			{
				Intrinsic.Store32(memory, Pointer.Size, 0);
				Intrinsic.Store32(memory, Pointer.Size * 2, elements);
			}
			else
			{
				Intrinsic.Store64(memory, Pointer.Size, 0);
				Intrinsic.Store64(memory, Pointer.Size * 2, elements);
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

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.Store8(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box16(RuntimeTypeHandle handle, ushort value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.Store16(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box32(RuntimeTypeHandle handle, uint value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.Store32(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Pointer Box64(RuntimeTypeHandle handle, ulong value)
		{
			var memory = AllocateObject(handle, Pointer.Size * 2);

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.Store64(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer BoxR4(RuntimeTypeHandle handle, float value)
		{
			var memory = AllocateObject(handle, Pointer.Size);

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.StoreR4(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer BoxR8(RuntimeTypeHandle handle, double value)
		{
			var memory = AllocateObject(handle, Pointer.Size * 2);

			Intrinsic.Store(memory, 0, new Pointer(handle.Value));
			Intrinsic.StoreR8(memory, Pointer.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Box(RuntimeTypeHandle handle, Pointer value, uint size)
		{
			var memory = AllocateObject(handle, size);

			MemoryCopy(memory + (Pointer.Size * 2), value, size);

			return memory;
		}

		public static Pointer Unbox8(Pointer box)
		{
			return box + (Pointer.Size * 2);
		}

		public static Pointer Unbox16(Pointer box)
		{
			return box + (Pointer.Size * 2);
		}

		public static Pointer Unbox32(Pointer box)
		{
			return box + (Pointer.Size * 2);
		}

		public static Pointer Unbox64(Pointer box)
		{
			return box + (Pointer.Size * 2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Pointer Unbox(Pointer box, Pointer vt, uint size)
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
				byte value = Intrinsic.Load8(src, i);
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemorySet(Pointer dest, byte value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemorySet(Pointer dest, ushort value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i = i + 2)
			{
				Intrinsic.Store16(dest, i, value);
			}
		}

		public static void MemorySet(Pointer dest, uint value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i = i + 4)
			{
				Intrinsic.Store32(dest, i, value);
			}
		}

		public static void MemoryClear(Pointer dest, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				Intrinsic.Store8(dest, i, 0);
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
			var objTypeDefinition = new TypeDefinition(Intrinsic.LoadPointer(o));
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
			var objTypeDefinition = new TypeDefinition(Intrinsic.LoadPointer(o));

			var bitmap = objTypeDefinition.Bitmap;

			if (bitmap.IsNull)
				return null;

			int index = interfaceSlot / 32;
			int bit = interfaceSlot % 32;

			uint value = Intrinsic.Load32(bitmap, index * 4);
			uint result = value & (uint)(1 << bit);

			if (result == 0)
				return null;

			return obj;
		}

		#endregion Virtual Machine

		public static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}
	}
}
