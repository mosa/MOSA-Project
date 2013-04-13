/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.IO;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public enum ExceptionHandlerType : byte
	{
		/// <summary>
		/// A typed exception handler clause.
		/// </summary>
		Exception = 0,

		/// <summary>
		/// An exception filter and handler clause.
		/// </summary>
		Filter = 1,

		/// <summary>
		/// A finally clause.
		/// </summary>
		Finally = 2,

		/// <summary>
		/// A fault clause. This is similar to finally, except its only executed if an exception is/was processed.
		/// </summary>
		Fault = 4
	}

	/// <summary>
	///
	/// </summary>
	public class ExceptionHandlingClause
	{
		/// <summary>
		///
		/// </summary>
		public ExceptionHandlerType ExceptionHandler;

		/// <summary>
		///
		/// </summary>
		public int TryOffset;

		/// <summary>
		///
		/// </summary>
		public int TryLength;

		/// <summary>
		///
		/// </summary>
		public int HandlerOffset;

		/// <summary>
		///
		/// </summary>
		public int HandlerLength;

		/// <summary>
		///
		/// </summary>
		public uint ClassToken;

		/// <summary>
		///
		/// </summary>
		public int FilterOffset;

		/// <summary>
		///
		/// </summary>
		public int TryEnd
		{
			get { return this.TryOffset + this.TryLength; }
		}

		/// <summary>
		///
		/// </summary>
		public int HandlerEnd
		{
			get { return this.HandlerOffset + this.HandlerLength; }
		}

		/// <summary>
		/// Determines whether [is label within try] [the specified label].
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns>
		///   <c>true</c> if [is label within try] [the specified label]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLabelWithinTry(int label)
		{
			return (label >= TryOffset && label < TryEnd);
		}

		/// <summary>
		/// Determines whether [is label within handler] [the specified label].
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns>
		///   <c>true</c> if [is label within handler] [the specified label]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsLabelWithinHandler(int label)
		{
			return (label >= HandlerOffset && label < HandlerEnd);
		}

		/// <summary>
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="isFat">if set to <c>true</c> [is fat].</param>
		public void Read(BinaryReader reader, bool isFat)
		{
			if (!isFat)
			{
				this.ExceptionHandler = (ExceptionHandlerType)reader.ReadInt16();
				this.TryOffset = reader.ReadInt16();
				this.TryLength = reader.ReadByte();
				this.HandlerOffset = reader.ReadInt16();
				this.HandlerLength = reader.ReadByte();
			}
			else
			{
				this.ExceptionHandler = (ExceptionHandlerType)reader.ReadInt32();
				this.TryOffset = reader.ReadInt32();
				this.TryLength = reader.ReadInt32();
				this.HandlerOffset = reader.ReadInt32();
				this.HandlerLength = reader.ReadInt32();
			}

			if (ExceptionHandlerType.Exception == this.ExceptionHandler)
			{
				this.ClassToken = reader.ReadUInt32();
			}
			else if (ExceptionHandlerType.Filter == this.ExceptionHandler)
			{
				this.FilterOffset = reader.ReadInt32();
			}
			else
			{
				reader.ReadInt32();
			}
		}
	}
}