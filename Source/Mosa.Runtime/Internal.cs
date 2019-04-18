// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public static class Internal
	{
		#region Allocation

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr AllocateObject(RuntimeTypeHandle handle, uint classSize)
		{
			// An object has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields

			var memory = GC.AllocateObject((2 * (uint)(IntPtr.Size)) + classSize);

			Intrinsic.Store(memory, 0, handle.Value);

			if (IntPtr.Size == 4)
			{
				Intrinsic.Store32(memory, IntPtr.Size, 0);
			}
			else
			{
				Intrinsic.Store64(memory, IntPtr.Size, 0);
			}

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr AllocateObject(RuntimeTypeHandle handle, int classSize)
		{
			return AllocateObject(handle, (uint)classSize);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr AllocateArray(RuntimeTypeHandle handle, uint elementSize, uint elements)
		{
			// An array has the following memory layout:
			//   - IntPtr TypeDef
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//   - Padding

			uint allocationSize = ((uint)(IntPtr.Size) * 3) + (elements * elementSize);
			allocationSize = (allocationSize + 3) & ~3u;    // Align to 4-bytes boundary

			var memory = GC.AllocateObject(allocationSize);

			Intrinsic.Store(memory, 0, handle.Value);

			if (IntPtr.Size == 4)
			{
				Intrinsic.Store32(memory, IntPtr.Size, 0);
				Intrinsic.Store32(memory, IntPtr.Size * 2, elements);
			}
			else
			{
				Intrinsic.Store64(memory, IntPtr.Size, 0);
				Intrinsic.Store64(memory, IntPtr.Size * 2, elements);
			}

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr AllocateString(RuntimeTypeHandle handle, uint length)
		{
			return AllocateArray(handle, sizeof(char), length);
		}

		#endregion Allocation

		#region (Un)Boxing

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Box8(RuntimeTypeHandle handle, byte value)
		{
			var memory = AllocateObject(handle, IntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store8(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Box16(RuntimeTypeHandle handle, ushort value)
		{
			var memory = AllocateObject(handle, IntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store16(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Box32(RuntimeTypeHandle handle, uint value)
		{
			var memory = AllocateObject(handle, IntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store32(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Box64(RuntimeTypeHandle handle, ulong value)
		{
			var memory = AllocateObject(handle, IntPtr.Size * 2);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.Store64(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr BoxR4(RuntimeTypeHandle handle, float value)
		{
			var memory = AllocateObject(handle, IntPtr.Size);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.StoreR4(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr BoxR8(RuntimeTypeHandle handle, double value)
		{
			var memory = AllocateObject(handle, IntPtr.Size * 2);

			Intrinsic.Store(memory, 0, handle.Value);
			Intrinsic.StoreR8(memory, IntPtr.Size * 2, value);

			return memory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Box(RuntimeTypeHandle handle, IntPtr value, uint size)
		{
			var memory = AllocateObject(handle, size);

			MemoryCopy(memory + (IntPtr.Size * 2), value, size);

			return memory;
		}

		public static IntPtr Unbox8(IntPtr box)
		{
			return box + (IntPtr.Size * 2);
		}

		public static IntPtr Unbox16(IntPtr box)
		{
			return box + (IntPtr.Size * 2);
		}

		public static IntPtr Unbox32(IntPtr box)
		{
			return box + (IntPtr.Size * 2);
		}

		public static IntPtr Unbox64(IntPtr box)
		{
			return box + (IntPtr.Size * 2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr Unbox(IntPtr box, IntPtr vt, uint size)
		{
			MemoryCopy(vt, box + (IntPtr.Size * 2), size);

			return vt;
		}

		#endregion (Un)Boxing

		#region Memory Manipulation

		public static void MemoryCopy(IntPtr dest, IntPtr src, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				byte value = Intrinsic.Load8(src, i);
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemorySet(IntPtr dest, byte value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemoryClear(IntPtr dest, uint count)
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

		public static IntPtr IsInstanceOfType(RuntimeTypeHandle handle, object obj)
		{
			if (obj == null)
				return IntPtr.Zero;

			var o = Intrinsic.GetObjectAddress(obj);
			var objTypeDefinition = new TypeDefinition(Intrinsic.LoadPointer(o));
			var typeDefinition = new TypeDefinition(handle.Value);

			if (IsTypeInInheritanceChain(typeDefinition, objTypeDefinition))
				return o;

			return IntPtr.Zero;
		}

		public static object IsInstanceOfInterfaceType(int interfaceSlot, object obj)
		{
			if (obj == null)
				return null;

			var o = Intrinsic.GetObjectAddress(obj);
			var objTypeDefinition = new TypeDefinition(Intrinsic.LoadPointer(o));

			var bitmap = objTypeDefinition.Bitmap;

			if (bitmap == IntPtr.Zero)
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
