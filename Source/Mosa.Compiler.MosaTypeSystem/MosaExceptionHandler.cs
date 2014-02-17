/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using dnlib.DotNet.Emit;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class MosaExceptionHandler
	{
		/// <summary>
		///
		/// </summary>
		public ExceptionHandlerType HandlerType { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int TryOffset { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerOffset { get; private set; }

		/// <summary>
		///
		/// </summary>
		public MosaType Type { get; internal set; }

		/// <summary>
		///
		/// </summary>
		public int FilterOffset { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int TryEnd { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerEnd { get; private set; }

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

		internal MosaExceptionHandler(ExceptionHandler handler)
		{
			HandlerType = (ExceptionHandlerType)handler.HandlerType;

			TryOffset = (int)handler.TryStart.Offset;
			TryEnd = (int)handler.TryEnd.Offset;

			HandlerOffset = (int)handler.HandlerStart.Offset;
			HandlerEnd = (int)handler.HandlerEnd.Offset;

			if (handler.FilterStart != null)
				FilterOffset = (int)handler.FilterStart.Offset;
		}
	}
}