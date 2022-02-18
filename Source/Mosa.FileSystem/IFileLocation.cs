// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem
{
	public interface IFileLocation
	{
		uint FirstCluster { get; set; }

		uint DirectorySector { get; set; }

		uint DirectorySectorIndex { get; set; }

		bool IsValid { get; }

		bool IsDirectory { get; }
	}
}
