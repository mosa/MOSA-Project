// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class FatFileLocation
	{
		/// <summary>
		///
		/// </summary>
		public bool IsValid;

		/// <summary>
		///
		/// </summary>
		public uint FirstCluster;

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
		/// Initializes a new instance of the <see cref="FatFileLocation"/> class.
		/// </summary>
		public FatFileLocation()
		{
			IsValid = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FatFileLocation"/> class.
		/// </summary>
		/// <param name="startCluster">The start cluster.</param>
		/// <param name="directorySector">The directory sector.</param>
		/// <param name="directoryIndex">Index of the directory.</param>
		/// <param name="directory">if set to <c>true</c> [directory].</param>
		public FatFileLocation(uint startCluster, uint directorySector, uint directoryIndex, bool directory)
		{
			IsValid = true;
			FirstCluster = startCluster;
			DirectorySector = directorySector;
			DirectorySectorIndex = directoryIndex;
			this.directory = directory;
		}
	}
}
