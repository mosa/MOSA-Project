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
using System.IO;
using System.Text;

namespace Mosa.Utility.IsoImage
{

	/// <summary>
	/// This class is responsible for encoding types of data to the filestream and also keeping track
	/// of our current file position.
	/// </summary>
	internal class Generator
	{
		private bool pedantic;
		public int Index;
		private FileStream fs;
		public ASCIIEncoding Ascii;

		public Generator(bool pedantic)
		{
			this.pedantic = pedantic;
			this.Index = 0;
			this.fs = null;
			this.Ascii = new System.Text.ASCIIEncoding();
		}

		public void ResetWithFileStream(FileStream fs)
		{
			this.Index = 0;
			this.fs = fs;
		}

		/// <summary>
		/// This function converts a normal Name into an iso9660-compatible file Name.
		/// TODO FIXME: check the pedantic setting...
		/// </summary>
		/// <param Name="name">the normal file Name</param>
		/// <param Name="folder">if this is a Name for a folder</param>
		/// <returns>the iso9660-compatible version of the file Name</returns>
		public byte[] IsoName(string name, bool folder)
		{
			if (folder)
			{
				if (name == ".")
					return new byte[] { 0 };
				if (name == "..")
					return new byte[] { 1 };
#if ROCKRIDGE
				if (this.pedantic)
					name = name.Replace('.', '_');
#endif
			}
#if ROCKRIDGE
			name = name.ToUpper();
#endif
			string ext = "";
			int dot = name.LastIndexOf('.');
			if (dot >= 0)
			{
				ext = name.Substring(dot);
				name = name.Substring(0, dot);
			}
#if ROCKRIDGE
			name = name.Substring(0, System.Math.Min(name.Length, 8)).Replace('.', '_').Replace(' ', '_');
			ext = ext.Substring(0, System.Math.Min(ext.Length, 4));
#else
				// make sure Name isn't too long, 1st pass ( trying to preserve extension )
				int total = name.Length + ext.Length;
				const int max_length = (256 - 34);
				if (total > max_length)
				{
					int new_length = name.Length - (total - max_length);
					if ( new_length >= 0 )
						name = name.Substring ( 0, new_length );
				}
#endif
			name += ext;

#if !ROCKRIDGE
				// make sure Name isn't too long, 2nd pass ( if 1st pass failed, then extension is too long - sacrifice it now )
				if (name.Length > max_length)
					name = name.Substring(0, max_length);
#endif

			return this.Ascii.GetBytes(name);
		}

		public void FinishBlock()
		{
			int extra = (-this.Index) % 2048;
			if (extra < 0)
				extra += 2048;
			DupByte(0, extra);
		}

		public void DupByte(byte b, int count)
		{
			this.Index += count;
			if (this.fs == null) return;
			while (count-- > 0)
				this.fs.WriteByte(b);
		}

		public void Bytes(byte[] b)
		{
			Bytes(b, 0, b.Length);
		}

		public void Bytes(byte[] b, int offset, int bytes)
		{
			this.Index += bytes;
			if (this.fs == null) return;
			this.fs.Write(b, offset, bytes);
		}

		public void String(string s)
		{
			Bytes(this.Ascii.GetBytes(s));
		}

		/// <summary>
		/// writes a file out to cd. Generate the Boot Info Table if necessary
		/// </summary>
		/// <param Name="f">the file to write</param>
		/// <param Name="primaryVolumeDescriptor">the PVD block, passed in case we need to generate the Boot Info Table</param>
		public void WriteFile(IsoFile f, int primaryVolumeDescriptor)
		{
			if (f.fileInfo.Length > 0xffffffff)
				throw new NotImplementedException(">4G files not implemented");

			int bytes = (int)f.fileInfo.Length;
			if (this.fs == null)
			{
				this.Index += bytes;
				return;
			}

			// TODO FIXME - create smaller reusable buffer and read fixed-size chunks at a time...
			byte[] b = new byte[bytes];

			using (FileStream stream = f.fileInfo.OpenRead())
			{
				if (bytes != stream.Read(b, 0, bytes))
					throw new Exception("number of bytes read from file != reported length of file: " + f.fileInfo.Name);
			}

			if (f.BootInfoTable)
			{
				// TODO FIXME - this is TERRIBLE. This should be implemented at a higher level, and
				// doing it here requires passing the primaryVolumeDescriptor to every call of WriteFile()
				// The reason it is here is because this is the only place where the file actually gets
				// pulled into memory and I didn't want to modify the boot image on-disk like mkisofs does.
				Bytes(b, 0, 8);
				Bytes(ConvertTo.Int2LSB(primaryVolumeDescriptor));
				Bytes(ConvertTo.Int2LSB(f.DataBlock));
				Bytes(ConvertTo.Int2LSB((int)f.fileInfo.Length));
				Bytes(ConvertTo.Int2LSB(0)); // TODO FIXME - checksum
				DupByte(0, 40); // reserved
				Bytes(b, 64, bytes - 64);
			}
			else
				Bytes(b);
		}
	}
}
