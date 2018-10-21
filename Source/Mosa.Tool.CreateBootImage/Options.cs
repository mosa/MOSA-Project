// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.CreateBootImage
{
	internal class Options
	{
		private BootImageOptions options;

		[Option('m', "mbr")]
		public string MBRFile
		{
			set { options.MBROption = true; options.MBRCode = File.ReadAllBytes(value); }
		}

		[Option('b', "boot")]
		public string BootCodeFile
		{
			set { options.FatBootCode = File.ReadAllBytes(value); }
		}

		[Option("vhd")]
		public bool VHDImageFormat
		{
			set { options.ImageFormat = ImageFormat.VHD; }
			get { return (options.ImageFormat == ImageFormat.VHD); }
		}

		[Option("img")]
		public bool IMGImageFormat
		{
			set { options.ImageFormat = ImageFormat.IMG; }
			get { return (options.ImageFormat == ImageFormat.IMG); }
		}

		[Option("vdi")]
		public bool VDIImageFormat
		{
			set { options.ImageFormat = ImageFormat.VDI; }
			get { return (options.ImageFormat == ImageFormat.VDI); }
		}

		[Option("syslinux")]
		public bool Syslinux
		{
			set { options.PatchSyslinuxOption = true; }
			get { return options.PatchSyslinuxOption; }
		}

		[Option("guid")]
		public string GUID
		{
			set { options.MediaGuid = new Guid(value); }
			get { return options.MediaGuid.ToString(); }
		}

		[Option("snapguid")]
		public string SnapGUID
		{
			set { options.MediaLastSnapGuid = new Guid(value); }
			get { return options.MediaLastSnapGuid.ToString(); }
		}

		[Option("fat32")]
		public bool Fat32FileSystem
		{
			set { options.FileSystem = FileSystem.FAT32; }
			get { return (options.FileSystem == FileSystem.FAT32); }
		}

		[Option("fat16")]
		public bool Fat16FileSystem
		{
			set { options.FileSystem = FileSystem.FAT16; }
			get { return (options.FileSystem == FileSystem.FAT16); }
		}

		[Option("fat12")]
		public bool Fat12FileSystem
		{
			set { options.FileSystem = FileSystem.FAT12; }
			get { return (options.FileSystem == FileSystem.FAT12); }
		}

		[Option("blocks")]
		public uint BlockCount
		{
			set { options.BlockCount = Convert.ToUInt32(value); }
			get { return options.BlockCount; }
		}

		[Option("volume")]
		public string VolumeLabel
		{
			set { options.VolumeLabel = value; }
			get { return options.VolumeLabel; }
		}

		[Option("file", Separator = ',', HelpText = "A list of files which will be included in the output image file.")]
		public IEnumerable<string> RawFileList
		{
			set
			{
				var list = (IList<string>)value;

				for (int x = 0; x < list.Count; x++)
				{
					string path = list[x];
					if (Path.IsPathRooted(path))
					{
						if (x + 1 < list.Count) //Is there a next entry?
						{
							if (Path.IsPathRooted(list[x + 1]))
							{
								options.IncludeFiles.Add(new IncludeFile(path));
							}
							else //If the next is not rooted, it's the new name of the files
							{
								options.IncludeFiles.Add(new IncludeFile(path, list[++x]));
							}
						}
						else
						{
							options.IncludeFiles.Add(new IncludeFile(path));
						}
					}
					else
					{
						var currDir = Environment.CurrentDirectory;
						options.IncludeFiles.Add(new IncludeFile(Path.GetFullPath(Path.Combine(currDir, path))));
					}
				}
			}
		}

		public BootImageOptions BootImageOptions
		{
			get { return options; }
		}

		public Options()
		{
			options = new BootImageOptions();
		}
	}
}
