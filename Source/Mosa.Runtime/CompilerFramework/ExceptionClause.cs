/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public enum ExceptionClauseType : byte
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
	public class ExceptionClause
	{
		/// <summary>
		/// 
		/// </summary>
		private int label;
		/// <summary>
		/// 
		/// </summary>
		public ExceptionClauseType Kind;
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
		public int ClassToken;
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
		/// Reads the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="isFat">if set to <c>true</c> [is fat].</param>
		public void Read(BinaryReader reader, bool isFat)
		{
			if (!isFat)
			{
				this.Kind = (ExceptionClauseType)reader.ReadInt16();
				this.TryOffset = reader.ReadInt16();
				this.TryLength = reader.ReadByte();
				this.HandlerOffset = reader.ReadInt16();
				this.HandlerLength = reader.ReadByte();
			}
			else
			{
				this.Kind = (ExceptionClauseType)reader.ReadInt32();
				this.TryOffset = reader.ReadInt32();
				this.TryLength = reader.ReadInt32();
				this.HandlerOffset = reader.ReadInt32();
				this.HandlerLength = reader.ReadInt32();
			}

			if (ExceptionClauseType.Exception == this.Kind)
			{
				this.ClassToken = reader.ReadInt32();
			}
			else if (ExceptionClauseType.Filter == this.Kind)
			{
				this.FilterOffset = reader.ReadInt32();
			}
		}

		/// <summary>
		/// Updates the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		public void Update(Context context, Stream stream)
		{
			UpdateOffset(context, stream);
			UpdateEnd(context, stream);
		}

		/// <summary>
		/// Updates the offset.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		public void UpdateOffset(Context context, Stream stream)
		{
			if (context.Label == this.TryOffset)
			{
				this.TryOffset = (int)stream.Position;
				this.label = this.TryOffset;
			}
			else if (context.Label == this.HandlerOffset)
			{
				this.HandlerOffset = (int)stream.Position;
				this.label = this.HandlerOffset;
			}
		}

		/// <summary>
		/// Updates the end.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		public void UpdateEnd(Context context, Stream stream)
		{
			if (context.Label == this.TryEnd)
			{
				this.TryLength = (int)stream.Position - this.TryOffset;
				this.label = this.TryEnd;
			}
			if (context.Label == this.HandlerEnd)
			{
				this.HandlerLength = (int)stream.Position - this.HandlerOffset;
				this.label = this.HandlerLength;
			}
		}

		/// <summary>
		/// Adds the label to code stream.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		public void AddLabelToCodeStream(ICodeEmitter emitter)
		{
			emitter.Label(0x50000000 + this.label);
		}
	}
}
