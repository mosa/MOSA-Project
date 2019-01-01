// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.CodeCompiler
{
	public static class Compiler
	{
		#region Data Members

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static readonly Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private static readonly TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		/// <summary>
		/// The temporary directory
		/// </summary>
		private static string tempDirectory;

		#endregion Data Members

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
		/// Initializes a new instance of the <see cref="Compiler" /> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <returns></returns>
		public static CompilerResults ExecuteCompiler(CompilerSettings settings)
		{
			if (!providerCache.TryGetValue(settings.Language, out CodeDomProvider provider))
			{
				provider = CodeDomProvider.CreateProvider(settings.Language);
				if (provider == null)
					throw new NotSupportedException("The language '" + settings.Language + "' is not supported on this machine.");
				providerCache.Add(settings.Language, provider);
			}

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			var references = new string[settings.References.Count];
			settings.References.CopyTo(references, 0);

			var parameters = new CompilerParameters(references, filename, false)
			{
				CompilerOptions = "/optimize-"
			};

			if (settings.UnsafeCode)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions += " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (settings.DoNotReferenceMscorlib)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions += " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (settings.CodeSource != null)
			{
				return provider.CompileAssemblyFromSource(parameters, settings.CodeSource + settings.AdditionalSource);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#endregion Construction
	}
}
