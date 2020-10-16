// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		public ExceptionHandlerType ExceptionHandlerType { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int TryStart { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int TryEnd { get; private set; }

		/// <summary>
		///
		/// </summary>
		public int HandlerStart { get; private set; }

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
		public int? FilterStart { get; private set; }

		public int? FilterEnd
		{
			get { return (ExceptionHandlerType == ExceptionHandlerType.Filter) ? HandlerStart : (int?)null; }
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
			return (label >= TryStart && label < TryEnd);
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
			return (label >= HandlerStart && label < HandlerEnd);
		}

		public MosaExceptionHandler(ExceptionHandlerType ehType, int tryStart, int tryEnd, int handlerStart, int handlerEnd, MosaType type, int? filterOffset)
		{
			ExceptionHandlerType = ehType;

			TryStart = tryStart;
			TryEnd = tryEnd;

			HandlerStart = handlerStart;
			HandlerEnd = handlerEnd;

			Type = type;
			FilterStart = filterOffset;
		}
	}
}
