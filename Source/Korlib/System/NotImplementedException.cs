// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Implementation of the "System.NotImplementedException" class
	/// </summary>
	[Serializable]
	public class NotImplementedException : SystemException
	{
		private readonly string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="NotImplementedException"/> class.
		/// </summary>
		public NotImplementedException()
			: base("A Not Implemented exception was thrown.")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="NotImplementedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public NotImplementedException(string message)
			: base(message)
		{ }
	}
}
