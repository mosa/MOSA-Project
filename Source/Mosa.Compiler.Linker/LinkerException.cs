﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Runtime.Serialization;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Indicates linker exceptions, such as unresolved symbols or duplicate symbols.
	/// </summary>
	public class LinkerException : Exception // : CompilationException
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerException"/> class.
		/// </summary>
		public LinkerException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public LinkerException(string message) :
			base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		public LinkerException(string message, Exception inner) :
			base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerException"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected LinkerException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{
		}

		#endregion Construction
	}
}