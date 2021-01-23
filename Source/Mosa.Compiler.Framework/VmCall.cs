// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// An enumeration used to identify icalls provided by the MOSA VM.
	/// </summary>
	public enum VmCall
	{
		/// <summary>
		/// Allocates memory for a new object instance.
		/// </summary>
		AllocateObject,

		/// <summary>
		/// Allocates and initializes a new array.
		/// </summary>
		AllocateArray,

		/// <summary>
		/// Allocates a string object.
		/// </summary>
		AllocateString,

		/// <summary>
		/// Boxes a value type.
		/// </summary>
		Box32,

		Box64,
		Box,

		BoxR4,
		BoxR8,

		/// <summary>
		/// Casts an object references to a specific type.
		/// </summary>
		Castclass,

		/// <summary>
		/// Loads the address of a function.
		/// </summary>
		GetFunctionPtr,

		/// <summary>
		/// Retrieves a handle for the specified token.
		/// </summary>
		GetHandleForToken,

		/// <summary>
		/// Loads the address of a virtual function.
		/// </summary>
		GetVirtualFunctionPtr,

		/// <summary>
		/// Determines the instance of the type.
		/// </summary>
		IsInstanceOfType,

		/// <summary>
		/// Determines the instance of the interface type.
		/// </summary>
		IsInstanceOfInterfaceType,

		/// <summary>
		/// The method call represents a runtime defined memory copy method.
		/// </summary>
		/// <remarks>
		/// The memcpy method is similar to the memcpy function in C runtime libraries. It copies the
		/// specified number of bytes From a source to a destination block.
		/// </remarks>
		MemoryCopy,

		/// <summary>
		/// The method call represents a runtime defined memory set method.
		/// </summary>
		/// <remarks>
		/// The memset method is similar to the memset function in C runtime libraries. It fills a block
		/// of memory with a specific value.
		/// </remarks>
		MemorySet,

		/// <summary>
		/// Rethrows the given exception.
		/// </summary>
		Rethrow,

		/// <summary>
		/// Throws the given exception.
		/// </summary>
		Throw,

		/// <summary>
		/// Unboxes a value type.
		/// </summary>
		Unbox,

		UnboxAny,
	}
}
