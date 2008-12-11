/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.FileSystem.FATFileSystem
{
	/// <summary>
	/// 
	/// </summary>
	public class DirectoryEntryLocation
	{
		/// <summary>
		/// 
		/// </summary>
		public bool Valid;

		/// <summary>
		/// 
		/// </summary>
		public uint StartCluster;

		/// <summary>
		///  
		/// </summary>
		public uint DirectorySector;

		/// <summary>
		/// 
		/// </summary>
		public uint DirectorySectorIndex;

		/// <summary>
		/// 
		/// </summary>
		private bool directory;

		/// <summary>
		/// 
		/// </summary>
		public bool IsDirectory { get { return directory; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectoryEntryLocation"/> class.
		/// </summary>
		public DirectoryEntryLocation() { this.Valid = false; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectoryEntryLocation"/> class.
		/// </summary>
		/// <param name="startCluster">The start cluster.</param>
		/// <param name="directorySector">The directory sector.</param>
		/// <param name="directoryIndex">Index of the directory.</param>
		/// <param name="directory">if set to <c>true</c> [directory].</param>
		public DirectoryEntryLocation(uint startCluster, uint directorySector, uint directoryIndex, bool directory)
		{
			this.Valid = true;
			this.StartCluster = startCluster;
			this.DirectorySector = directorySector;
			this.DirectorySectorIndex = directoryIndex;
			this.directory = directory;
		}
	}

}
