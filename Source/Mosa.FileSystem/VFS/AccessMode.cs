// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// The access mode enumeration is used to determine accessibility of the file by the immediate caller. See Mosa.VFS.VirtualFileSystem.Access(System.String,Mosa.VFS.AccessMode) for more information.
	/// </summary>
	[System.Flags]
	public enum AccessMode
	{
		/// <summary>
		/// Check if the caller has the right to traverse the resource.
		/// </summary>
		Traverse,

		/// <summary>
		/// Check if the caller has read access to the resource.
		/// </summary>
		Read,

		/// <summary>
		/// Check if the caller has write access to the resource.
		/// </summary>
		Write,

		/// <summary>
		/// Check if the caller has delete access to the resoure.
		/// </summary>
		Delete,

		/// <summary>
		/// Check if the caller has execute permissions for the resource.
		/// </summary>
		Execute,

		/// <summary>
		/// Check if the named resource is available.
		/// </summary>
		Exists
	}
}