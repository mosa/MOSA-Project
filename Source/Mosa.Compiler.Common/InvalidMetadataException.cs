/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Common
{
	[Serializable]
	public class InvalidMetadataException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		public InvalidMetadataException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidMetadataException(string message)
			: base(message)
		{ }
	}
}