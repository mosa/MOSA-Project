using CommandLine;
using Mosa.Utility.BootImage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.CreateBootImage
{
	class Options
	{
		private BootImageOptions _options;

		[Option('m', "mbr")]
		public string MBRFile
		{
			set { _options.MBROption = true; _options.MBRCode = File.ReadAllBytes(value); }
		}

		[Option('b', "boot")]
		public string BootCodeFile
		{
			set { _options.FatBootCode = File.ReadAllBytes(value); }
		}

		[Option("vhd")]
		public bool VHDImageFormat
		{
			set { _options.ImageFormat = ImageFormat.VHD; }
			get { return (_options.ImageFormat == ImageFormat.VHD); }
		}

		[Option("img")]
		public bool IMGImageFormat
		{
			set { _options.ImageFormat = ImageFormat.IMG; }
			get { return (_options.ImageFormat == ImageFormat.IMG); }
		}

		[Option("vdi")]
		public bool VDIImageFormat
		{
			set { _options.ImageFormat = ImageFormat.VDI; }
			get { return (_options.ImageFormat == ImageFormat.VDI); }
		}

		[Option("syslinux")]
		public bool Syslinux
		{
			set { _options.PatchSyslinuxOption = true; }
			get { return _options.PatchSyslinuxOption; }
		}

		[Option("guid")]
		public string GUID
		{
			set { _options.MediaGuid = new Guid(value); }
			get { return _options.MediaGuid.ToString(); }
		}

		[Option("snapguid")]
		public string SnapGUID
		{
			set { _options.MediaLastSnapGuid = new Guid(value); }
			get { return _options.MediaLastSnapGuid.ToString(); }
		}

		[Option("fat32")]
		public bool Fat32FileSystem
		{
			set { _options.FileSystem = FileSystem.FAT32; }
			get { return (_options.FileSystem == FileSystem.FAT32); }
		}

		[Option("fat16")]
		public bool Fat16FileSystem
		{
			set { _options.FileSystem = FileSystem.FAT16; }
			get { return (_options.FileSystem == FileSystem.FAT16); }
		}

		[Option("fat12")]
		public bool Fat12FileSystem
		{
			set { _options.FileSystem = FileSystem.FAT12; }
			get { return (_options.FileSystem == FileSystem.FAT12); }
		}

		[Option("blocks")]
		public uint BlockCount
		{
			set { _options.BlockCount = Convert.ToUInt32(value); }
			get { return _options.BlockCount; }
		}

		[Option("volume")]
		public string VolumeLabel
		{
			set { _options.VolumeLabel = value; }
			get { return _options.VolumeLabel; }
		}

		public BootImageOptions BootImageOptions
		{
			get { return _options; }
		}

		public Options()
		{
			_options = new BootImageOptions();
		}
	}
}
