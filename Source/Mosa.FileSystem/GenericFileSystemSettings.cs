// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem
{
	/// <summary>
	/// File system settings base class for formatting purposes.
	/// </summary>
	/// <remarks>
	/// This base class holds properties and data members common to most file systems. A specialized
	/// derived class should be created for specific file systems and its type should be returned from
	/// IFileSystemService.SettingsType to allow mkfs style commands to automate most processing.
	/// </remarks>
	public class GenericFileSystemSettings
	{
		#region Data members

		/// <summary>
		/// The volume label.
		/// </summary>
		public string VolumeLabel;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="GenericFileSystemSettings"/>.
		/// </summary>
		public GenericFileSystemSettings()
		{
			VolumeLabel = "New Volume";
		}

		#endregion Construction
	}
}
