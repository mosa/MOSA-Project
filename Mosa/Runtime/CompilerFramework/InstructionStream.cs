/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Mosa.Runtime.CompilerFramework {
	public sealed class InstructionStream : Stream {

		#region Types

		[Flags]
		enum MethodFlags : ushort {
			TinyFormat = 0x02,
			FatFormat = 0x03,
			MoreSections = 0x08,
			InitLocals = 0x10,
			CodeSizeMask = 0xF000,
			HeaderMask = 0x0003
		}

		[Flags]
		enum MethodDataSectionType {
			EHTable = 0x01,
			OptIL = 0x02,
			FatFormat = 0x40,
			MoreSections = 0x80
		}

		#endregion // Types

		#region Data members

		/// <summary>
		/// The CIL stream offset.
		/// </summary>
		private long _startOffset;

		/// <summary>
		/// Stream, which holds the il code to decode.
		/// </summary>
		private Stream _stream;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="NetOS.SharpOS.Runtime.Jit.CILInstructionStream"/>.
		/// </summary>
		/// <param name="assemblyStream">The stream, which represents the IL assembly.</param>
		/// <param name="offset">The offset, where the IL stream starts.</param> 
		public InstructionStream(Stream assemblyStream, long offset)
		{
			// Check preconditions
			if (null == assemblyStream)
				throw new ArgumentNullException(@"assembly");

			// Store the arguments
			_stream = assemblyStream;
			_startOffset = offset;
			_stream.Position = offset;
		}

		#endregion // Construction

		#region Stream Overrides

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void Flush()
		{
			// Do nothing. We can not flush.
		}

		public override long Length
		{
			get { return _stream.Length; }
		}

		public override long Position
		{
			get
			{
				return _stream.Position;
			}
			set
			{
				if (0 > value)
					throw new ArgumentOutOfRangeException(@"value");

				_stream.Position = value;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			// Check preconditions
			if (null == buffer)
				throw new ArgumentNullException(@"buffer");

			return _stream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			// FIXME: Fix the seeking...
			return _stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		#endregion // #region Stream Overrides
	}
}
