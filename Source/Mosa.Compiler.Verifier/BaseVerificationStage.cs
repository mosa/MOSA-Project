/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Verifier
{
	public abstract class BaseVerificationStage
	{

		protected Verify verify;
		protected IMetadataProvider metadata;

		public void Run(Verify verify)
		{
			this.verify = verify;
			this.metadata = verify.Metadata;
			Run();
		}

		protected abstract void Run();

		protected void AddSpecificationError(string section, string error, string data)
		{
			verify.AddVerificationEntry(new VerificationEntry(VerificationType.Error, section, error, data));
		}

		protected void AddSpecificationError(string section, string error)
		{
			verify.AddVerificationEntry(new VerificationEntry(VerificationType.Error, section, error));
		}

	}
}

