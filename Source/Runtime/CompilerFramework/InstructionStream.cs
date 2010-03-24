/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;

namespace Mosa.Runtime.CompilerFramework {
    /// <summary>
    /// 
    /// </summary>
	public sealed class InstructionStream : Stream {

		#region Types

        /// <summary>
        /// 
        /// </summary>
		[Flags]
		enum MethodFlags : ushort {
            /// <summary>
            /// 
            /// </summary>
			TinyFormat = 0x02,
            /// <summary>
            /// 
            /// </summary>
			FatFormat = 0x03,
            /// <summary>
            /// 
            /// </summary>
			MoreSections = 0x08,
            /// <summary>
            /// 
            /// </summary>
			InitLocals = 0x10,
            /// <summary>
            /// 
            /// </summary>
			CodeSizeMask = 0xF000,
            /// <summary>
            /// 
            /// </summary>
			HeaderMask = 0x0003
		}

        /// <summary>
        /// 
        /// </summary>
		[Flags]
		enum MethodDataSectionType {
            /// <summary>
            /// 
            /// </summary>
			EHTable = 0x01,
            /// <summary>
            /// 
            /// </summary>
			OptIL = 0x02,
            /// <summary>
            /// 
            /// </summary>
			FatFormat = 0x40,
            /// <summary>
            /// 
            /// </summary>
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
        /// Initializes a new instance of the <see cref="InstructionStream"/> class.
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

        /// <summary>
		/// Returns true if the current stream is able to read
        /// </summary>
        /// <value></value>
        /// <returns>
		/// True if the current stream is able to read, false otherwise
        /// </returns>
		public override bool CanRead
		{
			get { return true; }
		}

        /// <summary>
		/// Returns true if the current stream is able to seek
        /// </summary>
        /// <value></value>
        /// <returns>
		/// Returns true if the current stream is able to seek
        /// </returns>
		public override bool CanSeek
		{
			get { return true; }
		}

        /// <summary>
        /// Ruft beim Überschreiben in einer abgeleiteten Klasse einen Wert ab, der angibt, ob der aktuelle Stream Schreibvorgänge unterstützt.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// true, wenn der Stream Schreibvorgänge unterstützt, andernfalls false.
        /// </returns>
		public override bool CanWrite
		{
			get { return false; }
		}

        /// <summary>
        /// Löscht beim Überschreiben in einer abgeleiteten Klasse alle Puffer für diesen Stream und veranlasst die Ausgabe aller gepufferten Daten an das zugrunde liegende Gerät.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">
        /// Ein E/A-Fehler tritt auf.
        /// </exception>
		public override void Flush()
		{
			// Do nothing. We can not flush.
		}

        /// <summary>
		/// Gets the stream's length in bytes.
        /// </summary>
        /// <value></value>
        /// <returns>
		/// The stream's length in bytes.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
		/// A class derived from Stream does not support searching.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
		/// Methods have been called after the stream has been closed.
        /// </exception>
		public override long Length
		{
			get { return _stream.Length; }
		}

        /// <summary>
		/// Gets the stream's current position or sets it.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current position in the stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// IOException
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The stream does not support searching.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
		/// Methods have been called after the stream has been closed.
        /// </exception>
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

        /// <summary>
        /// Liest beim Überschreiben in einer abgeleiteten Klasse eine Folge von Bytes aus dem aktuellen Stream und erhöht die Position im Stream um die Anzahl der gelesenen Bytes.
        /// </summary>
        /// <param name="buffer">Ein Bytearray. Nach dem Beenden dieser Methode enthält der Puffer das angegebene Bytearray mit den Werten zwischen <paramref name="offset"/> und (<paramref name="offset"/> + <paramref name="count"/> - 1), die durch aus der aktuellen Quelle gelesene Bytes ersetzt wurden.</param>
        /// <param name="offset">Der nullbasierte Byteoffset im <paramref name="buffer"/>, ab dem die aus dem aktuellen Stream gelesenen Daten gespeichert werden.</param>
        /// <param name="count">Die maximale Anzahl an Bytes, die aus dem aktuellen Stream gelesen werden sollen.</param>
        /// <returns>
        /// Die Gesamtanzahl der in den Puffer gelesenen Bytes. Dies kann weniger als die Anzahl der angeforderten Bytes sein, wenn diese Anzahl an Bytes derzeit nicht verfügbar ist, oder 0, wenn das Ende des Streams erreicht ist.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// Die Summe aus <paramref name="offset"/> und <paramref name="count"/> ist größer als die Pufferlänge.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> ist null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> oder <paramref name="count"/> ist negativ.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// Ein E/A-Fehler tritt auf.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// Der Stream unterstützt keine Lesevorgänge.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Es wurden Methoden aufgerufen, nachdem der Stream geschlossen wurde.
        /// </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			// Check preconditions
			if (null == buffer)
				throw new ArgumentNullException(@"buffer");

			return _stream.Read(buffer, offset, count);
		}

        /// <summary>
        /// Legt beim Überschreiben in einer abgeleiteten Klasse die Position im aktuellen Stream fest.
        /// </summary>
        /// <param name="offset">Ein Byteoffset relativ zum <paramref name="origin"/>-Parameter.</param>
        /// <param name="origin">Ein Wert vom Typ <see cref="T:System.IO.SeekOrigin"/>, der den Bezugspunkt angibt, von dem aus die neue Position ermittelt wird.</param>
        /// <returns>
        /// Die neue Position innerhalb des aktuellen Streams.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">
        /// Ein E/A-Fehler tritt auf.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// Der Stream unterstützt keine Suchvorgänge. Dies ist beispielsweise der Fall, wenn der Stream aus einer Pipe- oder Konsolenausgabe erstellt wird.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Es wurden Methoden aufgerufen, nachdem der Stream geschlossen wurde.
        /// </exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			// FIXME: Fix the seeking...
			return _stream.Seek(offset, origin);
		}

        /// <summary>
        /// Legt beim Überschreiben in einer abgeleiteten Klasse die Länge des aktuellen Streams fest.
        /// </summary>
        /// <param name="value">Die gewünschte Länge des aktuellen Streams in Bytes.</param>
        /// <exception cref="T:System.IO.IOException">
        /// Ein E/A-Fehler tritt auf.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// Der Stream unterstützt nicht sowohl Lese- als auch Schreibvorgänge. Dies ist beispielsweise der Fall, wenn der Stream aus einer Pipe- oder Konsolenausgabe erstellt wird.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Es wurden Methoden aufgerufen, nachdem der Stream geschlossen wurde.
        /// </exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

        /// <summary>
        /// Schreibt beim Überschreiben in einer abgeleiteten Klasse eine Folge von Bytes in den aktuellen Stream und erhöht die aktuelle Position im Stream um die Anzahl der geschriebenen Bytes.
        /// </summary>
        /// <param name="buffer">Ein Bytearray. Diese Methode kopiert <paramref name="count"/> Bytes aus dem <paramref name="buffer"/> in den aktuellen Stream.</param>
        /// <param name="offset">Der nullbasierte Byteoffset im <paramref name="buffer"/>, ab dem Bytes in den aktuellen Stream kopiert werden.</param>
        /// <param name="count">Die Anzahl an Bytes, die in den aktuellen Stream geschrieben werden sollen.</param>
        /// <exception cref="T:System.ArgumentException">
        /// Die Summe aus <paramref name="offset"/> und <paramref name="count"/> ist größer als die Pufferlänge.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> ist null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> oder <paramref name="count"/> ist negativ.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// Ein E/A-Fehler tritt auf.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// Der Stream unterstützt keine Schreibvorgänge.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// Es wurden Methoden aufgerufen, nachdem der Stream geschlossen wurde.
        /// </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		#endregion // #region Stream Overrides
	}
}
