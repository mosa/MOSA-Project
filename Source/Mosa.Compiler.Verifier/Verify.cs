
using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Loader.PE;

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
			string file = options.InputFile;
			
			try
			{
				if (!File.Exists(file))
				{
					AddNonSpecificationError("File not found");
					return false;
				}

				IMetadataModule module = LoadPE(file);

				// TODO
			}
			catch (Exception e)
			{
				AddNonSpecificationError("Exception thrown", e.ToString());
			}

			return !HasErrors;
		}

		#region Error Helpers
	
		protected void AddNonSpecificationError(string error)
		{
			errors.Add(new VerificationError("0", error));
		}

		protected void AddNonSpecificationError(string error, string data)
		{
			errors.Add(new VerificationError("0", error, data));
		}

		protected void AddError(string section, string error, string data)
		{
			errors.Add(new VerificationError(section, error, data));
		}

		protected void AddError(string section, string error)
		{
			errors.Add(new VerificationError(section, error));
		}

		#endregion

		protected PortableExecutableImage LoadPE(string file)
		{
			try
			{
				Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
				PortableExecutableImage peImage = new PortableExecutableImage(stream);

				return peImage;
			}
			catch
			{
				AddNonSpecificationError("Unable to load PE image", file);
				throw;
			}

		}
	}
}
