/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class FatFileStream : System.IO.Stream
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
		protected uint directorySectorIndex;

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
		protected FatFileSystem fs;

		/// <summary>
		/// Initializes a new instance of the <see cref="FatFileStream"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="location">The location.</param>
		public FatFileStream(FatFileSystem fs, FatFileLocation location)
			: this(fs, location.FirstCluster, location.DirectorySector, location.DirectorySectorIndex)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FatFileStream"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="startCluster">The start cluster.</param>
		/// <param name="directorySector">The directory sector.</param>
		/// <param name="directorySectorIndex">Index of the directory sector.</param>
		public FatFileStream(FatFileSystem fs, uint startCluster, uint directorySector, uint directorySectorIndex)
		{
			this.fs = fs;
			this.clusterSize = fs.ClusterSizeInBytes;
			this.data = new byte[clusterSize];
			this.startCluster = startCluster;
			this.directorySector = directorySector;
			this.directorySectorIndex = directorySectorIndex;
			this.read = true;
			this.write = false;
			this.position = 0;
			this.dirty = false;

			this.nthCluster = System.UInt32.MaxValue; // Not positioned yet

			this.lengthOnDisk = fs.GetFileSize(directorySector, directorySectorIndex);
			this.length = this.lengthOnDisk;

			currentCluster = 0;
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports reading; otherwise, false.
		/// </returns>
		public override bool CanRead { get { return read; } }

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports seeking; otherwise, false.
		/// </returns>
		public override bool CanSeek { get { return true; } }

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports writing; otherwise, false.
		/// </returns>
		public override bool CanWrite { get { return write; } }

		/// <summary>
		/// Gets a value that determines whether the current stream can time out.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A value that determines whether the current stream can time out.
		/// </returns>
		public override bool CanTimeout { get { return false; } }

		/// <summary>
		/// When overridden in a derived class, gets the length in bytes of the stream.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A long value representing the length of the stream in bytes.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// A class derived from Stream does not support seeking.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Length { get { return length; } }

		/// <summary>
		/// When overridden in a derived class, gets or sets the position within the current stream.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The current position within the stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support seeking.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				Seek((long)value, System.IO.SeekOrigin.Begin);
			}
		}

		/// <summary>
		/// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		public override void Flush()
		{
			if (!dirty)
				return;

			fs.WriteCluster(currentCluster, data);
			SetLength(length);

			dirty = false;
		}

		/// <summary>
		/// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support reading.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
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
		/// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>
		/// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support reading.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override int ReadByte()
		{
			if (position >= length)
				return -1;	// EOF

			uint index = (uint)(position % clusterSize);

			if (index == 0)
				NextCluster();

			byte b = data[index];
			position++;

			return b;
		}

		/// <summary>
		/// When overridden in a derived class, sets the position within the current stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
		/// <returns>
		/// The new position within the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			// FIXME: off-by-one bug when new position modulus 512 = 0

			long newposition = position;

			switch (origin)
			{
				case System.IO.SeekOrigin.Begin: newposition = offset; break;
				case System.IO.SeekOrigin.Current: newposition = position + offset; break;
				case System.IO.SeekOrigin.End: newposition = length + offset; break;
			}

			// find cluster number of new position
			uint newNthCluster = (uint)(newposition / clusterSize);
			uint currentNthCluster = (uint)(position / clusterSize);
			int diff = (int)(newNthCluster - currentNthCluster);

			uint newCluster = 0;

			if (newNthCluster == currentNthCluster)
			{
				newCluster = currentCluster;
			}
			else
				if (newNthCluster > currentNthCluster)
				{
					newCluster = fs.FindNthCluster(currentCluster, (uint)diff);
					currentNthCluster = currentNthCluster + (uint)diff;
				}
				else
					if (newNthCluster < currentNthCluster)
					{
						newCluster = fs.FindNthCluster(this.startCluster, newNthCluster);
						currentNthCluster = newNthCluster;
					}

			ReadCluster(newCluster);
			position = newposition;
			return position;
		}

		/// <summary>
		/// Gets the next cluster.
		/// </summary>
		protected bool NextCluster()
		{
			uint newcluster = 0;

			if (currentCluster == 0)
				newcluster = startCluster;
			else
				newcluster = fs.GetNextCluster(currentCluster);

			ReadCluster(newcluster);

			return true;
		}

		/// <summary>
		/// Gets the next cluster
		/// </summary>
		/// <returns></returns>
		protected bool NextClusterExpand()
		{
			Flush();

			uint newcluster = 0;

			if (currentCluster == 0)
			{
				if (startCluster == 0)
				{
					uint newCluster = fs.AllocateFirstCluster(directorySector, directorySectorIndex);

					if (newCluster == 0)
						return false;

					startCluster = newCluster;
					currentCluster = newCluster;
					dirty = true;

					// Clear cluster
					for (int i = 0; i < clusterSize; i++)
						data[i] = 0;

					return true;
				}

				newcluster = startCluster;
			}
			else
			{
				newcluster = fs.GetNextCluster(currentCluster);

				if (newcluster == 0)
				{
					uint newCluster = fs.AddCluster(currentCluster);

					if (newCluster == 0)
						return false;

					currentCluster = newCluster;
					dirty = true;

					// Clear cluster
					for (int i = 0; i < clusterSize; i++)
						data[i] = 0;

					return true;
				}
			}

			ReadCluster(newcluster);

			return true;
		}

		/// <summary>
		/// Reads the cluster.
		/// </summary>
		/// <param name="cluster">The cluster.</param>
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
		/// When overridden in a derived class, sets the length of the current stream.
		/// </summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override void SetLength(long value)
		{
			if (value == lengthOnDisk)
				return;

			lengthOnDisk = value;

			fs.UpdateLength((uint)lengthOnDisk, startCluster, directorySector, directorySectorIndex);
		}

		/// <summary>
		/// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support writing.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if ((count == 0) || (buffer.Length == 0))
				return;

			if (buffer.Length - offset < count)
				count = buffer.Length - offset;

			for (int i = 0; i < count; i++)
			{
				WriteByte(buffer[offset + i]);
			}

			//dirty = true;

			//uint remaining = (uint)count;

			//while (remaining != 0)
			//{
			//    uint clusterIndex = (uint)(position % clusterSize);

			//    if (clusterIndex == 0)
			//        NextClusterExpand();

			//    uint clusterAvailable = clusterSize - clusterIndex;

			//    uint size = System.Math.Min(remaining, clusterAvailable);

			//    System.Array.Copy(buffer, offset, data, clusterIndex, size);

			//    offset += (int)size;
			//    position += size;
			//    remaining -= size;
			//}
		}

		/// <summary>
		/// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
		/// </summary>
		/// <param name="value">The byte to write to the stream.</param>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support writing, or the stream is already closed.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override void WriteByte(byte value)
		{
			uint index = (uint)(position % clusterSize);

			if (index == 0)
				NextClusterExpand();

			dirty = true;
			data[index] = value;
			position++;

			if (position > length)
				length = position;
		}
	}
}