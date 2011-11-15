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

		protected VerifyAssembly verifyAssembly;
		protected IMetadataProvider metadata;

		public void Run(VerifyAssembly verifyAssembly)
		{
			this.verifyAssembly = verifyAssembly;
			this.metadata = verifyAssembly.Metadata;
			Run();
		}

		protected abstract void Run();

		protected void AddSpecificationError(string section, string error, string description, Token location)
		{
			verifyAssembly.AddVerificationEntry(new VerificationEntry(verifyAssembly.Module.Name, VerificationType.Error, section, error, description, location));
		}

		protected void AddSpecificationError(string section, string error, string description)
		{
			verifyAssembly.AddVerificationEntry(new VerificationEntry(verifyAssembly.Module.Name, VerificationType.Error, section, error, description));
		}

	}
}

