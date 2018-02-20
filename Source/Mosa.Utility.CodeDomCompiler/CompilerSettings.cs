// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.CodeDomCompiler
{
	public class CompilerSettings
	{
		#region Data Members

		/// <summary>
		/// An array of assembly references to include in the compilation.
		/// </summary>
		private readonly List<string> references;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompilerSettings"/> class.
		/// </summary>
		public CompilerSettings()
		{
			Language = "C#";
			UnsafeCode = true;
			DoNotReferenceMscorlib = true;
			references = new List<string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCompilerSettings"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public CompilerSettings(CompilerSettings settings)
		{
			Language = settings.Language;
			UnsafeCode = settings.UnsafeCode;
			DoNotReferenceMscorlib = settings.DoNotReferenceMscorlib;
			CodeSource = settings.CodeSource;
			AdditionalSource = settings.AdditionalSource;
			references = new List<string>(settings.references);
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string CodeSource { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string AdditionalSource { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		public bool UnsafeCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [do not reference mscorlib].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [do not reference mscorlib]; otherwise, <c>false</c>.
		/// </value>
		public bool DoNotReferenceMscorlib { get; set; }

		/// <summary>
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		public IList<string> References
		{
			get { return references; }
		}

		#endregion Properties

		public void AddReference(string file)
		{
			references.Add(file);
		}

		public bool IsEqual(CompilerSettings other)
		{
			if (other == null)
				return false;

			if (CodeSource != other.CodeSource)
				return false;

			if (AdditionalSource != other.AdditionalSource)
				return false;

			if (UnsafeCode != other.UnsafeCode)
				return false;

			if (Language != other.Language)
				return false;

			if (DoNotReferenceMscorlib != other.DoNotReferenceMscorlib)
				return false;

			if (references.Count != other.references.Count)
				return false;

			foreach (string file in references)
			{
				if (!other.references.Contains(file))
				{
					return false;
				}
			}

			return true;
		}
	}
}
