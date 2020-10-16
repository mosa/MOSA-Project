// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mosa.Tool.CreateBootImage
{
	internal class Options
	{
		private BootImageOptions options;

		[Option('m', "mbr", HelpText = "MBR file")]
		public string MBRFile
		{
			set { options.MBROption = true; options.MBRCode = File.ReadAllBytes(value); }
		}

		[Option('b', "boot", HelpText = "FAT boot code file")]
		public string BootCodeFile
		{
			set { options.FatBootCode = File.ReadAllBytes(value); }
		}

		[Option('o', "out", Required = true, HelpText = "Output disk file name")]
		public string DiskImageFileName
		{
			set { options.DiskImageFileName = value; }
		}

		[Option("format", HelpText = "Disk image format [img|iso|vhd|vdi|vmdk]")]
		public string ImageFormat
		{
			set
			{
				options.ImageFormat = (ImageFormat)Enum.Parse(typeof(ImageFormat), value, true);
			}
		}

		[Option("syslinux")]
		public bool Syslinux
		{
			set { options.PatchSyslinuxOption = true; }
			get { return options.PatchSyslinuxOption; }
		}

		[Option("filesystem", HelpText = "FileSystem [fat12|fat16|fat32]")]
		public string FileSystem
		{
			set
			{
				options.FileSystem = (Utility.BootImage.FileSystem)Enum.Parse(typeof(Utility.BootImage.FileSystem), value, true);
			}
		}

		[Option("blocks")]
		public uint BlockCount
		{
			set { options.BlockCount = Convert.ToUInt32(value); }
			get { return options.BlockCount; }
		}

		[Option("volume-label", HelpText = "Name of the volume")]
		public string VolumeLabel
		{
			set { options.VolumeLabel = value; }
			get { return options.VolumeLabel; }
		}

		[Value(0, HelpText = "A list of files which will be included in the output image file.")]
		public IEnumerable<string> RawFileList
		{
			set
			{
				foreach (var itm in value)
				{
					var ar = itm.Split(',');
					string src = ar[0];
					string dst = null;

					if (ar.Length == 1)
					{
						dst = Path.GetFileName(ar[0]);
					}
					else if (ar.Length >= 2)
					{
						dst = ar[1];
					}

					if (Path.IsPathRooted(src))
					{
						var currDir = Environment.CurrentDirectory;
						src = Path.GetFullPath(Path.Combine(currDir, src));
					}

					options.IncludeFiles.Add(new IncludeFile(src, dst));
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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(" > Output file: ").AppendLine(options.DiskImageFileName);
			sb.Append(" > Input file(s): ").AppendLine(string.Join(", ", new List<string>(options.IncludeFiles.Select(f => f.SourceFileName).ToArray())));
			sb.Append(" > ImageFormat: ").AppendLine(options.ImageFormat.ToString());
			sb.Append(" > FileSystem: ").AppendLine(options.FileSystem.ToString());
			return sb.ToString();
		}
	}
}
