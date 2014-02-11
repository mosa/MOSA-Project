/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class ExceptionBlock
	{
		/// <summary>
		///
		/// </summary>
		public ExceptionBlockType ExceptionHandler;

		/// <summary>
		///
		/// </summary>
		public int TryOffset { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public int TryLength { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerOffset { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerLength { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public MosaType Type { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public int FilterOffset { get; internal set; }

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
	}
}