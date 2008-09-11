/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

namespace Mosa.FileSystem.FATFileSystem
{
    /// <summary>
    /// 
    /// </summary>
	public class FATFileStream : Stream
	{
        /// <summary>
        /// 
        /// </summary>
		protected uint startCluster;

        /// <summary>
        /// 
        /// </summary>
		protected uint currentCluster;
        
        /// <summary>
        /// 
        /// </summary>
		protected uint directorySector;
        
        /// <summary>
        /// 
        /// </summary>
		protected uint directoryIndex;

        /// <summary>
        /// 
        /// </summary>
		protected uint nthCluster;

        /// <summary>
        /// 
        /// </summary>
		protected long position;

        /// <summary>
        /// 
        /// </summary>
		protected long length;

        /// <summary>
        /// 
        /// </summary>
		protected long lengthOnDisk;

        /// <summary>
        /// 
        /// </summary>
		protected bool read;

        /// <summary>
        /// 
        /// </summary>
		protected bool write;

        /// <summary>
        /// 
        /// </summary>
		protected byte[] data;

        /// <summary>
        /// 
        /// </summary>
		protected bool dirty;

        /// <summary>
        /// 
        /// </summary>
		protected uint clusterSize;

        /// <summary>
        /// 
        /// </summary>
		protected FAT fs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="startCluster"></param>
        /// <param name="directorySector"></param>
        /// <param name="directoryIndex"></param>
		public FATFileStream(FAT fs, uint startCluster, uint directorySector, uint directoryIndex)
		{
			this.fs = fs;
			this.clusterSize = fs.ClusterSizeInBytes;
			this.data = new byte[clusterSize];
			this.startCluster = startCluster;
			this.directorySector = directorySector;
			this.directoryIndex = directoryIndex;
			this.read = true;
			this.write = false;
			this.position = -1;
			this.dirty = false;

			this.nthCluster = UInt32.MaxValue; // Not positioned yet 

			this.lengthOnDisk = fs.GetFileSize(directorySector, directoryIndex);
			this.length = this.lengthOnDisk;

			if (length != 0)
				ReadCluster(startCluster);
		}
        
        /// <summary>
        /// 
        /// </summary>
		public override bool CanRead
		{
			get
			{
				return read;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override bool CanWrite
		{
			get
			{
				return write;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override bool CanTimeout
		{
			get
			{
				return false;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override long Length
		{
			get
			{
				return length;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				Seek((long)value, SeekOrigin.Begin);
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public override void Flush()
		{
			if (!dirty)
				return;

			fs.WriteCluster(currentCluster, data);

			SetLength(length);

			dirty = false;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (position >= length)
				return -1;	// EOF

			int index = 0;

			// very slow
			for (; (position < length) && (index < count); index++)
				buffer[offset + index] = (byte)ReadByte();

			return index;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override int ReadByte()
		{
			if (position >= length)
				return -1;	// EOF

			if (position < 0)
				position = 0;

			uint index = (uint)((uint)position % clusterSize); // BUG WORKAROUND: inner (uint) is because long drive is not supported yet

			position++;

			byte b = data[index];

			if (index == clusterSize) {
				if (position < length)
					NextCluster();
			}

			return b;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			long newposition = position;

			switch (origin) {
				case SeekOrigin.Begin: newposition = offset; break;
				case SeekOrigin.Current: newposition = position + offset; break;
				case SeekOrigin.End: newposition = length + offset; break;
			}

			// find cluster number of new position
			uint newNthCluster = (uint)((uint)newposition / clusterSize);		// BUG WORKAROUND: inner (uint) is because long is not supported yet
			uint currentNthCluster = (uint)((uint)position / clusterSize);		// BUG WORKAROUND: inner (uint) is because long is not supported yet
			int diff = (int)(newNthCluster - currentNthCluster);

			uint newcluster = 0;

			if (newNthCluster == currentNthCluster) {
				newcluster = currentCluster;
			}
			else
				if (newNthCluster > currentNthCluster) {
					newcluster = fs.FindNthCluster(currentCluster, (uint)diff);
					currentNthCluster = currentNthCluster + (uint)diff;
				}
				else
					if (newNthCluster < currentNthCluster) {
						newcluster = fs.FindNthCluster(this.startCluster, newNthCluster);
						currentNthCluster = newNthCluster;
					}

			ReadCluster(newcluster);
			position = newposition;
			return position;
		}

        /// <summary>
        /// 
        /// </summary>
		protected void NextCluster()
		{
			uint newcluster = fs.NextCluster(currentCluster);
			ReadCluster(newcluster);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cluster"></param>
		protected void ReadCluster(uint cluster)
		{
			if (currentCluster == cluster)
				return;

			Flush();

			currentCluster = cluster;
			fs.ReadCluster(cluster, data);
			dirty = false;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
		public override void SetLength(long value)
		{
			// TODO: incomplete

			if (value == lengthOnDisk)
				return;

			// incomplete here

			lengthOnDisk = value;

			return;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			// TODO
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
		public override void WriteByte(byte value)
		{
			// TODO
		}
	}
}