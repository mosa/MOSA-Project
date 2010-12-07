/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Runtime.CompilerServices;

using Mosa.Runtime.Vm;
using Mosa.Runtime.Memory;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Vm
{

	/// <summary>
	/// Provides central runtime entry points for various features.
	/// </summary>
	public static class Runtime
	{
		#region Data members

		/// <summary>
		/// The memory page manager of this runtime.
		/// </summary>
		public static IMemoryPageManager MemoryPageManager = new Win32MemoryPageManager();

		#endregion // Data members

		#region Internal Call Prototypes

		[VmCall(VmCall.AllocateObject)]
		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			//
			// An object has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - 0 .. n object data fields
			//
			ulong allocationSize = (ulong)((2 * nativeIntSize) + classSize);

			void* memory = MemoryPageManager.Allocate(IntPtr.Zero, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.WriteCombine).ToPointer();
			if (memory == null)
			{
				throw new OutOfMemoryException();
			}

			uint* destination = (uint*)memory;
			Memset((byte*)destination, 0, (int)allocationSize);
			destination[0] = (uint)methodTable;
			destination[1] = 0; // No sync block initially

			return memory;
		}

		/// <summary>
		/// This function requests allocation of an array.
		/// </summary>
		/// <param name="methodTable">Pointer to the array method table.</param>
		/// <param name="elementSize">The size of a single element in the method table.</param>
		/// <param name="elements">The number of elements to allocate of the type.</param>
		/// <returns>A ptr to the allocated memory.</returns>
		/// <remarks>
		/// The allocated object is not constructed, e.g. the caller must invoke
		/// the appropriate constructor in order to obtain a real object. The object header
		/// has been set.
		/// </remarks>
		[VmCall(VmCall.AllocateArray)]
		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			if (elements < 0)
			{
				throw new OverflowException();
			}

			// HACK: Add compiler architecture to the runtime
			uint nativeIntSize = 4;

			//
			// An array has the following memory layout:
			//   - IntPtr MTable
			//   - IntPtr SyncBlock
			//   - int length
			//   - ElementType[length] elements
			//
			uint allocationSize = (uint)(nativeIntSize + (elements * elementSize));

			void* memory = AllocateObject(methodTable, allocationSize);

			uint* destination = (uint*)memory;
			Memset((byte*)(destination + 3), 0, (int)allocationSize);
			destination[2] = elements;

			return memory;
		}

		/// <summary>
		/// Boxes the specified value type.
		/// </summary>
		/// <param name="valueType">Type of the value.</param>
		/// <returns>The boxed value type.</returns>
		[VmCall(VmCall.Box)]
		public static object Box(ValueType valueType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function performs the cast operation and type checking.
		/// </summary>
		/// <param name="obj">The object to cast.</param>
		/// <param name="typeHandle">Handle to the type to cast to.</param>
		/// <returns>
		/// The cast object if type checks were successful.
		/// </returns>
		[VmCall(VmCall.Castclass)]
		public static object Castclass(object obj, UIntPtr typeHandle)
		{
			throw new InvalidCastException();
		}

		/// <summary>
		/// This function performs the isinst operation and type checking.
		/// </summary>
		/// <param name="obj">The object to cast.</param>
		/// <param name="typeHandle">Handle to the type to cast to.</param>
		/// <returns>
		/// The cast object if type checks were successful. Otherwise null.
		/// </returns>
		[VmCall(VmCall.IsInstanceOfType)]
		public static bool IsInstanceOfType(object obj, UIntPtr typeHandle)
		{
			return false;
		}

		/// <summary>
		/// Copies bytes From the source to destination.
		/// </summary>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source.</param>
		/// <param name="count">The number of bytes to copy.</param>
		[VmCall(VmCall.Memcpy)]
		public unsafe static void Memcpy(byte* destination, byte* source, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fills the destination with <paramref name="value"/>.
		/// </summary>
		/// <param name="destination">The destination.</param>
		/// <param name="value">The value.</param>
		/// <param name="count">The number of bytes to fill.</param>
		[VmCall(VmCall.Memset)]
		public unsafe static void Memset(byte* destination, byte value, int count)
		{
			// FIXME: Forward this to the architecture
		}

		/// <summary>
		/// Rethrows the current exception.
		/// </summary>
		[VmCall(VmCall.Rethrow)]
		public static void Rethrow()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Throws the specified exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		[VmCall(VmCall.Throw)]
		public static void Throw(object exception)
		{
		}

		/// <summary>
		/// Unboxes the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="valueType">The value type to unbox.</param>
		[VmCall(VmCall.Unbox)]
		public static void Unbox(object obj, ValueType valueType)
		{
			throw new NotImplementedException();
		}

		[VmCall(VmCall.Throw)]
		public static void ThrowException(uint eax, uint ecx, uint edx, uint ebx, uint esi, uint edi, uint ebp, object exception, uint eip, uint esp)
		{
		}

		#endregion // Virtual Machine Call Prototypes

	}
}
