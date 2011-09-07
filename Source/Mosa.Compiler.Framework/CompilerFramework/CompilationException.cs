/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Runtime.Serialization;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class for all compiler and linker exceptions.
	/// </summary>
	[Serializable]
	public class CompilationException : Exception
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilationException"/> class.
		/// </summary>
		public CompilationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public CompilationException(string message) :
			base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		public CompilationException(string message, Exception inner) :
			base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilationException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
		protected CompilationException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{
		}

		#endregion // Construction
	}
}
