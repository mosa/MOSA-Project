// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	/// Specifies the CLI stack type of a type reference.
	/// </summary>
	public enum StackTypeCode
	{
		/// <summary>
		/// Unknown stack type. This most likely hasn't been processed yet.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// CLI stack type is int32.
		/// </summary>
		Int32 = 1,

		/// <summary>
		/// CLI stack type is int64.
		/// </summary>
		Int64 = 2,

		/// <summary>
		/// CLI stack type is native int.
		/// </summary>
		N = 3,

		/// <summary>
		/// CLI stack type is native floating point.
		/// </summary>
		F = 4,

		/// <summary>
		/// CLI Stack type is managed pointer.
		/// </summary>
		ManagedPointer = 5,

		/// <summary>
		/// CLI Stack type is unmanaged pointer.
		/// </summary>
		UnmanagedPointer = 6,

		/// <summary>
		/// CLI stack type is object reference.
		/// </summary>
		O = 7,
	}
}
