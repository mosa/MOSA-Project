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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public enum EhClauseType : byte
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
	public class EhClause
	{
		/// <summary>
		/// 
		/// </summary>
		private int label;
		/// <summary>
		/// 
		/// </summary>
		public EhClauseType Kind;
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
				this.Kind = (EhClauseType)reader.ReadInt16();
				this.TryOffset = reader.ReadInt16();
				this.TryLength = reader.ReadByte();
				this.HandlerOffset = reader.ReadInt16();
				this.HandlerLength = reader.ReadByte();
			}
			else
			{
				this.Kind = (EhClauseType)reader.ReadInt32();
				this.TryOffset = reader.ReadInt32();
				this.TryLength = reader.ReadInt32();
				this.HandlerOffset = reader.ReadInt32();
				this.HandlerLength = reader.ReadInt32();
			}

			if (EhClauseType.Exception == this.Kind)
			{
				this.ClassToken = reader.ReadInt32();
			}
			else if (EhClauseType.Filter == this.Kind)
			{
				this.FilterOffset = reader.ReadInt32();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public bool LinkBlockToClause(Context context, BasicBlock block)
		{
			int label = context.Label;

			if (this.TryOffset == label || this.TryEnd == label ||
				this.HandlerOffset == label || this.HandlerEnd == label)
			{
				block.ExceptionHeaderClause = this;
				return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="stream"></param>
		public void Update(Context context, Stream stream)
		{
			UpdateOffset(context, stream);
			UpdateEnd(context, stream);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="stream"></param>
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
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="stream"></param>
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
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="emitter"></param>
		public void AddLabelToCodeStream(ICodeEmitter emitter)
		{
			emitter.Label(0x50000000 + this.label);
		}
	}
}
