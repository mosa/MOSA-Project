
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Verifier
{
	public class Verify
	{

		private VerifierOptions options;
		private List<VerificationError> errors;

		/// <summary>
		/// Gets a value indicating whether [any errors].
		/// </summary>
		/// <value><c>true</c> if [any errors]; otherwise, <c>false</c>.</value>
		public bool HasErrors { get { return errors.Count != 0; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Verify"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public Verify(VerifierOptions options)
		{
			this.options = options;
			errors = new List<VerificationError>();
		}

		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		public bool Run()
		{

			if (!File.Exists(options.InputFile))
			{
				errors.Add(new VerificationError("0", "File not found"));
				return false;
			}

			// TODO

			return !HasErrors;
		}

	}
}
