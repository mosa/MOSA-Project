﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System
{
	/// <summary>
	/// The exception that is thrown when type-loading failures occur.
	/// </summary>
	[Serializable]
	public class TypeLoadException : SystemException
	{
		private string typeName;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeLoadException"/> class.
		/// </summary>
		public TypeLoadException()
			: base("A failure has occurred while loading a type.")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeLoadException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public TypeLoadException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeLoadException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the inner parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
		public TypeLoadException(string message, Exception inner)
			: base(message, inner)
		{ }

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public override string Message
		{
			get { return base.Message + " " + this.typeName; }
		}

		public string TypeName
		{
			get { return this.typeName; }
		}
	}
}