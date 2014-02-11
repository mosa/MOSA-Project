/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	///
	/// </summary>
	public class ExceptionHandlingClause
	{
		/// <summary>
		///
		/// </summary>
		public ExceptionHandlerType ExceptionHandlerType;

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
		public Token ClassToken;

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
				this.ExceptionHandlerType = (ExceptionHandlerType)reader.ReadInt16();
				this.TryOffset = reader.ReadInt16();
				this.TryLength = reader.ReadByte();
				this.HandlerOffset = reader.ReadInt16();
				this.HandlerLength = reader.ReadByte();
			}
			else
			{
				this.ExceptionHandlerType = (ExceptionHandlerType)reader.ReadInt32();
				this.TryOffset = reader.ReadInt32();
				this.TryLength = reader.ReadInt32();
				this.HandlerOffset = reader.ReadInt32();
				this.HandlerLength = reader.ReadInt32();
			}

			if (ExceptionHandlerType.Exception == this.ExceptionHandlerType)
			{
				this.ClassToken = new Token(reader.ReadUInt32());
			}
			else if (ExceptionHandlerType.Filter == this.ExceptionHandlerType)
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