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
		public int TryEnd { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerOffset { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerEnd { get; private set; }

		/// <summary>
		///
		/// </summary>
		public MosaType Type { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int? FilterOffset { get; private set; }

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

		public MosaExceptionHandler(
			ExceptionHandlerType ehType,
			int tryOffset, int tryEnd,
			int handlerOffset, int handlerEnd,
			MosaType type, int? filterOffset)
		{
			HandlerType = ehType;

			TryOffset = tryOffset;
			TryEnd = tryEnd;

			HandlerOffset = handlerOffset;
			HandlerEnd = handlerEnd;

			Type = type;
			FilterOffset = filterOffset;
		}
	}
}