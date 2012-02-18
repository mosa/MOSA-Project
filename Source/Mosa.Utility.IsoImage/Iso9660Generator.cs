/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

#define ROCKRIDGE

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.IsoImage
{
	/// <summary>
	/// Generates ISO9660 images
	/// </summary>
	public class Iso9660Generator
	{
		// http://en.wikipedia.org/wiki/ISO_9660
		// http://users.telenet.be/it3.consultants.bvba/handouts/ISO9960.html
		// ECMA-119 ( a.k.a. ISO-9660 ): http://www.ecma-international.org/publications/standards/Ecma-119.htm
		// System Use Sharing Protocol ( Rock Ridge uses this ): http://www.fordi.org/docs/IEEE%20P1281%20-%20System%20Use%20Sharing%20Protocol.pdf
		// Rock Ridge (system use field): http://www.fordi.org/docs/IEEE%20P1282%20-%20Rock%20Ridge%20Interchange%20Protocol.pdf
		// Joliet (UCS-2): http://bmrc.berkeley.edu/people/chaffee/jolspec.html
		// El Torito spec (bootable): http://www.phoenix.com/NR/rdonlyres/98D3219C-9CC9-4DF5-B496-A286D893E36A/0/specscdrom.pdf
		// El Torito supplement: http://littlesvr.ca/isomaster/eltoritosuppl.php

		// this class's current target is to automatically support Rock Ridge and El Torito. Eventually Joliet will be added I'm sure.
		// there's no real need to make them optional, as the specs are designed to be backwards compatible.

		// the only real optional item is the "pedantic" option. When enabled, I plan to force strict adherance to the ISO9660 file Name
		// spec. Specifically:
		// * file names must have ";1" appended to them ( windows violates this )
		// * directory names cannot have "." in them ( some linux distros violate this )
		// * maximum directory depth is 8 ( I can't find any reason why this limitation should exist, and I think one of the specs above relax this anyway )
		// * (I'm thinking something else, too, haven't decided yet...)

		/// <summary>
		/// Constructor
		/// </summary>
		public Iso9660Generator(bool pedantic)
		{
			this.isoRoot = new IsoFolder();
			this.isoRoot.Name = ".";
			this.pedantic = pedantic;
		}

		/// <summary>
		/// sets the volume label of the image
		/// </summary>
		/// <param Name="volumeLabel">the requested volume label</param>
		public void SetVolumeLabel(string volumeLabel)
		{
			// TODO FIXME - fixup label to make sure it's compatible...
			int maxLength = 72 - 41 + 1;

			this.volumeLabel = volumeLabel.Replace('.', '_');
			if (this.volumeLabel.Length > maxLength)
				this.volumeLabel = this.volumeLabel.Substring(0, maxLength);
		}

		/// <summary>
		/// create directory in ISO file
		/// </summary>
		public void MkDir(string path)
		{
			IsoFolder f = this.isoRoot;
			string[] ar = NormalizePath(path).Split('/');
			for (int i = 0; i < ar.Length; i++)
			{
				string key = ar[i].Trim().ToLower();
				if (!f.entries.ContainsKey(key))
				{
					var subf = new IsoFolder();
					subf.Name = ar[i].Trim();
					f.entries[key] = subf;
				}
				IsoEntry e = f.entries[key];
				if (e.IsFile)
				{
					//throw new Exception("cannot create directory \"" + ar[i].Trim() + "\", a file by that Name already exists");
					return; // already exists - silently fail for now
				}
				f = (IsoFolder)e;
			}
		}

		/// <summary>
		/// add a "normal" file to the ISO ( not a boot file )
		/// </summary>
		public void AddFile(string sPath, FileInfo fileInfo)
		{
			AddFileEx(NormalizePath(sPath), fileInfo);
		}

		/// <summary>
		/// add a boot file to the ISO
		/// TODO FIXME - add support for boot images other than x86
		/// </summary>
		public void AddBootFile(string path, FileInfo fileInfo)
		{
			if (this.boot != null)
				throw new Exception("only one boot file can be added to an ISO");

			this.boot = AddFileEx(NormalizePath(path), fileInfo);
			this.boot.BootFile = true;
			this.boot.BootInfoTable = this.bootInfoTable;
		}

		/// <summary>
		/// sets the boot load size, see El Torito spec figure 3 offset 6-7
		/// </summary>
		/// <param Name="bootLoadSize">the number of 512-byte sectors to load during boot</param>
		public void BootLoadSize(short bootLoadSize)
		{
			this.bootLoadSize = bootLoadSize;
		}

		/// <summary>
		/// enables support for a boot info table within the boot image
		/// </summary>
		/// <param Name="bootInfoTable"></param>
		public void SetBootInfoTable(bool bootInfoTable)
		{
			this.bootInfoTable = bootInfoTable;
			if (boot != null)
				boot.BootInfoTable = bootInfoTable;
		}

		/// <summary>
		/// generate the iso image
		/// </summary>
		public void Generate(string isoFileName)
		{
			LogicalBlockSize = 2048;

			this.generator = new Generator(this.pedantic);
			GenerateIso(); // 1st pass to calculate offsets
			using (FileStream stream = File.OpenWrite(isoFileName))
			{
				this.generator.ResetWithFileStream(stream);
				GenerateIso(); // 2nd pass to actually write the data
			}
		}

		private IsoFolder isoRoot;
		private IsoFile boot;
		private bool pedantic;
		private string volumeLabel;
		private short bootLoadSize;
		private bool bootInfoTable;

		/// <summary>
		/// take a path and convert back-slashes to forward slashes and remove trailing slash if it exists
		/// </summary>
		private string NormalizePath(string path)
		{
			// first fix-up path separators
			path = path.Replace('\\', '/').Trim();
			// check for os-specific leading characters and remove...
			if (path[1] == ':') // dos
				path = path.Substring(2);
			if (path[0] == '/') // dos and unix
				path = path.Substring(1);
			// remove trailing slash if necessary
			if (path[path.Length - 1] == '/')
				path = path.Substring(0, path.Length - 1);
			return path;
		}

		/// <summary>
		/// add a file to the ISO ( common implementation - called by AddFile() and AddBootFile() )
		/// </summary>
		private IsoFile AddFileEx(string path, FileInfo fileInfo)
		{
			string key;
			string[] ar = NormalizePath(path).Split('/');
			int i;
			IsoFolder f = this.isoRoot;
			for (i = 0; i < ar.Length - 1; i++)
			{
				key = ar[i].Trim().ToLower();
				if (!f.entries.ContainsKey(key))
				{
					var subf = new IsoFolder();
					subf.Name = ar[i].Trim();
					f.entries[key] = subf;
				}
				IsoEntry e = f.entries[key];
				if (e.IsFile)
				{
					throw new Exception("cannot create directory \"" + ar[i].Trim() + "\", a file by that Name already exists");
					//return;
				}
				f = (IsoFolder)e;
			}
			var x = new IsoFile(fileInfo);
			x.Name = ar[i].Trim();
			key = ar[i].Trim().ToLower();
			if (f.entries.ContainsKey(key))
			{
				//throw new Exception("file or folder by that Name already exists");
				return (IsoFile)f.entries[key]; // just don't add it for now...
			}
			f.entries[key] = x;

			return x;
		}

		private Generator generator;
		private int TotalSize;
		private short LogicalBlockSize;
		//const private short VolumeSetSize; ( don't see a need to customize this value at this time )
		private int PathTableSize;
		private int LPathTable;
		private int MPathTable;
		private int BootCatalog;
		private int ContinuationBlock, ContinuationLength;
		private short NextPathTableEntry;
		private int PrimaryVolumeDescriptor;

		/// <summary>
		/// pass all the data to the Generator object, and record offsets as we go
		/// On 1st pass, no data actually gets written. This helps us calculate the correct block #'s
		/// On 2nd pass, we now have the correct block #'s, and we actually write the data
		/// </summary>
		private void GenerateIso()
		{
			// note - a comment followed by numbers in parenthesis is the Name of the field and the number in parenthesis is the section number in ECMA-119
			this.generator.DupByte(0, 0x8000);

			// BEGIN Volume Descriptors...

			GenPrimaryVolumeDescriptor();

			if (this.boot != null)
				GenBootRecordDescriptor(); // Boot Record MUST be right after PVD ( see El Torito section 2.0 )

			// TODO FIXME - generate any other needed Volume Descriptors here
			GenVolumeDescriptorTerminator();

			// END Volume Descriptors

			if (this.boot != null)
				GenBootCatalog(this.boot);

			GenPathTables();

			NextPathTableEntry = 1;
			GenDirectoryTree(this.isoRoot, this.isoRoot, 2);

#if ROCKRIDGE
			GenContinuationBlock();
#endif

			GenFiles(this.isoRoot);

			TotalSize = this.generator.Index;
		}

		private void GenPrimaryVolumeDescriptor() // ( 8.4 )
		{
			PrimaryVolumeDescriptor = this.generator.Index / LogicalBlockSize;
			var pvd = new FieldValidator(this.generator);
			pvd.Byte(1, 1); // Volume Descriptor Type ( 8.4.1 )
			pvd.AString("CD001", 2, 6); // Standard Identifier ( 8.4.2 )
			pvd.Byte(1, 7); // Volume Descriptor Version ( 8.4.3 )
			pvd.Zero(8, 8); // Unused Field ( 8.4.4 )
			pvd.AString("?", 9, 40); // System Identifier ( 8.4.5 )
			pvd.DString(this.volumeLabel, 41, 72); // Volume Identifier ( 8.4.6 )
			pvd.Zero(73, 80); // Unused Field ( 8.4.7 )
			pvd.IntLSBMSB((TotalSize + LogicalBlockSize - 1) / LogicalBlockSize, 81, 88); // Volume Space Size ( 8.4.8 )
			pvd.Zero(89, 120); // Unused Field ( 8.4.9 )
			pvd.ShortLSBMSB(1, 121, 124); // Volume Set Size ( 8.4.10 )
			pvd.ShortLSBMSB(1, 125, 128); // Volume Sequence Number ( 8.4.11 )
			pvd.ShortLSBMSB(LogicalBlockSize, 129, 132); // Logical Block Size ( 8.4.12 )
			pvd.IntLSBMSB(PathTableSize, 133, 140); // Path Table Size ( 8.4.13 )
			pvd.IntLSB(LPathTable, 141, 144); // L Path Table ( 8.4.14 )
			pvd.IntLSB(0, 145, 148); // Optional L Path Table ( 8.4.15 )
			pvd.IntMSB(MPathTable, 149, 152); // M Path Table ( 8.4.16 )
			pvd.IntMSB(0, 153, 156); // Optional M Path Table ( 8.4.17 )
			pvd.BeginField(157);
			DirectoryRecord(".", this.isoRoot, 1); // Directory Record for Root Directory ( 8.4.18 )
			pvd.EndField(190);
			pvd.DupByte(0x20, 191, 318); // Volume Set Identifier ( 8.4.19 )
			pvd.DupByte(0x20, 319, 446); // Publisher Identifier ( 8.4.20 )
			pvd.DupByte(0x20, 447, 574); // Data Preparer ( 8.4.21 )
			pvd.DupByte(0x20, 575, 702); // Application Identifier ( 8.4.22 )
			pvd.DupByte(0x20, 703, 739); // Copyright File Identifier ( 8.4.23 )
			pvd.DupByte(0x20, 740, 776); // Abstract File Identifier ( 8.4.24 )
			pvd.DupByte(0x20, 777, 813); // Bibliographic File Identifier ( 8.4.25 )
			System.DateTime now = System.DateTime.Now;
			pvd.AnsiDateTime(now, 814, 830); // Volume Creation Date and Time ( 8.4.26 )
			pvd.AnsiDateTime(now, 831, 847); // Volume Modification Date and Time ( 8.4.27 )
			pvd.Zero(848, 864); // Volume Expiration Date and Time ( 8.4.28 )
			pvd.Zero(865, 881); // Volume Effective Date and Time ( 8.4.29 )
			pvd.Byte(1, 882); // File Structure Version ( 8.4.30 )
			pvd.Zero(883, 883); // Reserved ( 8.4.31 )
			pvd.Zero(884, 1395); // Application Use ( 8.4.32 )
			pvd.Zero(1396, 2048); // Reserved for Future Standardization ( 8.4.33 )
		}

		private void GenVolumeDescriptorTerminator() // ISO 9660 - 8.3
		{
			var vdt = new FieldValidator(this.generator);
			vdt.Byte(255, 1); // Volume Descriptor Type ( 8.3.1 )
			vdt.AString("CD001", 2, 6); // Standard Identifier ( 8.4.2 )
			vdt.Byte(1, 7); // Volume Descriptor Version ( 8.4.3 )
			this.generator.FinishBlock();
		}

		private void GenBootRecordDescriptor() // ( ISO 9660 - 8.2, El Torito Figure 7 )
		{
			var br = new FieldValidator(this.generator);
			br.Byte(0, 1); // Volume Descriptor Type ( 8.2.1 ), Boot Record Indicator - must be 0 ( offset 0x00 )
			br.AString("CD001", 2, 6); // Standard Identifier ( 8.2.2 ), ( offset 0x01-0x05 )
			br.Byte(1, 7); // Volume Descriptor Version ( 8.2.3 ), must be 1 for El Torito also ( offset 0x06 )
			br.AString("EL TORITO SPECIFICATION", 8, 39); // Boot System Identifier ( 8.2.4 ), ( offset 0x07-0x26 )
			br.Zero(40, 71); // Boot Identifier ( 8.2.5 ), Unused - must be 0 ( offset 0x27-0x46 )
			br.IntLSB(BootCatalog, 72, 75); // Boot System Use ( 8.2.6 ), Absolute Pointer to first sector of Boot Catalog ( offset 0x47-0x4A )
			this.generator.FinishBlock();
		}

		private void GenBootCatalog(IsoFile f)
		{
			BootCatalog = this.generator.Index / LogicalBlockSize;
			// write validation entry first... see El Torito section 2.1
			var ve = new FieldValidator(this.generator);
			ve.Byte(1, 1); // Header ID, must be 0x01
			ve.Byte(0, 2); // 0 == x86 - TODO FIXME: 1 == PowerPC, 2 == Mac, see El Torito figure 2
			ve.DupByte(0, 3, 4); // Reserved
			ve.DupByte(0, 5, 0x1C); // Manufacturer/Developer of CD-ROM, see El Torito figure 2
			ve.ShortLSB(0x55AA, 0x1D, 0x1E); // checksum - TODO FIXME - allow custom Manufacturer string - calculate checksum dynamically
			ve.Byte(0x55, 0x1F); // 1st Key Byte
			ve.Byte(0xAA, 0x20); // 2nd Key Byte

			// initial/default entry... see El Torito section 2.2 and figure 3
			var ide = new FieldValidator(this.generator);
			ide.Byte(0x88, 1); // 0x88 = bootable, 0x00 = not bootable

			// TODO FIXME - this is extremely hackish... fix me...
			ide.Byte(0, 2); // no emulation
			/*if ( f.fileInfo.Length <= 1182720 )
				ide.Byte(1, 2); // 1.2 meg diskette
			else if ( f.fileInfo.Length <= 1440 * 1024 )
				ide.Byte(2,2); // 1.44 meg diskette
			else if ( f.fileInfo.Length <= 2880 * 1024 )
				ide.Byte(3,2); // 2.88 meg diskette
			else
				ide.Byte(4,2);*/
			// Hard Disk ( drive 80 )

			ide.ShortLSB(0, 3, 4); // Load Segment - 0 == default of 0x7C0
			ide.Byte(0, 5); // System Type, according to El Torito figure 3 this MUST be a copy of the "System Type" from the boot image. In practice this appears to not be the case.
			ide.Byte(0, 6); // Unused, must be 0
			if (this.bootLoadSize == 0)
				this.bootLoadSize = (short)((f.fileInfo.Length - 1) / 0x200 + 1);
			ide.ShortLSB(this.bootLoadSize, 7, 8); // Sector Count
			ide.IntLSB(this.boot.DataBlock, 9, 12); // Logical Block of boot image
			ide.Zero(13, 32); // unused

			this.generator.FinishBlock();
		}

		private void GenPathTables()
		{
			LPathTable = this.generator.Index / LogicalBlockSize;
			GenPathTableEx(this.isoRoot, this.isoRoot, true);
			PathTableSize = this.generator.Index - LPathTable * LogicalBlockSize;
			this.generator.FinishBlock();

			MPathTable = this.generator.Index / LogicalBlockSize;
			GenPathTableEx(this.isoRoot, this.isoRoot, false);
			this.generator.FinishBlock();
		}

		private void GenPathTableEx(IsoFolder parentFolder, IsoFolder thisFolder, bool lsb)
		{
			var di = new FieldValidator(this.generator);
			// Path table record ( ECMA-119 section 9.4 )
			byte[] b_di = this.generator.IsoName(thisFolder.Name, true);
			di.Byte((byte)b_di.Length, 1);
			di.Byte(0, 2); // Extended Attribute Record Length
			if (lsb)
			{
				di.IntLSB(thisFolder.DataBlock, 3, 6); // Location of Extent
				di.ShortLSB(parentFolder.PathTableEntry, 7, 8); // Parent Directory Number
			}
			else
			{
				di.IntMSB(thisFolder.DataBlock, 3, 6); // Location of Extent
				di.ShortMSB(parentFolder.PathTableEntry, 7, 8); // Parent Directory Number
			}
			di.Bytes(b_di, 9, 8 + b_di.Length); // Directory Identifier
			if ((b_di.Length & 1) != 0)
				di.Byte(0, 9 + b_di.Length); // optional padding if LEN_DI is odd

			foreach (KeyValuePair<string, IsoEntry> it in thisFolder.entries)
				if (it.Value.IsFolder)
					GenPathTableEx(thisFolder, (IsoFolder)it.Value, lsb);
		}

		private void GenDirectoryTree(IsoFolder parent_folder, IsoFolder this_folder, byte root)
		{
			this_folder.PathTableEntry = NextPathTableEntry++;
			this_folder.DataBlock = this.generator.Index / LogicalBlockSize;
			DirectoryRecord(".", this_folder, root);
			DirectoryRecord("..", parent_folder, 0);

			foreach (KeyValuePair<string, IsoEntry> it in this_folder.entries)
				DirectoryRecord(it.Value.Name, it.Value, 0);

			this_folder.DataLength = this.generator.Index - this_folder.DataBlock * LogicalBlockSize;
			this.generator.FinishBlock();

			foreach (KeyValuePair<string, IsoEntry> it in this_folder.entries)
				if (it.Value.IsFolder)
					GenDirectoryTree(this_folder, (IsoFolder)it.Value, 0);
		}

		private void GenContinuationBlock()
		{
			ContinuationBlock = this.generator.Index / LogicalBlockSize;
			ContinuationLength = -this.generator.Index; // HACK ALERT: we will add the final offset when done, and that will give us the length

			// ER - Extensions Reference - see P1281 section 5.5
			byte[] b_id = this.generator.Ascii.GetBytes("RRIP_1991A"); // I can't find documentation anywhere for these values, but they came from an ISO I analyzed.
			byte[] b_des = this.generator.Ascii.GetBytes("THE ROCK RIDGE INTERCHANGE PROTOCOL PROVIDES SUPPORT FOR POSIX FILE SYSTEM SEMANTICS");
			byte[] b_src = this.generator.Ascii.GetBytes("PLEASE CONTACT DISC PUBLISHER FOR SPECIFICATION SOURCE.  SEE PUBLISHER IDENTIFIER IN PRIMARY VOLUME DESCRIPTOR FOR CONTACT INFORMATION.");
			var er = new FieldValidator(this.generator);
			er.Byte((byte)'E', 1);
			er.Byte((byte)'R', 2);
			System.Diagnostics.Debug.Assert((9 + b_id.Length + b_des.Length + b_src.Length) < 256);
			er.Byte((byte)(8 + b_id.Length + b_des.Length + b_src.Length), 3);
			er.Byte(1, 4); // version
			er.Byte((byte)b_id.Length, 5);
			er.Byte((byte)b_des.Length, 6);
			er.Byte((byte)b_src.Length, 7);
			er.Byte(1, 8); // extension version ( got this value from peeking at another ISO file )
			er.Bytes(b_id, 9, 9 + b_id.Length - 1);
			er.Bytes(b_des, 9 + b_id.Length, 9 + b_id.Length + b_des.Length - 1);
			er.Bytes(b_src, 9 + b_id.Length + b_des.Length, 9 + b_id.Length + b_des.Length + b_src.Length - 1);

			ContinuationLength += this.generator.Index;
			this.generator.FinishBlock();
		}

		private void GenFiles(IsoFolder thisFolder)
		{
			foreach (KeyValuePair<string, IsoEntry> it in thisFolder.entries)
				if (it.Value.IsFile)
					GenFile((IsoFile)it.Value);
				else
					GenFiles((IsoFolder)it.Value);
		}

		/// <summary>
		///  This function is responsible for generating directory records. However, since more than one directory
		///  record may be required to get all the information out, it defers the actual directory record generation
		///  to a sub-function DirectoryRecordEx().
		/// </summary>
		/// <param Name="name">The real folder Name</param>
		/// <param Name="e">object containing info about the folder</param>
		/// <param Name="root">1==PVD's root entry, 2==root's "." entry, 0==everything else ( special stuff needs to happen for the root entries )</param>
		private void DirectoryRecord(string name, IsoEntry e, byte root)
		{
			byte[] fileName = this.generator.IsoName(name, true);
			string cont = DirectoryRecordEx(fileName, name, e, root, false);
#if ROCKRIDGE
			while (cont.Length > 0)
			{
				cont = DirectoryRecordEx(fileName, cont, e, 0, true);
			}
#endif
		}

		private string DirectoryRecordEx(byte[] fileName, string realName, IsoEntry e, byte root, bool secondPass)
		{
			byte[] /*b_fi,*/ b_su = null;
			/*if (fileName == ".")
			{
				b_fi = new byte[] { 0 };
				realName = "";
			}
			else if (fileName == "..")
			{
				b_fi = new byte[] { 1 };
				realName = "";
			}
			else
				b_fi = generator.Ascii.GetBytes(fileName);*/
			byte LEN_FI = (byte)fileName.Length;
			byte LEN_DR = (byte)(33 + LEN_FI);
			bool fi_padding = ((LEN_DR & 1) != 0);
			if (fi_padding) LEN_DR++;
			// as much as I HATE to do it, I have to generate this data in both passes for now.
			// I don't yet understand enough about what and how many DR entries have to be made to figure out how to do it "right"
			byte LEN_SU = 0;
#if ROCKRIDGE
			if (root != 1) // don't generate susp on PVD's root entry...
			{
				b_su = Susp(ref realName, e, root == 2, secondPass, (byte)(255 - LEN_DR));
				if (b_su.Length > 255)
					throw new NotImplementedException("can't yet handle SUSP > 255 bytes");
				LEN_SU = (byte)b_su.Length;
			}
			else
				realName = "";
#endif
			LEN_DR += LEN_SU;

			var dr = new FieldValidator(this.generator);
			dr.Byte(LEN_DR, 1); // Length of Directory Record ( 9.1.1 )
			dr.Byte(0, 2); // Extended Attribute Record Length ( 9.1.2 )
			dr.IntLSBMSB(e.DataBlock, 3, 10); // Location of Extent ( 9.1.3 )
#if true
			// in this test - I round the data length up to the next multiple of 2048, didn't help fix my booting problem though...
			dr.IntLSBMSB(((e.DataLength - 1) / 2048 + 1) * 2048, 11, 18); // Data Length ( 9.1.4 )
#else
			dr.IntLSBMSB(e.DataLength, 11, 18); // Data Length ( 9.1.4 )
#endif
			dr.BinaryDateTime(System.DateTime.Now, 19, 25); // Recording Date and Time ( 9.1.5 )
			byte flags = 0;
			if (e.IsFile)
			{
				IsoFile f = (IsoFile)e;
				if ((f.fileInfo.Attributes & FileAttributes.Hidden) != 0)
					flags |= 1; // hidden
			}
			else
			{
				// TODO FIXME - not supporting hidden folders right now
				//IsoFolder f = (IsoFolder)e;
				//if ((f.dirInfo.Attributes & DirectoryAttributes.Hidden) != 0)
				//    flags |= 1; // hidden
			}
			if (e.IsFolder)
				flags |= 2; // directory
#if false // I'm disabling this because analysing of a working ISO never sets this bit...
			if (real_name.Length == 0)
				flags |= 128; // final
#endif
			dr.Byte(flags, 26); // flags ( 9.1.6 )
			dr.Byte(0, 27); // File Unit Size ( 9.1.7 )
			dr.Byte(0, 28); // Interleave Gap Size ( 9.1.8 )
			dr.ShortLSBMSB(1, 29, 32); // Volume Sequence Number ( 9.1.9 )
			dr.Byte(LEN_FI, 33); // Length of File Identifier ( 9.1.10 )
			dr.Bytes(fileName, 34, 33 + LEN_FI);
			if (fi_padding) dr.Zero(34 + LEN_FI, 34 + LEN_FI);
			if (LEN_SU > 0) dr.Bytes(b_su, LEN_DR - LEN_SU + 1, LEN_DR);

			return realName;
		}

		private byte[] Susp(ref string name, IsoEntry e, bool root, bool secondPass, byte available) // IEEE P1282 SUSP
		{
			MemoryStream m = new MemoryStream();
			bool dots = (name == "." || name == "..");

			if (!secondPass)
			{
				if (root)
				{
					susp_base_sp(m);
					susp_base_ce(m);
				}
				if (e.RrFlags != 0)
					susp_rr_rr(m, e);
			}

			// TODO FIXME - write and generate susp_rr_tf() - timestamps...
			if (dots)
				name = "";
			else if (name.Length > 0)
				name = susp_rr_nm(m, name, available, e);

			if ((m.Length & 1) != 0) m.WriteByte(0); // padding
			byte[] b = m.GetBuffer();
			Array.Resize(ref b, (int)m.Length);
			return b;
		}

		private void susp_base_sp(MemoryStream m) // IEEE P1281 SUSP ( 5.3 )
		{
			int start = (int)m.Length;
			m.WriteByte((byte)'S');
			m.WriteByte((byte)'P');
			m.WriteByte(7); // LEN_SP
			m.WriteByte(1); // Version
			m.WriteByte(0xBE); // 1st checksum byte
			m.WriteByte(0xEF); // 2nd checksum byte
			m.WriteByte(0); // LEN_SKP
			System.Diagnostics.Debug.Assert((m.Length - start) == 7);
		}

		private void susp_base_ce(MemoryStream m) // IEEE P1282 SUSP ( 5.1 )
		{
			int start = (int)m.Length;
			m.WriteByte((byte)'C');
			m.WriteByte((byte)'E');
			m.WriteByte(28); // LEN_CE
			m.WriteByte(1); // version
			m.Write(ConvertTo.Int2LSB(ContinuationBlock), 0, 4);
			m.Write(ConvertTo.Int2MSB(ContinuationBlock), 0, 4);
			m.Write(ConvertTo.Int2LSB(0), 0, 4); // Continuation Offset
			m.Write(ConvertTo.Int2MSB(0), 0, 4); // Continuation Offset
			m.Write(ConvertTo.Int2LSB(ContinuationLength), 0, 4);
			m.Write(ConvertTo.Int2MSB(ContinuationLength), 0, 4);
			System.Diagnostics.Debug.Assert((m.Length - start) == 28);
		}

		private void susp_rr_rr(MemoryStream m, IsoEntry e) // IEEE P1282 Rock Ridge Protocol Identifier ( ? - I can't find this documented, but analysis of iso files reveals it's existence )
		{
			int start = (int)m.Length;
			m.WriteByte((byte)'R');
			m.WriteByte((byte)'R');
			m.WriteByte(5); // LEN_RR
			m.WriteByte(1); // Version
			m.WriteByte(e.RrFlags); // flags
			System.Diagnostics.Debug.Assert((m.Length - start) == 5);
		}

		private string susp_rr_nm(MemoryStream m, string name, byte available, IsoEntry e) // IEEE P1282 Alternate Name ( 4.1.4 )
		{
			int start = (int)m.Length;
			if (m.Length > available)
				throw new Exception("buffer overflow - internal design error");

			available -= (byte)m.Length;
			string cont = "";
			byte flags = 0;
			if (name == ".")
				flags |= 2;
			else if (name == "..")
				flags |= 4;
			else if (name.Length > (available - 5))
			{
				flags |= 1; // need another entry...
				cont = name.Substring(available - 5);
				name = name.Substring(0, available - 5);
			}

			e.RrFlags |= 0x08;
			m.WriteByte((byte)'N');
			m.WriteByte((byte)'M');
			m.WriteByte((byte)(name.Length + 5));
			m.WriteByte(1); // version?
			m.WriteByte(flags);
			m.Write(this.generator.Ascii.GetBytes(name), 0, name.Length);
			System.Diagnostics.Debug.Assert((m.Length - start) == (name.Length + 5));
			return cont;
		}

		private void GenFile(IsoFile f)
		{
			f.DataBlock = this.generator.Index / LogicalBlockSize;
			f.DataLength = (int)f.fileInfo.Length;
			this.generator.WriteFile(f, PrimaryVolumeDescriptor);
			this.generator.FinishBlock();
		}

	}
}
