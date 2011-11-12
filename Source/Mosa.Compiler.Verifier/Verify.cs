/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Loader.PE;
using Mosa.Compiler.Verifier.TableStage;

namespace Mosa.Compiler.Verifier
{
	public class Verify
	{

		public VerifierOptions Options { get; protected set; }
		public IMetadataProvider Metadata { get; protected set; }
		private List<VerificationEntry> entries = new List<VerificationEntry>();

		/// <summary>
		/// Gets a value indicating whether [any errors].
		/// </summary>
		/// <value><c>true</c> if [any errors]; otherwise, <c>false</c>.</value>
		public bool HasErrors
		{
			get
			{
				foreach (VerificationEntry entry in entries)
					if (entry.Type == VerificationType.Error)
						return true;

				return false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Verify"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public Verify(VerifierOptions options)
		{
			this.Options = options;
		}

		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		public bool Run()
		{
			string file = Options.InputFile;

			try
			{
				if (!File.Exists(file))
				{
					AddNonSpecificationError("File not found");
					return false;
				}

				IMetadataModule module = LoadPE(file);
				Metadata = module.Metadata;

				VerifyMetadata();

			}
			catch (Exception e)
			{
				AddNonSpecificationError("Exception thrown", e.ToString());
			}

			return !HasErrors;
		}

		public void AddVerificationEntry(VerificationEntry entry)
		{
			entries.Add(entry);
		}

		protected void AddNonSpecificationError(string error)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, "0", error));
		}

		protected void AddNonSpecificationError(string error, string data)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, "0", error, data));
		}

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

		protected void VerifyMetadata()
		{
			List<BaseVerificationStage> stages = new List<BaseVerificationStage>() { 
				new MetadataTables()
			};

			foreach (BaseVerificationStage stage in stages)
			{
				stage.Run(this);

				if (HasErrors)
					return;
			}
		}

	}
}

