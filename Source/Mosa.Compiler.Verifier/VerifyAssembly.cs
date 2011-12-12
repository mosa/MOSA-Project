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

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Verifier.TableStage;

namespace Mosa.Compiler.Verifier
{
	public class VerifyAssembly
	{

		public VerificationOptions Options { get; protected set; }
		internal IMetadataProvider Metadata { get; private set; }
		internal IMetadataModule Module { get; private set; }
		internal Verify Verify { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Verify"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public VerifyAssembly(Verify verify, VerificationOptions options, IMetadataModule module)
		{
			this.Verify = verify;
			this.Options = options;
			this.Module = module;
			this.Metadata = Module.Metadata;
		}

		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		public bool Run()
		{
			try
			{
				VerifyMetadata();
			}
			catch (Exception e)
			{
				AddNonSpecificationError("Exception thrown", e.ToString());
			}

			return !Verify.HasError;
		}

		public void AddVerificationEntry(VerificationEntry entry)
		{
			Verify.AddVerificationEntry(entry);
		}

		protected void AddNonSpecificationError(string error, string data)
		{
			Verify.AddVerificationEntry(new VerificationEntry(Module.Name, VerificationType.Error, "0", error, data));
		}

		protected void VerifyMetadata()
		{
			List<BaseVerificationStage> stages = new List<BaseVerificationStage>() { 
				new MetadataTables()
			};

			foreach (BaseVerificationStage stage in stages)
			{
				stage.Run(this);

				if (Verify.HasError)
					return;
			}
		}

	}
}

