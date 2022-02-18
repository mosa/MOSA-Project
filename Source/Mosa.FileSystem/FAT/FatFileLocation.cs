// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class FatFileLocation : IFileLocation
	{
		public uint FirstCluster { get; set; }

		public uint DirectorySector { get; set; }

		public uint DirectorySectorIndex { get; set; }

		public bool IsValid { get; private set; }

		public bool IsDirectory { get; private set; }

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
			IsDirectory = directory;
		}
	}
}
