// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// Flags, which control the operation of the Mosa.Filesystem.VFS.PathResolver.
	/// </summary>
	[System.Flags]
	public enum PathResolutionFlags
	{
		/// <summary>
		/// No flags passed to control lookup.
		/// </summary>
		None = 0x00000000,

		/// <summary>
		/// Do not throw file not found exceptions along the way. Lookup functions return null instead.
		/// </summary>
		DoNotThrowNotFoundException = 0x00000002,

		/// <summary>
		/// Indicates flags, which can be passed to nested symbolic link lookups.
		/// </summary>
		SymLinkLookupSafe = 0x0000FFFF,

		/// <summary>
		/// Indicates that only the parent directory of the path is returned and the name of the final directory entry is returned back in path.
		/// </summary>
		RetrieveParent = 0x00010000,

		/// <summary>
		/// Specifies if Lookup functions follow symbolic links.
		/// </summary>
		DoNotFollowSymbolicLinks = 0x00020000,
	}
}