// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.CodeDomCompiler
{
	public static class Compiler
	{
		#region Data members

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		/// <summary>
		///
		/// </summary>
		private static string tempDirectory;

		#endregion Data members

		#region Properties

		private static string TempDirectory
		{
			get
			{
				if (tempDirectory == null)
				{
					tempDirectory = Path.Combine(Path.GetTempPath(), "mosa");
					if (!Directory.Exists(tempDirectory))
					{
						Directory.CreateDirectory(tempDirectory);
					}
				}
				return tempDirectory;
			}
		}

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler"/> class.
		/// </summary>
		public static CompilerResults ExecuteCompiler(CompilerSettings settings)
		{
			CodeDomProvider provider;
			if (!providerCache.TryGetValue(settings.Language, out provider))
			{
				provider = CodeDomProvider.CreateProvider(settings.Language);
				if (provider == null)
					throw new NotSupportedException("The language '" + settings.Language + "' is not supported on this machine.");
				providerCache.Add(settings.Language, provider);
			}

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			string[] references = new string[settings.References.Count];
			settings.References.CopyTo(references, 0);

			CompilerParameters parameters = new CompilerParameters(references, filename, false);
			parameters.CompilerOptions = "/optimize-";

			if (settings.UnsafeCode)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (settings.DoNotReferenceMscorlib)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (settings.CodeSource != null)
			{
				CompilerResults compileResults = provider.CompileAssemblyFromSource(parameters, settings.CodeSource + settings.AdditionalSource);

				return compileResults;
			}
			else
				throw new NotSupportedException();
		}

		#endregion Construction
	}
}
