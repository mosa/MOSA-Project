/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.IO;

using Mosa.Compiler.Metadata.Loader;

namespace Mosa.Compiler.Verifier
{
	public class Verify
	{
		protected VerificationOptions options { get; private set; }

		protected IAssemblyLoader assemblyLoader { get; private set; }

		protected List<VerificationEntry> entries = new List<VerificationEntry>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Verify"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public Verify(VerificationOptions options)
		{
			this.options = options;
			assemblyLoader = new AssemblyLoader();

			assemblyLoader.AddPrivatePath(Path.GetDirectoryName(options.InputFile));
			assemblyLoader.InitializePrivatePaths(options.Paths);
			assemblyLoader.LoadModule(options.InputFile);

			HasError = false;
		}

		public IEnumerable<VerificationEntry> Entries
		{
			get
			{
				foreach (VerificationEntry entry in entries)
					yield return entry;
			}
		}

		internal void AddVerificationEntry(VerificationEntry entry)
		{
			entries.Add(entry);
			if (entry.Type == VerificationType.Error)
				HasError = true;
		}

		/// <summary>
		/// Gets a value indicating whether [any errors].
		/// </summary>
		/// <value><c>true</c> if [any errors]; otherwise, <c>false</c>.</value>
		public bool HasError { get; private set; }

		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		public bool Run()
		{
			foreach (IMetadataModule module in assemblyLoader.Modules)
			{
				VerifyAssembly verifyAssembly = new VerifyAssembly(this, options, module);

				verifyAssembly.Run();
			}

			return !HasError;
		}
	}
}