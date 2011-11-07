/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Verifier
{

	public class VerifierOptions
	{

		#region Properties

		/// <summary>
		/// Gets or sets the input file.
		/// </summary>
		/// <value>The input file.</value>
		public string InputFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [metadata validation].
		/// </summary>
		/// <value><c>true</c> if [metadata validation]; otherwise, <c>false</c>.</value>
		public bool MetadataValidation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [IL validation].
		/// </summary>
		/// <value><c>true</c> if [IL validation]; otherwise, <c>false</c>.</value>
		public bool ILValidation { get; set; }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VerifierOptions"/> class.
		/// </summary>
		public VerifierOptions()
		{
			MetadataValidation = true;
			ILValidation = true;
		}
	}
}
