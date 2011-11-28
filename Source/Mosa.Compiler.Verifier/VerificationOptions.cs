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

	public class VerificationOptions
	{

		#region Properties

		/// <summary>
		/// Gets or sets the input file.
		/// </summary>
		/// <value>The input file.</value>
		public string InputFile { get; set; }

		/// <summary>
		/// Gets or sets the paths.
		/// </summary>
		/// <value>The paths.</value>
		public IList<string> Paths { get; set;}

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

		/// <summary>
		/// Gets or sets a value indicating whether [including warnings].
		/// </summary>
		/// <value><c>true</c> if [including warnings]; otherwise, <c>false</c>.</value>
		public bool IncludingWarnings { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [mosa specific validation] to performed. 
		/// This might might be necessary when MOSA does not implement a specific feature and 
		/// wish the verifier to detected it early.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [mosa specific validation]; otherwise, <c>false</c>.
		/// </value>
		public bool MosaSpecificValidation { get; set; }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VerificationOptions"/> class.
		/// </summary>
		public VerificationOptions()
		{
			Paths = new List<string>();
			MetadataValidation = true;
			ILValidation = true;
			IncludingWarnings = true;
			MosaSpecificValidation = true;
		}
	}
}
